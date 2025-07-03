/**
Copyright (c) 2016 Foundation.IO (https://github.com/foundationio). All rights reserved.

This work is licensed under the terms of the BSD license.
For a copy, see <https://opensource.org/licenses/BSD-3-Clause>.
**/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Framework.Infrastructure.Interfaces.Services;
using Framework.Infrastructure.Models.FileInput;
using Framework.Infrastructure.Models.Result;
using Framework.Infrastructure.Utils;
using MailKit.Security;
using MimeKit;

namespace Framework.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        public async Task<ReturnModel<bool>> SendAsync(string smtpHost, int port, string userName, string password, bool useSSL, string fromDisplayName, string fromEmail, string toDisplayName, string toEmail, string ccEmail, string bccEmail, string subject, string body, List<FileStreamModel> attachments = null, bool useMailKit = false, bool isHtml = false)
        {
            try
            {
                Task<ReturnModel<bool>> sendMailTask;
                if (useMailKit)
                {
                    sendMailTask = SendMailUsingMailKitAsync(smtpHost, port, userName, password, useSSL, fromDisplayName, fromEmail, toDisplayName, toEmail, ccEmail, bccEmail, subject, body, attachments, isHtml);
                }
                else
                {
                    sendMailTask = SendMailUsingSmtpAsync(smtpHost, port, userName, password, useSSL, fromDisplayName, fromEmail, toDisplayName, toEmail, ccEmail, bccEmail, subject, body, attachments, isHtml);
                }

                return await sendMailTask;
            }
            catch (Exception ex)
            {
                return new ReturnModel<bool>("Unexpected error when sending email.", ex);
            }
        }

        public async Task<ReturnModel<bool>> SendMailUsingMailKitAsync(string smtpHost, int port, string userName, string password, bool useSSL, string fromDisplayName, string fromEmail, string toDisplayName, string toEmail, string ccEmail, string bccEmail, string subject, string body, List<FileStreamModel> attachments = null, bool isHtml = false)
        {
            var client = new MailKit.Net.Smtp.SmtpClient();

            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(fromDisplayName, fromEmail));
                message.To.Add(new MailboxAddress(toDisplayName, toEmail));
                message.Subject = subject;

                var bodyBuilder = new BodyBuilder();
                if (isHtml)
                {
                    bodyBuilder.HtmlBody = body;
                }
                else
                {
                    bodyBuilder.TextBody = body;
                }

                message.Body = bodyBuilder.ToMessageBody();

                if (!ccEmail.IsTrimmedStringNullOrEmpty())
                {
                    message.Cc.Add(new MailboxAddress(ccEmail, ccEmail));
                }

                if (attachments != null && attachments.Any())
                {
                    foreach (var attachment in attachments)
                    {
                        // Attachment handling code should be here
                    }
                }

                if (!bccEmail.IsTrimmedStringNullOrEmpty())
                {
                    message.Bcc.Add(new MailboxAddress(bccEmail, bccEmail));
                }

                await client.ConnectAsync(smtpHost, port, useSSL);

                await client.AuthenticateAsync(userName, password);

                string mailSend = await client.SendAsync(message);

                return ReturnModel<bool>.Success(true);
            }
            catch (AuthenticationException authEx)
            {
                return new ReturnModel<bool>($"Authentication failed: {authEx.Message}", authEx);
            }
            catch (MailKit.Net.Smtp.SmtpCommandException cmdEx)
            {
                return new ReturnModel<bool>($"SMTP command error: {cmdEx.Message}, Status: {cmdEx.StatusCode}, ErrorCode: {cmdEx.ErrorCode}", cmdEx);
            }
            catch (MailKit.Net.Smtp.SmtpProtocolException protocolEx)
            {
                return new ReturnModel<bool>($"SMTP protocol error: {protocolEx.Message}", protocolEx);
            }
            catch (Exception ex)
            {
                return new ReturnModel<bool>($"Unexpected error when sending email: {ex.Message}", ex);
            }
            finally
            {
                if (client.IsConnected)
                {
                    await client.DisconnectAsync(true);
                }
            }
        }

        public async Task<ReturnModel<bool>> SendMailUsingSmtpAsync(string smtpHost, int port, string userName, string password, bool useSSL, string fromDisplayName, string fromEmail, string toDisplayName, string toEmail, string ccEmail, string bccEmail, string subject, string body, List<FileStreamModel> attachments = null, bool isHtml = false)
        {
            try
            {
                using (var client = port == 0 ? new SmtpClient(smtpHost) : new SmtpClient(smtpHost, port))
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                    var mmail = new MailMessage(new MailAddress(fromEmail, fromDisplayName), new MailAddress(toEmail, toDisplayName))
                    {
                        IsBodyHtml = isHtml,
                        BodyEncoding = Encoding.UTF8,
                        Body = body
                    };

                    if (attachments != null)
                    {
                        foreach (var att in attachments)
                        {
                            att.FileData.Position = 0;
                            mmail.Attachments.Add(new Attachment(att.FileData, att.FileName, FileUtils.GetMimeTypeFromFileName(att.FileName)));
                        }
                    }

                    mmail.SubjectEncoding = Encoding.UTF8;
                    if (mmail.Subject.IsTrimmedStringNullOrEmpty())
                        mmail.Subject = subject;

                    if (!ccEmail.IsTrimmedStringNullOrEmpty())
                    {
                        mmail.CC.Add(ccEmail);
                    }

                    if (!bccEmail.IsTrimmedStringNullOrEmpty())
                    {
                        mmail.Bcc.Add(bccEmail);
                    }

                    if ((!userName.IsTrimmedStringNullOrEmpty()) && (!password.IsTrimmedStringNullOrEmpty()))
                    {
                        client.UseDefaultCredentials = false;
                        client.Credentials = new NetworkCredential(userName, password);
                    }

                    if (useSSL)
                        client.EnableSsl = true;

                    await client.SendMailAsync(mmail);

                    return new ReturnModel<bool>(true);
                }
            }
            catch (Exception ex)
            {
                return new ReturnModel<bool>("Unexpected error when sending email.", ex);
            }
        }

        // Keep this for backward compatibility, but implement it in terms of the async method
        public ReturnModel<bool> Send(string smtpHost, int port, string userName, string password, bool useSSL, string fromDisplayName, string fromEmail, string toDisplayName, string toEmail, string ccEmail, string bccEmail, string subject, string body, List<FileStreamModel> attachments = null, bool useMailKit = false, bool isHtml = false)
        {
            // Use Task.Run to convert async to sync, but this is not ideal for performance
            return SendAsync(smtpHost, port, userName, password, useSSL, fromDisplayName, fromEmail, toDisplayName, toEmail, ccEmail, bccEmail, subject, body, attachments, useMailKit, isHtml).GetAwaiter().GetResult();
        }
    }
}