using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Hookr.Web.Backend.Operations.Queries.Auth
{
    public class CreateSessionQuery
    {
        [JsonPropertyName("auth_date")] 
        public long AuthDate { get; set; }

        [JsonPropertyName("first_name")]
        public string? FirstName { get; set; }

        [NotNull] public string? Hash { get; set; }

        public int Id { get; set; }

        [NotNull]
        [JsonPropertyName("photo_url")]
        public string? PhotoUrl { get; set; }

        [NotNull] public string? Username { get; set; }
    }
}