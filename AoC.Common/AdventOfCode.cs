using System.Reflection;

namespace Ujeby.AoC.Common
{
	public class AdventOfCode
	{
		public const int ConsoleWidth = 100;

		public static string GetPreset()
		{
			var envFilePath = Path.Combine(AppContext.BaseDirectory, "preset.txt");
			if (File.Exists(envFilePath))
				return File.ReadAllText(envFilePath);

			return null;
		}

		public static string GetSettingsFilename()
		{
			var preset = GetPreset();
			return string.IsNullOrEmpty(preset) ? "appsettings.json" : $"appsettings.{preset}.json";
		}

		public static void RunAll(string[] puzzles,
			string inputStorage = null, string inputSuffix = null, bool ignoreSkip = false)
		{
			if (string.IsNullOrEmpty(inputStorage))
				inputStorage = Environment.CurrentDirectory;

			Log.Line($"Using input storage '{inputStorage}'");

			var puzzleFilters = puzzles.Select(x => x.Split(':')).ToArray();

			static bool EqualOrWild(string left, string right) => 
				left == "*" || left == right || left.Split(',').Select(x => x.Trim()).Contains(right);

			foreach (var aocYear in AppDomain.CurrentDomain.GetAssemblies()
				.Where(a => a.FullName.StartsWith("Ujeby.AoC."))
				.SelectMany(a => a.GetTypes())
				.Where(t => Attribute.IsDefined(t, typeof(AoCPuzzleAttribute)))
				.OrderBy(p => p.GetCustomAttribute<AoCPuzzleAttribute>().Day)
				.GroupBy(p => p.GetCustomAttribute<AoCPuzzleAttribute>().Year))
			{
				if (!puzzleFilters.Any(x => EqualOrWild(x[0], aocYear.Key.ToString())))
					continue;

				var filteredPuzzles = aocYear
					.Select(x =>
					{
						var puzzle = (IPuzzle)Activator.CreateInstance(x);
						if (ignoreSkip)
							puzzle.Skip = false;

						return puzzle;
					})
					.Where(x => puzzleFilters.Any(pf => 
						EqualOrWild(pf[0], x.Year.ToString()) && 
						(EqualOrWild(pf[1], x.Day.ToString()) || (pf[1] == "?" && (x.Answer.Part1 == null || x.Answer.Part2 == null)))))
					.ToArray();

				if (filteredPuzzles.Length == 0)
					continue;

				Run(aocYear.Key, inputStorage, inputSuffix, filteredPuzzles);
			}
		}

		public static void Run(int year, string inputStorage, string inputSuffix, params IPuzzle[] puzzlesToSolve)
		{
			Log.Line();
			Log.ChristmasPattern($"┌──[ ", indent: 2);
			Log.Text($"{AoCHttpClient.BaseUrl}/{year}", indent: 0);
			Log.ChristmasPattern($" ]", indent: 0);
			Log.Line();
			Log.ChristmasPattern("│");
			Log.Line();

			var stars = 0;
			try
			{
				stars = puzzlesToSolve.Sum(x => x.Solve(inputStorage, inputSuffix));
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}
			finally
			{
				Log.ChristmasPattern("│", indent: 2);
				Log.Line();
				Log.ChristmasPattern($"└──[ ", indent: 2);
				Log.Text($"{stars}/{puzzlesToSolve.Length * 2} stars", textColor: ConsoleColor.Yellow, indent: 0);
				Log.ChristmasPattern($" ]", indent: 0);
				Log.Line();
			}
		}

		public static IPuzzle[] AllPuzzles()
			=> AppDomain.CurrentDomain.GetAssemblies()
				.Where(a => a.FullName.StartsWith("Ujeby.AoC."))
				.SelectMany(a => a.GetTypes())
				.Where(t => Attribute.IsDefined(t, typeof(AoCPuzzleAttribute)))
				.OrderBy(p => p.GetCustomAttribute<AoCPuzzleAttribute>().Day)
				.GroupBy(p => p.GetCustomAttribute<AoCPuzzleAttribute>().Year)
				.SelectMany(p => p.Select(x => (IPuzzle)Activator.CreateInstance(x)))
				.ToArray();

		public static IPuzzle GetInstance(int year, int day)
		{
			var puzzle = AppDomain.CurrentDomain.GetAssemblies()
				.Where(a => a.FullName.StartsWith("Ujeby.AoC."))
				.SelectMany(a => a.GetTypes())
				.Where(t => Attribute.IsDefined(t, typeof(AoCPuzzleAttribute)))
				.SingleOrDefault(x =>
					x.GetCustomAttribute<AoCPuzzleAttribute>().Year == year &&
					x.GetCustomAttribute<AoCPuzzleAttribute>().Day == day);

			if (puzzle == null)
				return null;

			return (IPuzzle)Activator.CreateInstance(puzzle);
		}
	}
}
