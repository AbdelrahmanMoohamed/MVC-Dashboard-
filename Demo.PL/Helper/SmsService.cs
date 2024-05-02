using Demo.DAL.Models;
using Demo.PL.Settings;
using Microsoft.Extensions.Options;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace Demo.PL.Helper
{
	public class SmsService : ISmsService
	{
		private TwilioSettings _options;

		public SmsService(IOptions<TwilioSettings> options)
        {
			_options = options.Value;
		}
        public MessageResource send(SmsMessage sms)
		{
			TwilioClient.Init(_options.AccountSID, _options.AuthToken);

			var result = MessageResource.Create(
				body: sms.Body,
				from: new Twilio.Types.PhoneNumber(_options.TwilioPhoneNumber),
				to:sms.PhoneNumber
				);

			return result;

		}
	}
}
