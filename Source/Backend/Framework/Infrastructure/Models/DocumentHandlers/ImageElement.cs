using System.IO;

namespace Framework.Infrastructure.Models.DocumentHandlers
{
    public class ImageElement
    {
        public MemoryStream MemStream { get; set; }

        public double Dpi { get; set; } // Dots per inch
    }
}
