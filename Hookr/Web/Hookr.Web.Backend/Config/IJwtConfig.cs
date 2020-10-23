namespace Hookr.Web.Backend.Config
{
    public interface IJwtConfig
    {
        string Issuer { get; }
        string Audience { get; }
        string Key { get; }
    }
}