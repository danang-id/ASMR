using System;
using Flurl;

namespace ASMR.Mobile.Common
{
	public class BackEndUrl : Url
	{
		public static readonly Uri BaseAddress = new(AppSettingsManager.Settings.BackEndService.BaseAddress);

		public BackEndUrl() : base(BaseAddress)
		{
		}
	}
}