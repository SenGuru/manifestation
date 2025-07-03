#pragma warning disable CS0108,SA1502,S4015
namespace Manifest.Common.Models.ViewModels
{
    public class SavedJobNotificationEmailCreateModel : SavedJobNotificationEmailUpdateModel
    {
        protected long SavedJobNotificationEmailId { get { return base.SavedJobNotificationEmailId; } set { base.SavedJobNotificationEmailId = value; } }
    }
}
#pragma warning restore CS0108, SA1502, S4015