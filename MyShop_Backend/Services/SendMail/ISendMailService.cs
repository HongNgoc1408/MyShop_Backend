namespace MyShop_Backend.Services.SendMailServices
{
	public interface ISendMailService
	{
		Task SendEmailAsync(string email, string subject, string htmlMessage);
	}
}
