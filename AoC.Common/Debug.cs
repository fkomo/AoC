namespace Ujeby.AoC.Common
{
	public static class Debug
	{
		public const string DateTimeFormat = "yyyy-MM-dd_HH:mm:ss.fff";

		public static void Line(
			string lineText = null, ConsoleColor textColor = ConsoleColor.White)
		{
#if DEBUG
			Console.ForegroundColor = textColor;
			Console.WriteLine(lineText);
			Console.ForegroundColor = ConsoleColor.White;
#endif
		}

		public static void Text(string text,
			ConsoleColor textColor = ConsoleColor.White)
		{
#if DEBUG
			Console.ForegroundColor = textColor;
			Console.Write(text);
			Console.ForegroundColor = ConsoleColor.White;
#endif
		}
	}
}