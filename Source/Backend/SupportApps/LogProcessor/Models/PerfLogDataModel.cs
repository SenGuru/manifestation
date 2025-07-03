using CsvHelper.Configuration.Attributes;
using Framework.Infrastructure.Models;

namespace LogProcessor.Models
{
    public class PerfLogDataModel : PerfLogData
    {
        [Ignore]
        public string Id { get; set; }
    }
}
