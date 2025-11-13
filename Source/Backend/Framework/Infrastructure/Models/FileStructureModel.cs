using Framework.Infrastructure.Utils;

namespace Framework.Infrastructure.Models
{
    public class FileStructureModel
    {
        public string Name { get; set; }

        public string AbsoluteLocation { get; set; }

        public bool IsFolder { get; set; }

        public long? Size { get; set; }

        public string SizeAsString
        {
            get
            {
                if (Size == null)
                    return "";
                return FileUtils.BytesToString(Size.Value);
            }
        }
    }
}