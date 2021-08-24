// ReSharper disable InconsistentNaming

using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace ASMR.Web.ReleaseInformation
{
	public static class ReleaseInformationManager
	{
		public static ValueTask<ASMRMobileReleaseInformation> GetASMRMobileReleaseInformation()
		{
			var path = Path.Combine(Directory.GetCurrentDirectory(),
				"ReleaseInformation", "ASMR.Mobile.json");
			if (!File.Exists(path))
			{
				return ValueTask.FromResult<ASMRMobileReleaseInformation>(null);
			}

			var file = File.OpenRead(path);
			return JsonSerializer.DeserializeAsync<ASMRMobileReleaseInformation>(file);
		}
		
		public static ValueTask<ASMRWebReleaseInformation> GetASMRWebReleaseInformation()
		{
			var path = Path.Combine(Directory.GetCurrentDirectory(),
				"ReleaseInformation", "ASMR.Web.json");
			if (!File.Exists(path))
			{
				return ValueTask.FromResult<ASMRWebReleaseInformation>(null);
			}

			var file = File.OpenRead(path);
			return JsonSerializer.DeserializeAsync<ASMRWebReleaseInformation>(file);
		}
	}
}