﻿using System.Reflection;

namespace Ujeby.AoC.Common
{
	public class AdventOfCode
	{
		public const int ConsoleWidth = 100;

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
			params IPuzzle[] puzzlesToSolve)
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
				for (var i = 0; i < puzzlesToSolve.Length; i++)
				{
					var puzzle = puzzlesToSolve[i];
					if (skipSolved && i != puzzlesToSolve.Length - 1)
					{
						// skip (sample) debugging of solved puzzles
						if (puzzle.Answer.Part1 != null && puzzle.Answer.Part2 != null)
							continue;
					}

					stars += puzzle.Solve(inputStorage, inputSuffix);
				}
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
