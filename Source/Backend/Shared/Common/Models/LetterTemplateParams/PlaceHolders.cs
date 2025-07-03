using System.Collections.Generic;

namespace Manifest.Common.Models.LetterTemplateParams
{
    public class PlaceHolders
    {
        public const string StartTag = "{";
        public const string EndTag = "}";
        public const string NewLineTag = "<br/>";
        public const string TableRowColumnSplitTag = "<coldelimiter>";

        public PlaceHolders()
        {
            TextPlaceholders = new Dictionary<string, string>();
            TablePlaceholders = new Dictionary<string, LetterTemplateTable>();
        }

        public Dictionary<string, string> TextPlaceholders { get; set; }

        public Dictionary<string, LetterTemplateTable> TablePlaceholders { get; set; }
    }
}
