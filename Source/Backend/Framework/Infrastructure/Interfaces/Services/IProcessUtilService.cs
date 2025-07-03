using Framework.Infrastructure.Models.Result;

namespace Framework.Infrastructure.Interfaces.Services
{
    public interface IProcessUtilService
    {
        string GetDotNetExecutableName();

        ProcessReturnValue ExecuteProcess(string executableName, string commandLineParameters, string workingDirectory, bool captureOutput = false, bool captureErrors = false);
    }
}