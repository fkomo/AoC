using System.Reflection;

namespace Ujeby.AoC.Common
{
	public class AdventOfCode
	{
		public const int ConsoleWidth = 99;

		public static void RunAll(int[] years,
			string inputStorage = null, string inputSuffix = null, bool ignoreSkip = false, bool skipSolved = false)
		{
			if (string.IsNullOrEmpty(inputStorage))
				inputStorage = Environment.CurrentDirectory;

			Log.Line($"Using input storage '{inputStorage}'");

			foreach (var aocYear in AppDomain.CurrentDomain.GetAssemblies()
				.Where(a => a.FullName.StartsWith("Ujeby.AoC."))
				.SelectMany(a => a.GetTypes())
				.Where(t => Attribute.IsDefined(t, typeof(AoCPuzzleAttribute)))
				.OrderBy(p => p.GetCustomAttribute<AoCPuzzleAttribute>().Day)
				.GroupBy(p => p.GetCustomAttribute<AoCPuzzleAttribute>().Year))
			{
				if (years?.Any() == true && !years.Contains(aocYear.Key))
					continue;

				Run(aocYear.Key,
					inputStorage,
					inputSuffix,
					skipSolved,
					aocYear.Select(p =>
					{
						var puzzle = (IPuzzle)Activator.CreateInstance(p);
						
						if (ignoreSkip)
							puzzle.Skip = false;

						return puzzle;
					})
					.ToArray());
			}
		}

		public static void Run(int year, string inputStorage, string inputSuffix, bool skipSolved,
			params IPuzzle[] problemsToSolve)
		{
			Log.Line();
			Log.ChristmasHeader($"{AoCHttpClient.BaseUrl}/{year}",
				length: ConsoleWidth);
			Log.Line();

			var stars = 0;
			try
			{
				Log.Indent += 0;
				Debug.Indent += 0;

				foreach (var problem in problemsToSolve)
				{
					if (skipSolved)
					{
						// skip (sample) debugging of solved puzzles
						if (problem.Answer.Part1 != null && problem.Answer.Part2 != null)
							continue;
					}

					stars += problem.Solve(inputStorage, inputSuffix);

				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}
			finally
			{
				Debug.Indent -= 0;
				Log.Indent -= 0;

				Log.Line();
				Log.ChristmasHeader($"{stars}/{problemsToSolve.Length * 2} stars",
					textColor: ConsoleColor.Yellow,
					length: ConsoleWidth);
			}
		}
	}
}
