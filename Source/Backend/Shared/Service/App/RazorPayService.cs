using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Manifest.Service.App
{
    public class RazorPayService
    {
        //private readonly string baseAddress;
        //private readonly string razorpayKey;
        //private readonly string razorpaySecret;
        private readonly string accountNumber;

        public RazorPayService(string razorpayKey, string razorpaySecret, string accountNumber)
        {
            //this.razorpayKey = razorpayKey;
            //this.razorpaySecret = razorpaySecret;
            this.accountNumber = accountNumber;
            //baseAddress = "https://api.razorpay.com/v1/";
            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            //    "Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_razorpayKey}:{_razorpaySecret}")));
        }

        public async Task<string> CreateFundAccountAsync(string upiId, string name)
        {
            using (HttpClient httpClient = new ())
            {
                var fundAccountData = new
                {
                    contact_id = "cont_" + Guid.NewGuid().ToString("N"),
                    account_type = "vpa",
                    vpa = new { address = upiId },
                    name
                };

                var content = new StringContent(JsonSerializer.Serialize(fundAccountData), Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync("fund_accounts", content);
                response.EnsureSuccessStatusCode();

                var responseBody = await response.Content.ReadAsStringAsync();
                var fundAccount = JsonSerializer.Deserialize<Dictionary<string, object>>(responseBody);

                return fundAccount["id"].ToString();
            }
        }

        public async Task SendPaymentToArtistAsync(string fundAccountId, decimal amount, string referenceId)
        {
            using (HttpClient httpClient = new ())
            {
                var payoutData = new
                {
                    account_number = accountNumber,
                    fund_account_id = fundAccountId,
                    amount = (int)(amount * 100), // Amount in paise
                    currency = "INR",
                    mode = "UPI",
                    purpose = "payout",
                    queue_if_low_balance = true,
                    reference_id = referenceId,
                    narration = "Artist Payout"
                };

                var content = new StringContent(JsonSerializer.Serialize(payoutData), Encoding.UTF8, "application/json");
                var idempotencyKey = Guid.NewGuid().ToString();
                content.Headers.Add("X-Payout-Idempotency", idempotencyKey);

                var response = await httpClient.PostAsync("payouts", content);
                response.EnsureSuccessStatusCode();

                var responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Payout created: {responseBody}");
            }
        }

        public async Task SendBulkPaymentsAsync(List<(string upiId, string name, decimal amount)> payments)
        {
            foreach (var payment in payments)
            {
                try
                {
                    string fundAccountId = await CreateFundAccountAsync(payment.upiId, payment.name);
                    if (!string.IsNullOrEmpty(fundAccountId))
                    {
                        string referenceId = "Transaction_" + Guid.NewGuid().ToString("N");
                        await SendPaymentToArtistAsync(fundAccountId, payment.amount, referenceId);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing payment for {payment.name}: {ex.Message}");
                }
            }
        }
    }
}