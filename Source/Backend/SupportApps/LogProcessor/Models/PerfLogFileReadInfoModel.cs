using Framework.Infrastructure.Models;

namespace LogProcessor.Models;

public class PerfLogFileReadInfoModel
{
    public string filePath { get; set; }

    public PerfLogDataModel LastLine { get; set; }

    public int LineCount { get; set; }
}
