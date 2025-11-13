#pragma warning disable CS0108,SA1502,S4015
namespace Manifest.Common.Models.ViewModels
{
    public class MailServerCreateModel : MailServerUpdateModel
    {
        protected long MailServerId { get { return base.MailServerId; } set { base.MailServerId = value; } }
    }
}
#pragma warning restore CS0108, SA1502, S4015