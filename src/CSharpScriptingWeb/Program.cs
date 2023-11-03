using CSharpScriptingWeb.Infrastructure;
using Serilog;

// The initial "bootstrap" logger is able to log errors during start-up. It's completely replaced by the
//   logger configured in `UseSerilog()`, once configuration and dependency-injection have both been
//   set up successfully.
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    ApplicationStartup.ConfigureServices(builder);

    var app = builder.Build();

    ApplicationStartup.ConfigureHttpRequestPipeline(app);

    app.Run();

    return 0;
}
catch (Exception ex)
{
    // Log the exception and let the application terminate. Try to write to Serilog, but in case it's not
    //   yet configured, also write out to the console.
    Log.Fatal(ex, "Host terminated unexpectedly.");

    return -1;
}
finally
{
    // Ensure all logs are written before the application terminates.
    Log.CloseAndFlush();
}
