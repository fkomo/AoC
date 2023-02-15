using System.Reflection;

namespace Ujeby.AoC.Common
{
	public class AdventOfCode
	{
		public const int ConsoleWidth = 99;

		private const string _aocUrl = "https://adventofcode.com";

		private readonly static HttpClient _httpClient = new();

		public static void RunAll(
			string session = null, string inputStorage = null)
		{
			foreach (var aocYear in AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes())
				.Where(t => Attribute.IsDefined(t, typeof(AoCPuzzleAttribute)))
				.OrderBy(p => p.GetCustomAttribute<AoCPuzzleAttribute>().Day)
				.GroupBy(p => p.GetCustomAttribute<AoCPuzzleAttribute>().Year))
			{
#if !_2020
				if (aocYear.Key == 2020)
					continue;
#endif
#if !_2021
				if (aocYear.Key == 2021)
					continue;
#endif
#if !_2022
				if (aocYear.Key == 2022)
					continue;
#endif

				Run(aocYear.Key,
					session,
					inputStorage ?? Environment.CurrentDirectory,
					aocYear.Select(p => (IPuzzle)Activator.CreateInstance(p)).ToArray());
			}
		}

		public static void Run(int year, string session, string inputStorage,
			params IPuzzle[] problemsToSolve)
		{
			var sessionFilename = Path.Combine(Environment.CurrentDirectory, ".session");
			if (File.Exists(sessionFilename))
				session = File.ReadAllText(sessionFilename);

			if (!string.IsNullOrEmpty(session) && !string.IsNullOrEmpty(inputStorage))
			{
				_httpClient.DefaultRequestHeaders.Add("Cookie", $"session={session};");
				DownloadMissingInput(inputStorage, year);
			}

			Log.Line();
			Log.ChristmasHeader($"{_aocUrl}/{year}",
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

		private static void DownloadMissingInput(string inputStorage, int year)
		{
			try
			{
				Log.Indent += 2;

				for (var day = 1; day <= 25; day++)
					DownloadInput(year, day, inputStorage)
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

		private async static Task<bool> DownloadInput(int year, int day, string inputStorage)
		{
			if (DateTime.Now.Year < year || (DateTime.Now.Year == year && (DateTime.Now.Month != 12 || DateTime.Now.Day < day)))
				return false;

			var path = Path.Combine(inputStorage, year.ToString());
			if (!Directory.Exists(path))
				Directory.CreateDirectory(path);

			var inputPath = Path.Combine(path, $"{day:d2}_input.txt");
			if (!File.Exists(inputPath))
			{
				var inputUrl = $"{_aocUrl}/{year}/day/{day}/input";

				Log.Text($"{inputUrl}",
					textColor: ConsoleColor.Yellow);

				var response = await _httpClient.GetAsync(inputUrl);
				if (response.IsSuccessStatusCode)
				{
					var input = await response.Content.ReadAsStringAsync();

					Log.Line($" [{input.Length}B]", indent: 0, textColor: ConsoleColor.Yellow);

					// NOTE trim last character from input
					File.WriteAllText(inputPath, input[..^1]);

					var sampleInputPath = Path.Combine(path, $"{day:d2}_input.sample.txt");
					if (!File.Exists(sampleInputPath))
						File.WriteAllText(sampleInputPath, null);
				}
				else
				{
					Log.Line($" {response.StatusCode}", indent: 0, textColor: ConsoleColor.Red);
					return false;
				}
			}

			return true;
		}

		private static void UpdateCode(string codeDir, string yearPrefix, int year)
		{
			try
			{
				Log.Indent += 2;

				var programCsFilename = Path.Combine(codeDir, "Program.cs");
				if (!File.ReadAllText(programCsFilename).Contains($"// TODO {year}"))
					return;

				for (var day = 1; day <= 25; day++)
					UpdateCodeTemplate(codeDir, yearPrefix, year, day);
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

		private static void UpdateCodeTemplate(string codeDir, string yearPrefix, int year, int day)
		{
			var path = Path.Combine(codeDir, yearPrefix + year.ToString(), $"Day{day:d2}");
			if (!Directory.Exists(path))
				Directory.CreateDirectory(path);

			if (!Directory.EnumerateFiles(path, "*.cs").Any())
			{
				var puzzleTitle = $"Puzzle{year}{day:d2}";

				var puzzleCs = Path.Combine(path, $"{puzzleTitle}.cs");
				var template = File.ReadAllText(Path.Combine(codeDir, "Common", "PuzzleTemplate.cs"))
					.Replace("YYYY", year.ToString())
					.Replace("DD", day.ToString("d2"))
					.Replace("PUZZLETITLE", puzzleTitle);

				File.WriteAllText(puzzleCs, template);
				Log.Line($"{puzzleCs}");

				var programCsFilename = Path.Combine(codeDir, "Program.cs");
				var programCs = File.ReadAllText(programCsFilename);
				var todo = $"// TODO {year}";
				if (programCs.Contains(todo))
				{
					programCs = programCs.Replace(todo,
						$"new Year{year}.Day{day:d2}.{puzzleTitle}()\t\t\t\t{{ Answer = new string[] {{ null, null }} }}," + Environment.NewLine +
						$"\t\t\t\t\t{todo}");

					File.WriteAllText(programCsFilename, programCs);
					Log.Line($"{programCsFilename}");
				}
			}
		}

		public async static Task<bool> SendAnswer(int year, int day, int part, string answer)
		{
			var url = $"{_aocUrl}/{year}/day/{day}/answer";

			var content = new FormUrlEncodedContent(
				new Dictionary<string, string>
				{
					{ "answer", answer },
					{ "level", part.ToString() }
				});

			var response = await _httpClient.PostAsync(url, content);
			if (response.IsSuccessStatusCode)
			{

			}

			// TODO parse answer response

			return false;
		}
	}
}
