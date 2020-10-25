namespace Hookr.Web.Backend.Exceptions.Auth
{
    public class MissingRefreshTokenException : WebAppException
    {
        public MissingRefreshTokenException() : base("There is no any available refresh token")
        {
        }
    }
}