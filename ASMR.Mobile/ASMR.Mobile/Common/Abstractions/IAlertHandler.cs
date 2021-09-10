namespace ASMR.Mobile.Common.Abstractions
{
	public enum AlertLength
	{
		Short,
		Long
	}
	
	public interface IAlertHandler
	{
		public void Display(string message, AlertLength length = AlertLength.Long);
	}
}