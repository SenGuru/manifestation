#pragma warning disable CS0108,SA1502,S4015
namespace Manifest.Common.Models.ViewModels
{
    public class SmsServerCreateModel : SmsServerUpdateModel
    {
        protected long SmsServerId { get { return base.SmsServerId; } set { base.SmsServerId = value; } }
    }
}
#pragma warning restore CS0108, SA1502, S4015