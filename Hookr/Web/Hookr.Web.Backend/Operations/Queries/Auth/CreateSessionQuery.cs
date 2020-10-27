using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Hookr.Web.Backend.Models.Auth;

namespace Hookr.Web.Backend.Operations.Queries.Auth
{
    public class CreateSessionQuery : TelegramUserDto
    {
        [JsonPropertyName("auth_date")] 
        public long AuthDate { get; set; }

        [NotNull] public string? Hash { get; set; }
    }
}