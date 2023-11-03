using System.Reflection;
using Serilog;

namespace CSharpScriptingWeb.Infrastructure;

public static class ApplicationStartup
{
    public static void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog(ConfigureSerilog);

        builder.Services.AddControllersWithViews();
    }

    public static void ConfigureHttpRequestPipeline(WebApplication app)
    {
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
    }


    //
    // Private methods
    //

    private static void ConfigureSerilog(HostBuilderContext context, IServiceProvider services, LoggerConfiguration loggerConfig)
    {
        var logDirectory = context.HostingEnvironment.IsDevelopment()
            ? Path.Combine(context.HostingEnvironment.ContentRootPath, "Logs")
            : Path.Combine(context.HostingEnvironment.ContentRootPath, "..", "CSharpScriptingWeb_Logs");


        //
        // Configure Serilog and its various sinks.
        //

        // Always write to a rolling file. For bad API request logging ONLY, write to a special file.
        loggerConfig
            .ReadFrom.Configuration(context.Configuration)
            .ReadFrom.Services(services)
            .Enrich.WithProperty("Assembly", Assembly.GetExecutingAssembly().GetName().Name!)
            .Enrich.With<UtcTimestampEnricher>()
            .Enrich.WithMachineName()
            .WriteTo.File(Path.Combine(logDirectory, "log.txt"), outputTemplate: "{UtcTimestamp:yyyy-MM-dd HH:mm:ss.fff} [{MachineName}] [{Level}] [{SourceContext:l}] {Message}{NewLine}{Exception}", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 10);
    }
}
