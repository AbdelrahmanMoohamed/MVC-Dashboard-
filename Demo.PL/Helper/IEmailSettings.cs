using Demo.DAL.Models;

namespace Demo.PL.Helper
{
	public interface IEmailSettings
	{
		public void SendEmail(Email email);
	}
}
