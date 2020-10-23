namespace Hookr.Web.Backend.Exceptions.Auth
{
    public class TelegramNotAuthenticatedException : WebAppException
    {
        public TelegramNotAuthenticatedException() : base("Not authenticated")
        {
        }
    }
}