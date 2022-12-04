namespace Ujeby.AoC.Common
{
	public static class Debug
	{
		public const string DateTimeFormat = "yyyy-MM-dd_HH:mm:ss.fff";

		private const bool _outputTimestamp = false;

		public static void Line(string message = null)
		{
			if (_outputTimestamp)
				message = $"[{ DateTime.Now.ToString(DateTimeFormat) }]: " + message;

			Console.WriteLine(message);
		}
	}
}