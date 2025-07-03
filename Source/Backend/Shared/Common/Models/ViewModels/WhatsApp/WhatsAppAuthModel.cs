using System.Text.Json.Serialization;

namespace Manifest.Common.Models.ViewModels
{
    public class WhatsAppAuthModel
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }
    }
}
