using Demo.DAL.Models;
using Twilio.Rest.Api.V2010.Account;

namespace Demo.PL.Helper
{
	public interface ISmsService
	{
		public MessageResource send(SmsMessage sms);
	}
}
