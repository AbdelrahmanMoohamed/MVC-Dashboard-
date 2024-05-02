using Demo.DAL.Models;
using Demo.PL.Services;
using Demo.PL.Settings;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Net;
using MailKit.Security;
//using System.Net.Mail;

namespace Demo.PL.Helper
{
	public class EmailSettings : IEmailSettings
	{
		private readonly MailSettings _options;


		//public static void SendEmail(Email email)
		//{
		//	// Mail server : gmail
		//	var client = new SmtpClient("smtp.gmail.com", 587);
		//	client.EnableSsl = true;
		//	client.Credentials = new NetworkCredential("routec41v02@gmail.com", "bqfivjutajhfxgto");

		//	client.Send("routec41v02@gmail.com", email.To, email.Subject, email.Body);

		//}

		public EmailSettings(IOptions<MailSettings> options)
        {
			_options = options.Value;
		}
        public void SendEmail(Email email)
		{
			var mail = new MimeMessage
			{
				Sender= MailboxAddress.Parse(_options.Email),
				Subject=email.Subject
			};

			mail.To.Add(MailboxAddress.Parse(email.To));
			mail.From.Add(new MailboxAddress(_options.DisplayName,_options.Email));

			var builder = new BodyBuilder();
			builder.TextBody = email.Body;
			mail.Body=builder.ToMessageBody();

			using var smtp = new SmtpClient();

			smtp.Connect(_options.Host, _options.Port,SecureSocketOptions.StartTls);

			smtp.Authenticate(_options.Email, _options.Password);
			smtp.Send(mail);

			smtp.Disconnect(true);
		}
	}
}
