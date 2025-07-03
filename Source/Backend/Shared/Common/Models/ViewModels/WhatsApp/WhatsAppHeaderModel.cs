using Framework.Infrastructure.Attributes;

namespace Manifest.Common.Models.ViewModels
{
    public class WhatsAppHeaderModel
    {
        [AlternateName("Authorization")]
        public string Authorization { get; set; }

        [AlternateName("Content-Type")]
        public string ContentType { get; set; }
    }
}
