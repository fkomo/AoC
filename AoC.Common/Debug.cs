namespace Ujeby.AoC.Common
{
	public static class Debug
	{
		public const string DateTimeFormat = "yyyy-MM-dd_HH:mm:ss.fff";

		public static void Line(
			string lineText = null, ConsoleColor textColor = ConsoleColor.White, int indent = 2)
		{
#if _DEBUG_SAMPLE
			if (lineText != null)
				Indent(indent);

			Console.ForegroundColor = textColor;
			Console.WriteLine(lineText);
			Console.ForegroundColor = ConsoleColor.White;
#endif
		}

		public static void Text(string text,
			int indent = 0, ConsoleColor textColor = ConsoleColor.White)
		{
#if _DEBUG_SAMPLE
			Indent(indent);

			Console.ForegroundColor = textColor;
			Console.Write(text);
			Console.ForegroundColor = ConsoleColor.White;
#endif
		}

		private static void Indent(int count)
		{
			for (var i = 0; i < count; i++)
				Console.Write(" ");
		}
	}
}