using System;
using System.Collections.Generic;
using FluentEmail.Core;
using FluentEmail.Core.Models;
using FluentEmail.Razor;
using FluentEmail.SendGrid;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using GreenPOS.Interfaces;
using GreenPOS.Models.EmailTemplates;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using GreenPOS.Common;
using GreenPOS.Models;
using System.Net;
using System.Reflection;
using System.Net.Http;
using System.Linq;

namespace GreenPOS.Service
{
    public class EmailSender : IEmailSender
    {
        private readonly AppSettings _appSettings;

        public EmailSender(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }


        public async Task SendEmailAsync(QuotesViewModel quoteModel)
        {
            if (quoteModel.Documents == null || quoteModel.Documents.Count == 0)
                return;
            var apiKey = _appSettings.SendGridKey;
            // Using Razor templating package (or set using AddRazorRenderer in services)
            Email.DefaultRenderer = new RazorRenderer();
            Email.DefaultSender = new SendGridSender(apiKey);

            //Create a stream for the file
            Stream stream = null;

            var mailAddresses = new List<Address>
            {
                new Address(quoteModel.CustomerEmail)
            };

            try
            {
                var fileNameOfAttachment = quoteModel.Documents.First().Name;
                stream = GetStreamFromUrl(fileNameOfAttachment);
                // var stream = File.OpenRead($"{Directory.GetCurrentDirectory()}/wwwroot/Images/EH10001-Quote-24012020.pdf");
                var emailToSend = Email
               .From("test email")
               .To(mailAddresses)
               .CC(quoteModel.EmailCC ?? string.Empty)
               .Subject("Your Building Quote")
               .UsingTemplateFromFile($"{Directory.GetCurrentDirectory()}/Templates/QuoteTemplate.cshtml", quoteModel) // TODO replace Path with actual data store nd add in config
               .Attach(new FluentEmail.Core.Models.Attachment { Data = stream, Filename = fileNameOfAttachment });

                await emailToSend.SendAsync();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (stream != null)
                {
                    //Close the input stream
                    stream.Close();
                }
            }
        }

        public static Stream  GetStreamFromUrl(string fileName)
        {
            //Create a stream for the file
            Stream stream = null;


            // The number of bytes read
            try
            {
                var client = new HttpClient();
                HttpResponseMessage response = client.GetAsync($"https://dhdocgen.blob.core.windows.net/quotes/{fileName}").GetAwaiter().GetResult();

                stream = response.Content.ReadAsStreamAsync().GetAwaiter().GetResult();
                return stream;

            }
            catch (Exception ex)
            {
                throw ex;
            }
           
        }
       
    }
}
