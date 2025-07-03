using Framework.Infrastructure.Models;
using LogProcessor.Models;

public class FileData
{
    public int Id { get; set; }

    public string Path { get; set; }

    public AppLogDataModel LastLine { get; set; }

    public int LastLineCount { get; set; }
}
