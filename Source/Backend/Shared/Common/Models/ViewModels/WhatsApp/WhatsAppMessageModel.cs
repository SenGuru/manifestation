using System.Text.Json.Serialization;

namespace Manifest.Common.Models.ViewModels
{
    public class WhatsAppMessageModel
    {
        [JsonPropertyName("messaging_product")]
        public string MessagingProduct { get; set; }

        [JsonPropertyName("recipient_type")]
        public string RecipientType { get; set; }

        [JsonPropertyName("to")]
        public string To { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        public WhatsAppTextMessage Text { get; set; }
    }
}
