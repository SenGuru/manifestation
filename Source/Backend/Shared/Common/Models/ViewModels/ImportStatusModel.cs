using Framework.Infrastructure.Exceptions;

namespace Manifest.Common.Models.ViewModels
{
    public class ImportStatusModel
    {
        public bool IsSucess { get; set; }

        public string Identifier { get; set; }

        public string Message { get; set; }

        public ReturnError ErrorHolder { get; set; }
    }
}
