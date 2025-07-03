using System.Collections.Generic;

namespace Framework.Infrastructure.Models.DocumentHandlers
{
    public class Placeholders
    {
        public Placeholders()
        {
            NewLineTag = "<br/>";
            TextPlaceholderStartTag = "##";
            TextPlaceholderEndTag = "##";
            TablePlaceholderStartTag = "==";
            TablePlaceholderEndTag = "==";
            ImagePlaceholderStartTag = "++";
            ImagePlaceholderEndTag = "++";
            HyperlinkPlaceholderStartTag = "//";
            HyperlinkPlaceholderEndTag = "//";

            TextPlaceholders = new Dictionary<string, string>();
            TablePlaceholders = new List<Dictionary<string, string[]>>();
            ImagePlaceholders = new Dictionary<string, ImageElement>();
            HyperlinkPlaceholders = new Dictionary<string, HyperlinkElement>();
        }

        public string NewLineTag { get; set; }

        public string TextPlaceholderStartTag { get; set; }

        public string TextPlaceholderEndTag { get; set; }

        public Dictionary<string, string> TextPlaceholders { get; set; }

        public string TablePlaceholderStartTag { get; set; }

        public string TablePlaceholderEndTag { get; set; }

        public List<Dictionary<string, string[]>> TablePlaceholders { get; set; }

        public string ImagePlaceholderStartTag { get; set; }

        public string ImagePlaceholderEndTag { get; set; }

        public Dictionary<string, ImageElement> ImagePlaceholders { get; set; }

        public string HyperlinkPlaceholderStartTag { get; set; }

        public string HyperlinkPlaceholderEndTag { get; set; }

        public Dictionary<string, HyperlinkElement> HyperlinkPlaceholders { get; set; }
    }
}
