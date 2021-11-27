using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace ASMR.Web.Extensions;

public static class LocalAppSettingsExtension
{
	public static IHostBuilder UseLocalAppSettings(this IHostBuilder host)
	{
		host.ConfigureAppConfiguration(configure =>
		{
			// Used for local settings like connection strings.
			configure.AddJsonFile("appsettings.Local.json", true);
		});

		return host;
	}
}