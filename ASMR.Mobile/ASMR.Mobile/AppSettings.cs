using System.Text.Json.Serialization;

// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace ASMR.Mobile
{
	public class AppCenterSecretKeysConfiguration
	{
		[JsonPropertyName("Android")]
		public string Android { get; set; }
		
		// ReSharper disable once InconsistentNaming
		[JsonPropertyName("iOS")]
		public string iOS { get; set; }
	}
	
	public class AppCenterConfiguration
	{
		[JsonPropertyName("SecretKeys")]
		public AppCenterSecretKeysConfiguration SecretKeys { get; set; }
	}

	public class BackEndServiceConfiguration
	{
		[JsonPropertyName("BaseAddress")]
		public string BaseAddress { get; set; }
	}
	
	public class AppSettings
	{
		[JsonPropertyName("AppCenter")]
		public AppCenterConfiguration AppCenter { get; set; }
		
		[JsonPropertyName("BackEndService")]
		public BackEndServiceConfiguration BackEndService { get; set; }
	}
}