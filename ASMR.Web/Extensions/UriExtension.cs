//
// asmr: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
//
// UriExtension.cs
//

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace ASMR.Web.Extensions;

public static class UriExtension
{
	public static void OpenBrowser(this Uri uri)
	{
		var uriString = uri.ToString();
		if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
		{
			Process.Start(new ProcessStartInfo(uriString) { UseShellExecute = true });
		}
		else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
		{
			Process.Start("xdg-open", uriString);
		}
		else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
		{
			Process.Start("open", uriString);
		}
	}
}