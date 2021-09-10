using System;
using Flurl;

namespace ASMR.Mobile.Common.Net
{
	public class BackEndUrl : Url
	{
		public static readonly Uri BaseUri = new(AppSettingsManager.Settings.BackEndService.BaseAddress);

		public BackEndUrl() : base(BaseUri)
		{
		}
	}
}