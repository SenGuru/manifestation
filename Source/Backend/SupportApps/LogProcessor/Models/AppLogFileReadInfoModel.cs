using Framework.Infrastructure.Models;

namespace LogProcessor.Models;

public class AppLogFileReadInfoModel
{
    public string filePath { get; set; }

    public AppLogDataModel LastLine { get; set; }

    public int LineCount { get; set; }
}
