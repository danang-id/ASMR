using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text.Json;

namespace ASMR.Mobile
{
	public class AppSettingsManager
	{
		private static AppSettingsManager _instance;

		private readonly AppSettings _settings;
		private const string Namespace = "ASMR.Mobile";
		private const string FileName = "appsettings.json";

		private AppSettingsManager()
		{
			try
			{
				var assembly = typeof(AppSettingsManager).GetTypeInfo().Assembly;
				var stream = assembly.GetManifestResourceStream($"{Namespace}.{FileName}");
				using var reader = stream is not null ? new StreamReader(stream) : null;
				var jsonString = reader?.ReadToEnd() ?? "{}";
				_settings = JsonSerializer.Deserialize<AppSettings>(jsonString);
			}
			catch (Exception exception)
			{
				Debug.WriteLine(exception, GetType().Name);
			}
		}

		public static AppSettingsManager Settings => _instance ??= new AppSettingsManager();

		public AppCenterConfiguration AppCenter => _settings?.AppCenter;
		
		public BackEndServiceConfiguration BackEndService => _settings?.BackEndService;
	}
}