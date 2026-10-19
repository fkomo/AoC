namespace Ujeby.AoC.Common
{
	public static class Log
	{
		public const string DateTimeFormat = "yyyy-MM-dd_HH:mm:ss.fff";

		public static int Indent { get; set; } = 2;

		public static readonly string OutputFile = Path.Combine(Environment.CurrentDirectory, $"AoC.Output.{DateTime.Now:yyyyMMdd_HHmmssfff}.txt");

		public static void Line(string lineText = null, int? indent = null, ConsoleColor textColor = ConsoleColor.White)
		{
			if (lineText != null)
				PrintIndent(indent);

			Console.ForegroundColor = textColor;

			Console.WriteLine(lineText);
			AddLinesToFile(lineText);

			Console.ForegroundColor = ConsoleColor.White;
		}

		public static void Text(string text, int? indent = null, ConsoleColor textColor = ConsoleColor.White)
		{
			PrintIndent(indent);

			Console.ForegroundColor = textColor;

			Console.Write(text);
			AddTextToFile(text);

			Console.ForegroundColor = ConsoleColor.White;
		}

		static readonly (ConsoleColor Color, char Char)[] _christmasColors =
		[
			//(ConsoleColor.DarkRed, '='),
			(ConsoleColor.Red, '+'),
			(ConsoleColor.DarkGreen, '*'),
			(ConsoleColor.DarkGreen, '#'),
			(ConsoleColor.DarkGreen, '*'),
			(ConsoleColor.DarkGreen, '#'),
			(ConsoleColor.Green, '#'),
			(ConsoleColor.Green, '*'),
			(ConsoleColor.DarkYellow, '&'),
			(ConsoleColor.Yellow, '$'),
			(ConsoleColor.Yellow, '@'),
		];

		static readonly (ConsoleColor Color, char Char)[] _christmasColorsDisabled =
		[
			(ConsoleColor.Gray, '+'),
			(ConsoleColor.DarkGray, '*'),
			(ConsoleColor.DarkGray, '#'),
			(ConsoleColor.DarkGray, '*'),
			(ConsoleColor.DarkGray, '#'),
			(ConsoleColor.Gray, '#'),
			(ConsoleColor.Gray, '*'),
			(ConsoleColor.DarkGray, '&'),
			(ConsoleColor.Gray, '$'),
			(ConsoleColor.Gray, '@'),
		];

		public static void PrintIndent( int? indent = null)
		{
			indent ??= Indent;

			var text = new string(Enumerable.Repeat(' ', indent.Value).ToArray());
			
			Console.Write(text);
			AddTextToFile(text);
		}

		static readonly (ConsoleColor Color, char Char)[] _christmasPattern = new (ConsoleColor, char)[]
		{
			(ConsoleColor.DarkGreen, '*'),
			(ConsoleColor.DarkGreen, '#'),
			(ConsoleColor.DarkGreen, '~'),
			(ConsoleColor.DarkGreen, '='),
			(ConsoleColor.Green, '#'),
			(ConsoleColor.Green, '*'),
			(ConsoleColor.Green, '~'),
			(ConsoleColor.Green, '='),
			(ConsoleColor.DarkYellow, '#'),
			(ConsoleColor.DarkYellow, '*'),
			(ConsoleColor.DarkYellow, '~'),
			(ConsoleColor.DarkYellow, '='),
		};

		public static void ChristmasPattern(string pattern, 
			int? indent = null)
		{
			PrintIndent(indent);

			var rng = new Random((int)DateTime.Now.Ticks);
			foreach (var c in pattern)
			{
				var r = rng.Next(_christmasPattern.Length);
				Console.ForegroundColor = _christmasPattern[r].Color;
				Console.Write(c);
			}
			Console.ForegroundColor = ConsoleColor.White;
		}

		public static void ChristmasText(string text, int? indent = null)
		{
			PrintIndent(indent);

			var rng = new Random((int)DateTime.Now.Ticks);
			foreach (var c in text)
			{
				var r = rng.Next(_christmasColors.Length);
				Console.ForegroundColor = _christmasColors[r].Color;
				Console.Write(c);
			}
			Console.ForegroundColor = ConsoleColor.White;
		}

		public static void ChristmasTextDisabled(string text, int? indent = null)
		{
			PrintIndent(indent);

			var rng = new Random((int)DateTime.Now.Ticks);
			foreach (var c in text)
			{
				var r = rng.Next(_christmasColorsDisabled.Length);
				Console.ForegroundColor = _christmasColorsDisabled[r].Color;
				Console.Write(c);
			}
			Console.ForegroundColor = ConsoleColor.White;
		}

		public static void ChristmasHeader(string text, 
			ConsoleColor textColor = ConsoleColor.White, int length = 80)
		{
			var headerPrefixLength = 6;
			ChristmasPattern(new string(Enumerable.Repeat('■', headerPrefixLength).ToArray()));

			var headerLength = headerPrefixLength + $"[ {text} ]".Length;
			Console.ForegroundColor = ConsoleColor.Green;
			Console.Write($"{{ ");

			Console.ForegroundColor = textColor;
			Console.Write(text);

			Console.ForegroundColor = ConsoleColor.Green;
			Console.Write($" }}");

			ChristmasPattern(new string(Enumerable.Repeat('■', length - headerLength).ToArray()));

			Console.WriteLine();
		}

		static object _fileLock = new();

		static void AddTextToFile(string text)
		{
			lock (_fileLock)
			{
				File.AppendAllText(OutputFile, text);
			}
		}

		static void AddLinesToFile(params string[] lines)
		{
			lock (_fileLock)
			{
				File.AppendAllLines(OutputFile, lines);
			}
		}
	}
}