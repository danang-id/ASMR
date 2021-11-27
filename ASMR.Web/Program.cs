using System;
using System.Diagnostics;
using ASMR.Web;
using ASMR.Web.Data;
using ASMR.Web.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Host
	.UseLocalAppSettings()
	.UseSerilog((hostingContext, loggerConfiguration) =>
	{
		loggerConfiguration
			.ReadFrom.Configuration(hostingContext.Configuration)
			.Enrich.FromLogContext()
			.Enrich.WithProperty("ApplicationName", typeof(Program).Assembly.GetName().Name)
			.Enrich.WithProperty("Environment", hostingContext.HostingEnvironment);

#if DEBUG
		// Used to filter out potentially bad data due debugging.
		// Very useful when doing Seq dashboards and want to remove logs under debugging session.
		loggerConfiguration.Enrich.WithProperty("DebuggerAttached", Debugger.IsAttached);
#endif
	});

var startup = new Startup(builder.Configuration);
startup.ConfigureServices(builder.Services, builder.Environment);

var app = builder.Build();
startup.Configure(app, app.Environment);

try
{
	var scope = app.Services.CreateScope();
	var migrationResult = SelfMigrator.Migrate(scope.ServiceProvider);
	if (!migrationResult)
	{
		throw new Exception("Data migration execution failed.");
	}

	app.Run();
}
catch (Exception exception)
{
	// Log.Logger will likely be internal type "Serilog.Core.Pipeline.SilentLogger".
	if (Log.Logger == null || Log.Logger.GetType().Name == "SilentLogger")
	{
		Log.Logger = new LoggerConfiguration()
			.MinimumLevel.Debug()
			.WriteTo.Console()
			.CreateLogger();
	}

	Log.Fatal(exception, "Host terminated unexpectedly");
}
finally
{
	Log.CloseAndFlush();
}