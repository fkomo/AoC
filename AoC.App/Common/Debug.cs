namespace Ujeby.AoC.Common
{
	public static class Debug
	{
		public const string DateTimeFormat = "yyyy-MM-dd_HH:mm:ss.fff";

		public static int Indent { get; set; } = 2;

		public static void Line(
			string lineText = null, int? indent = null, ConsoleColor textColor = ConsoleColor.White)
		{
#if _DEBUG_SAMPLE || _DEBUG
			PrintIndent(indent);

			Console.ForegroundColor = textColor;
			Console.WriteLine(lineText);
			Console.ForegroundColor = ConsoleColor.White;
#endif
		}

		public static void Text(string text,
			int? indent = null, ConsoleColor textColor = ConsoleColor.White)
		{
#if _DEBUG_SAMPLE || _DEBUG
			PrintIndent(indent);		

			Console.ForegroundColor = textColor;
			Console.Write(text);
			Console.ForegroundColor = ConsoleColor.White;
#endif
		}

		private static void PrintIndent(
			int? indent = null)
		{
			indent ??= Indent;
			for (var i = 0; i < indent; i++)
				Console.Write(" ");
		}
	}
}