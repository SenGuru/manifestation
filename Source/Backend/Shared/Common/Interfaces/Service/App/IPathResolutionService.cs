namespace Manifest.Common.Interfaces.Service.App
{
    public interface IPathResolutionService
    {
        string GetExecutablePath(string executableFolder, string executableName);

        string JasperTemplatePath(string templateName);

        string JasperStarterExePath();
    }
}
