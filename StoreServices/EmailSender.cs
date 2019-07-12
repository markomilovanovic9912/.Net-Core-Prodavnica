using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StoreServices

{

        public class EmailSender : IEmailSender
        {
        /*public EmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor)
        {
            Options = optionsAccessor.Value;
        }

        public AuthMessageSenderOptions Options { get; } //set only via Secret Manager*/

          string key = "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";//ZA BUDUCU PROMENU


            public Task SendEmailAsync(string email, string subject, string message)
            {
                return Execute(key, subject, message, email);
            }

            public Task Execute(string apiKey, string subject, string message, string email)
            {
                var client = new SendGridClient(apiKey);
                var msg = new SendGridMessage()
                {
                    From = new EmailAddress("MusicStore@Store.com", "Music Store"),
                    Subject = subject,
                    PlainTextContent = message,
                    HtmlContent = message
                };
                msg.AddTo(new EmailAddress(email));
                return client.SendEmailAsync(msg);
            }
        }



        /*Task IEmailSender.SendEmailAsync(string email, string subject, string message)
        {
            return Task.CompletedTask;
        }*/
    }

