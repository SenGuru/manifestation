using System.IO;

namespace Manifest.Common.Models.ViewModels
{
    public class FileStreamModel
    {
        public string FileName { get; set; }

        public string ContentType { get; set; }

        public Stream FileData { get; set; }
    }
}
