using Framework.Infrastructure.Models;
using LogProcessor.Models;

public class PerfFileData
{
    public int Id { get; set; }
    public string Path { get; set; }
    public PerfLogDataModel LastLine { get; set; }
    public int LastLineCount { get; set; }
}
