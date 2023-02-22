using System.Reflection;
using Ujeby.Tools.ArrayExtensions;
using Ujeby.Tools.StringExtensions;

namespace Ujeby.AoC.Common
{
	public class AdventOfCode
	{
		public const int ConsoleWidth = 99;

		public static void RunAll(
			string inputStorage = null)
		{
			if (string.IsNullOrEmpty(inputStorage))
			{
				inputStorage = Environment.CurrentDirectory;
				Log.Line($"Using input storage '{inputStorage}'");
			}

			foreach (var aocYear in AppDomain.CurrentDomain.GetAssemblies()
				.Where(a => a.FullName.StartsWith("Ujeby.AoC."))
				.SelectMany(a => a.GetTypes())
				.Where(t => Attribute.IsDefined(t, typeof(AoCPuzzleAttribute)))
				.OrderBy(p => p.GetCustomAttribute<AoCPuzzleAttribute>().Day)
				.GroupBy(p => p.GetCustomAttribute<AoCPuzzleAttribute>().Year))
			{
#if !_2022
				if (aocYear.Key == 2022)
					continue;
#endif
#if !_2021
				if (aocYear.Key == 2021)
					continue;
#endif
#if !_2020
				if (aocYear.Key == 2020)
					continue;
#endif
#if !_2019
				if (aocYear.Key == 2019)
					continue;
#endif
#if !_2018
				if (aocYear.Key == 2018)
					continue;
#endif
#if !_2017
				if (aocYear.Key == 2017)
					continue;
#endif
#if !_2016
				if (aocYear.Key == 2016)
					continue;
#endif
#if !_2015
				if (aocYear.Key == 2015)
					continue;
#endif
				Run(aocYear.Key,
					inputStorage,
					aocYear.Select(p => (IPuzzle)Activator.CreateInstance(p))
					.ToArray());
			}
		}

		public static void Run(int year, string inputStorage, 
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
					stars += problem.Solve(inputStorage);
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

		public static void GeneratePuzzleCode(string codePath, int year)
		{
			try
			{
				Log.Indent += 2;

				if (string.IsNullOrEmpty(codePath))
					throw new Exception($"Path to source code ({nameof(codePath)}) not set!");

				for (var day = 1; day <= 25; day++)
					GeneratePuzzleCodeTemplate(codePath, year, day)
						.Wait();
			}
			catch (Exception ex)
			{
				Log.Line(ex.ToString());
			}
			finally
			{
				Log.Indent -= 2;
			}
		}

		private const string _puzzleFilenameTemplate = "DD_PUZZLETITLE.cs";

		private async static Task GeneratePuzzleCodeTemplate(string codePath, int year, int day)
		{
			if (DateTime.Now.Year < year || (DateTime.Now.Year == year && (DateTime.Now.Month != 12 || DateTime.Now.Day < day)))
				return;

			var path = Path.Combine(codePath, year.ToString());
			if (!Directory.Exists(path))
				Directory.CreateDirectory(path);

			if (!Directory.EnumerateFiles(path, $"{day:d2}_*.cs").Any())
			{
				var puzzleTitle = $"ToDo";

				var puzzleUrl = $"{AoCHttpClient.BaseUrl}/{year}/day/{day}";
				Log.Text($"{puzzleUrl}",
					textColor: ConsoleColor.Yellow);

				var response = await AoCHttpClient.GetAsync(puzzleUrl);
				if (response.IsSuccessStatusCode)
				{
					var puzzleBody = await response.Content.ReadAsStringAsync();

					var header = puzzleBody.GetTag("<h2>", "</h2>");
					if (header != null)
					{
						var split = header.Split(' ').Skip(3).SkipLast(1).ToArray()
							.ToPascalCase();

						puzzleTitle = string.Join(string.Empty, split)
							.StripTags(("&", ";"))
							.LettersOrDigitsOnly();

						if (char.IsNumber(puzzleTitle[0]))
							puzzleTitle = "_" + puzzleTitle;
					}

					Log.Line($" [{puzzleBody.Length}B]", indent: 0, textColor: ConsoleColor.White);
				}

				var puzzleFilePath = Path.Combine(path,
					_puzzleFilenameTemplate
						.Replace("DD", day.ToString("d2"))
						.Replace("PUZZLETITLE", puzzleTitle));
				var template = File.ReadAllText(Path.Combine(codePath, "Common", "Template", _puzzleFilenameTemplate))
					.Replace("YYYY", year.ToString())
					.Replace("DD", day.ToString("d2"))
					.Replace("PUZZLETITLE", puzzleTitle);

				File.WriteAllText(puzzleFilePath, template);
				Log.Line($"{puzzleFilePath}");
			}
		}
	}
}
