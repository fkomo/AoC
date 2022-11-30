namespace Ujeby.AoC.Common
{
	public static class Debug
	{
		public static int LineLength = 64;
		static bool _outputTimestamp = true;
		const string DateTimeFormat = "yyyy-MM-dd_HH:mm:ss.fff";

		public static void Line(string message = null)
		{
			if (_outputTimestamp)
				message = $"[{ DateTime.Now.ToString(DateTimeFormat) }]: " + message;

			Console.WriteLine(message);
		}
	}
}
