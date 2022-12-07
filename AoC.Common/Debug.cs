namespace Ujeby.AoC.Common
{
	public static class Debug
	{
		public const string DateTimeFormat = "yyyy-MM-dd_HH:mm:ss.fff";

		public static void Line(
			string lineText = null, ConsoleColor textColor = ConsoleColor.White, int indent = 2)
		{
		
			if (lineText != null)
				Indent(indent);

			Console.ForegroundColor = textColor;
			Console.WriteLine(lineText);
			Console.ForegroundColor = ConsoleColor.White;
		}

		public static void Text(string text,
			int indent = 0, ConsoleColor textColor = ConsoleColor.White)
		{
			Indent(indent);

			Console.ForegroundColor = textColor;
			Console.Write(text);
			Console.ForegroundColor = ConsoleColor.White;
		}

		private static void Indent(int count)
		{
			for (var i = 0; i < count; i++)
				Console.Write(" ");
		}


		private static ConsoleColor[] _christmasColors = new ConsoleColor[]
		{
			Console.ForegroundColor = ConsoleColor.Red,
			Console.ForegroundColor = ConsoleColor.White,
			Console.ForegroundColor = ConsoleColor.DarkGreen,
		};

		public static void ChristmasHeader(string text, 
			ConsoleColor textColor = ConsoleColor.White, int indent = 2, int length = 80)
		{
			Line();
			Indent(indent);

			for (var i = 0; i < _christmasColors.Length; i++)
			{
				Console.ForegroundColor = _christmasColors[i % _christmasColors.Length];
				Console.Write("#");
			}

			var headerLength = indent + _christmasColors.Length + $"[ {text} ]".Length;
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