#pragma warning disable CS0108,SA1502,S4015

namespace Manifest.Common.Models.ViewModels
{
    public class StateCreateModel : StateUpdateModel
    {
        protected long StateId { get { return base.StateId; } set { base.StateId = value; } }
    }
}
#pragma warning restore CS0108, SA1502, S4015