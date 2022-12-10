namespace Ujeby.AoC.Common
{
	public static class Log
	{
		public const string DateTimeFormat = "yyyy-MM-dd_HH:mm:ss.fff";

		public static int Indent { get; set; } = 2;

		public static void Line(
			string lineText = null, int? indent = null, ConsoleColor textColor = ConsoleColor.White)
		{
			if (lineText != null)
				PrintIndent(indent);

			Console.ForegroundColor = textColor;
			Console.WriteLine(lineText);
			Console.ForegroundColor = ConsoleColor.White;
		}

		public static void Text(string text,
			int? indent = null, ConsoleColor textColor = ConsoleColor.White)
		{
			PrintIndent(indent);

			Console.ForegroundColor = textColor;
			Console.Write(text);
			Console.ForegroundColor = ConsoleColor.White;
		}

		private static void PrintIndent(
			int? indent = null)
		{
			indent ??= Indent; 
			for (var i = 0; i < indent; i++)
				Console.Write(" ");
		}

		private static ConsoleColor[] _christmasColors = new ConsoleColor[]
		{
			Console.ForegroundColor = ConsoleColor.DarkRed,
			Console.ForegroundColor = ConsoleColor.White,
			Console.ForegroundColor = ConsoleColor.DarkGreen,
			Console.ForegroundColor = ConsoleColor.Red,
			Console.ForegroundColor = ConsoleColor.White,
			Console.ForegroundColor = ConsoleColor.Green,
		};

		public static void ChristmasHeader(string text, 
			ConsoleColor textColor = ConsoleColor.White, int length = 80)
		{
			Console.WriteLine();
			PrintIndent();

			for (var i = 0; i < _christmasColors.Length; i++)
			{
				Console.ForegroundColor = _christmasColors[i % _christmasColors.Length];
				Console.Write("#");
			}

			var headerLength = _christmasColors.Length + $"[ {text} ]".Length;
			Console.ForegroundColor = ConsoleColor.Green;
			Console.Write($"[ ");

			Console.ForegroundColor = textColor;
			Console.Write(text);

			Console.ForegroundColor = ConsoleColor.Green;
			Console.Write($" ]");

			for (var i = 0; i < length - headerLength; i++)
			{
				Console.ForegroundColor = _christmasColors[i % _christmasColors.Length];
				Console.Write("#");
			}
			Console.ForegroundColor = ConsoleColor.White;

			Console.WriteLine();
			Console.WriteLine();
		}
	}
}