using CsvHelper.Configuration.Attributes;
using Framework.Infrastructure.Models;

namespace LogProcessor.Models
{
    public class AppLogDataModel : AppLogData
    {
        [Ignore]
        public string Id { get; set; }
    }
}
