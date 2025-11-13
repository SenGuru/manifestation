using System.Text.Json.Serialization;

namespace Manifest.Common.Models.ViewModels
{
    public class WhatsAppAuthPayloadModel
    {
        [JsonPropertyName("client_id")]
        public string ClientId { get; set; }

        [JsonPropertyName("client_secret")]
        public string ClientSecret { get; set; }

        [JsonPropertyName("grant_type")]
        public string GrantType { get; set; }
    }
}
