namespace Ujeby.AoC.Common
{
	public class AdventOfCode
	{
		private string _title;

		private const string _aocUrl = "https://adventofcode.com";

		private static HttpClient _httpClient = new();

		public AdventOfCode(string title)
		{
			_title = title;
		}

		public void Run(ISolvable[] problemsToSolve)
		{
			Log.Line();
			Log.ChristmasHeader(_title,
				length: 110);
			Log.Line();

			var stars = 0;
			try
			{
				Log.Indent += 2;
				Debug.Indent += 2;

				foreach (var problem in problemsToSolve)
					stars += problem.Solve();

				Debug.Indent -= 2;
				Log.Indent -= 2;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}
			finally
			{
				Log.Line();
				Log.ChristmasHeader($"{stars}/{problemsToSolve.Length * 2} stars",
					textColor: ConsoleColor.Yellow,
					length: 110);
			}
		}

		public static void DownloadMissingInput(string outputDir, string yearDirPrefix)
		{
			try
			{
				Log.Line();
				Log.Line("Checking input files ...");

				var session = ""; // aoc session cookie value
				if (string.IsNullOrEmpty(session))
				{
					var sessionFilename = Path.Combine(Environment.CurrentDirectory, ".session");
					session = File.ReadAllText(sessionFilename);
				}

				if (string.IsNullOrEmpty(session))
				{
					Log.Line("AoC session missing!",
						textColor: ConsoleColor.Red);
					return;
				}

				_httpClient.DefaultRequestHeaders.Add("Cookie", $"session={session};");

				Log.Indent += 2;

				var total = 0;
				var downloaded = 0;
				for (var year = 2015; year <= DateTime.Now.Year; year++)
				{
					for (var day = 1; day <= 24; day++)
					{
						var result = DownloadInput(year, day,
							yearPrefix: yearDirPrefix,
							rootDir: outputDir);
						result.Wait();

						if (result.Result)
						{
							downloaded++;

							Log.Indent += 2;
							CreateCodeTemplate(year, day, outputDir, yearDirPrefix);
							Log.Indent -= 2;
						}

						total++;
					}
				}

				Log.Indent -= 2;

				Log.Line($"All input files up to date ({total})");
			}
			catch (Exception ex)
			{
				Log.Line(ex.ToString());
			}
		}

		private async static Task<bool> DownloadInput(int year, int day,
			string rootDir = null, string yearPrefix = null)
		{
			if (DateTime.Now.Year < year || (DateTime.Now.Year == year && (DateTime.Now.Month != 12 || DateTime.Now.Day < day)))
				return false;

			var downloaded = false;

			var path = Path.Combine(rootDir ?? Environment.CurrentDirectory, yearPrefix + year.ToString(), $"Day{day:d2}");
			if (!Directory.Exists(path))
				Directory.CreateDirectory(path);

			var inputPath = Path.Combine(path, "input.txt");
			if (!File.Exists(inputPath))
			{
				var inputUrl = $"{_aocUrl}/{year}/day/{day}/input";

				Log.Text($"Downloading ");
				Log.Text($"{inputUrl}",
					textColor: ConsoleColor.Yellow, indent: 0);

				var response = await _httpClient.GetAsync(inputUrl);
				if (response.IsSuccessStatusCode)
				{
					var input = await response.Content.ReadAsStringAsync();
					downloaded = true;

					Log.Line($" [{input.Length}B]", indent: 0, textColor: ConsoleColor.Yellow);

					File.WriteAllText(inputPath, input[..^1]);
					File.WriteAllText(Path.Combine(path, "input.sample.txt"), null);
				}
				else
				{
					Log.Line($" {response.StatusCode}", indent: 0, textColor: ConsoleColor.Red);
				}
			}

			return downloaded;
		}

		private static void CreateCodeTemplate(int year, int day, string rootDir, string yearPrefix)
		{
			var path = Path.Combine(rootDir ?? Environment.CurrentDirectory, yearPrefix + year.ToString(), $"Day{day:d2}");
			if (!Directory.Exists(path))
				Directory.CreateDirectory(path);

			var puzzleTitle = $"Puzzle{year}{day:d2}";
			var template = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "PuzzleTemplate.cs"))
				.Replace("YYYY", year.ToString())
				.Replace("DD", day.ToString("d2"))
				.Replace("PUZZLETITLE", puzzleTitle);

			File.WriteAllText(Path.Combine(path, $"{puzzleTitle}.cs"), template);
			Log.Line($"{Path.Combine(path, $"{puzzleTitle}.cs")}");

			var programCsFilename = Path.Combine(rootDir, "Program.cs");
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
}
