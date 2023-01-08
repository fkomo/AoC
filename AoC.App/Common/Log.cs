using System.Text;

namespace Ujeby.AoC.Common
{
	public static class Log
	{
		public const string DateTimeFormat = "yyyy-MM-dd_HH:mm:ss.fff";

		public static int Indent { get; set; } = 2;

		public static string OutputFile => Path.Combine(Environment.CurrentDirectory, "output.txt");

		public static void Line(
			string lineText = null, int? indent = null, ConsoleColor textColor = ConsoleColor.White)
		{
			if (lineText != null)
				PrintIndent(indent);

			Console.ForegroundColor = textColor;

			Console.WriteLine(lineText);
			File.AppendAllLines(OutputFile, new string[] { lineText });

			Console.ForegroundColor = ConsoleColor.White;
		}

		public static void Text(string text,
			int? indent = null, ConsoleColor textColor = ConsoleColor.White)
		{
			PrintIndent(indent);

			Console.ForegroundColor = textColor;

			Console.Write(text);
			File.AppendAllText(OutputFile, text);

			Console.ForegroundColor = ConsoleColor.White;
		}

		private static void PrintIndent(
			int? indent = null)
		{
			indent ??= Indent;

			var text = new string(Enumerable.Repeat(' ', indent.Value).ToArray());
			
			Console.Write(text);
			File.AppendAllText(OutputFile, text);
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
			PrintIndent();

			var sb = new StringBuilder();
			for (var i = 0; i < _christmasColors.Length; i++)
			{
				Console.ForegroundColor = _christmasColors[i % _christmasColors.Length];

				Console.Write("#");
				sb.Append('#');
			}

			var headerLength = _christmasColors.Length + $"[ {text} ]".Length;
			Console.ForegroundColor = ConsoleColor.Green;
			Console.Write($"[ ");
			sb.Append("[ ");

			Console.ForegroundColor = textColor;
			Console.Write(text);
			sb.Append(text);

			Console.ForegroundColor = ConsoleColor.Green;
			Console.Write($" ]");
			sb.Append(" ]");

			for (var i = 0; i < length - headerLength; i++)
			{
				Console.ForegroundColor = _christmasColors[i % _christmasColors.Length];
				Console.Write("#");
				sb.Append("#");
			}
			Console.ForegroundColor = ConsoleColor.White;

			File.AppendAllText(OutputFile, sb.ToString());

			Console.WriteLine();
			File.AppendAllLines(OutputFile, new string[] { string.Empty });
		}
	}
}