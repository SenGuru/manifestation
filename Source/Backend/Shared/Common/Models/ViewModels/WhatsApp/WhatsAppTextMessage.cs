using System.Text.Json.Serialization;

namespace Manifest.Common.Models.ViewModels
{
    public class WhatsAppTextMessage
    {
        [JsonPropertyName("preview_url")]
        public bool PreviewUrl { get; set; }

        [JsonPropertyName("body")]
        public string Body { get; set; }
    }
}
