#pragma warning disable CS0108,SA1502,S4015
namespace Manifest.Common.Models.ViewModels
{
    public class CompanyStorageConfigurationCreateModel : CompanyStorageConfigurationUpdateModel
    {
        protected long CompanyStorageConfigurationId { get { return base.CompanyStorageConfigurationId; } set { base.CompanyStorageConfigurationId = value; } }
    }
}
#pragma warning restore CS0108, SA1502, S4015