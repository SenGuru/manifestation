using Framework.Infrastructure.Attributes;
using Manifest.Common.Constants;

namespace Manifest.Tests.Common
{
    public class AppHeaderModelx
    {
        [AlternateName(HeaderContants.QATestFlag)]
        public string IsTest { get; set; }
    }
}
