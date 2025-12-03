using Serilog;

namespace InternetSafetyPlan.Api;
public class Program
{
    public static void Main(string[] args)
    {
        CreateWebHostBuilder(args)
            .Build()
            .Run();
    }

    public static IHostBuilder CreateWebHostBuilder(string[] args) =>
        Host
            .CreateDefaultBuilder(args)
            .UseSerilog((context, configuration) =>
            {
                configuration.ReadFrom.Configuration(context.Configuration);
            })
        .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
}
