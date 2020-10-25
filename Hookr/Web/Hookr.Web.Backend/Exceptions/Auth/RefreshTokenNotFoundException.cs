namespace Hookr.Web.Backend.Exceptions.Auth
{
    public class RefreshTokenNotFoundException : WebAppException
    {
        public RefreshTokenNotFoundException() : base("There is no such refresh token")
        {
        }
    }
}