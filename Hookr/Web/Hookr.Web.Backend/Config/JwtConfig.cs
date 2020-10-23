using System.Diagnostics.CodeAnalysis;

namespace Hookr.Web.Backend.Config
{
    public class JwtConfig : IJwtConfig
    {
        [NotNull] public string? Issuer { get; set; }
        [NotNull] public string? Audience { get; set; }
        [NotNull] public string? Key { get; set; }
    }
}