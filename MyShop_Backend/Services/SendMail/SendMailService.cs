﻿using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MailKit.Security;
using Microsoft.AspNetCore.Hosting;


namespace MyShop_Backend.Services.SendMailServices
{
	public class MailSettings
	{
		public string Mail { get; set; }
		public string? DisplayName { get; set; }
		public string Password { get; set; }
		public string Host { get; set; }
		public int Port { get; set; }
	}

	public class SendMailService : ISendMailService
	{
		private readonly MailSettings _settings;
		private readonly IWebHostEnvironment _webHostEnvironment;

		public SendMailService(IOptions<MailSettings> settings, IWebHostEnvironment webHostEnvironment)
		{
			_settings = settings.Value;
			_webHostEnvironment = webHostEnvironment;
		}

		public async Task SendEmailAsync(string email, string subject, string htmlMessage)
		{
			var Message = new MimeMessage();
			Message.Sender = new MailboxAddress(_settings.DisplayName, _settings.Mail);
			Message.From.Add(new MailboxAddress(_settings.DisplayName, _settings.Mail));
			Message.To.Add(MailboxAddress.Parse(email));
			Message.Subject = subject;

			var builder = new BodyBuilder()
			{
				HtmlBody = htmlMessage
			};

			Message.Body = builder.ToMessageBody();

			using (var smtp = new SmtpClient())
			{
				try
				{
					await smtp.ConnectAsync(_settings.Host, _settings.Port, SecureSocketOptions.StartTls);
					await smtp.AuthenticateAsync(_settings.Mail, _settings.Password);
					await smtp.SendAsync(Message);
				}
				catch (Exception ex)
				{
					Directory.CreateDirectory("MailsSave");
					var emailsavefile = string.Format(@"MailsSave/{0}.txt", email + Guid.NewGuid());
					await Message.WriteToAsync(emailsavefile);
					await File.AppendAllTextAsync(emailsavefile, ex.Message);
				}
				await smtp.DisconnectAsync(true);
			}
		}


		public string GetPathOrderConfirm => Path.Combine(_webHostEnvironment.ContentRootPath, "Template", "email.txt");
		public string GetPathProductList => Path.Combine(_webHostEnvironment.ContentRootPath, "Template", "product.txt");
	}
}
