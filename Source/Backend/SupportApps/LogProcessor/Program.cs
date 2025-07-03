using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseUrls(new[] { $"http://0.0.0.0:{ConfigUtils.GetEnvironmentVariableOrDefault(args, "API_PORT", 5285)}" });
                webBuilder.UseStartup<Startup>();
            });
}