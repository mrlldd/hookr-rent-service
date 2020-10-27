using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Hookr.Web.Backend.Models.Auth
{
    public class TelegramUserDto
    {
        [JsonPropertyName("first_name")]
        public string? FirstName { get; set; }

        public int Id { get; set; }

        [NotNull]
        [JsonPropertyName("photo_url")]
        public string? PhotoUrl { get; set; }

        [NotNull] public string? Username { get; set; }
    }
}