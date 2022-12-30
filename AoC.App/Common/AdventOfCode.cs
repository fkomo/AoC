namespace Ujeby.AoC.Common
{
	public class AdventOfCode
	{
		public string Session { get; set; }
		
		private string _title;

		private const string _aocUrl = "https://adventofcode.com";

		private static HttpClient _httpClient = new();

		public AdventOfCode(int year)
		{
			_title = $"{_aocUrl}/{year}";

			if (string.IsNullOrEmpty(Session))
			{
				var sessionFilename = Path.Combine(Environment.CurrentDirectory, ".session");
				if (File.Exists(sessionFilename))
					Session = File.ReadAllText(sessionFilename);
			}

			if (!string.IsNullOrEmpty(Session))
			{
				_httpClient.DefaultRequestHeaders.Add("Cookie", $"session={Session};");

				DownloadMissingInput(@"c:\Users\filip\source\repos\fkomo\AoC\AoC.App\", "Year", year);
			}
		}

		public void Run(IPuzzle[] problemsToSolve)
		{
			Log.Line();
			Log.ChristmasHeader(_title,
				length: 100);
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
					length: 100);
			}
		}

		private void DownloadMissingInput(string outputDir, string yearDirPrefix, int year)
		{
			try
			{
				Log.Indent += 2;

				var total = 0;

				var programCsFilename = Path.Combine(outputDir, "Program.cs");
				if (!File.ReadAllText(programCsFilename).Contains($"// TODO {year}"))
					return;

				for (var day = 1; day <= 25; day++)
				{
					var result = DownloadInput(year, day,
						yearPrefix: yearDirPrefix,
						rootDir: outputDir);
					result.Wait();

					if (result.Result)
						UpdateCodeTemplate(year, day, outputDir, yearDirPrefix);

					total++;
				}

				Log.Indent -= 2;
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

			var path = Path.Combine(rootDir ?? Environment.CurrentDirectory, yearPrefix + year.ToString(), $"Day{day:d2}");
			if (!Directory.Exists(path))
				Directory.CreateDirectory(path);

			var inputPath = Path.Combine(path, "input.txt");
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

					File.WriteAllText(inputPath, input[..^1]);
					File.WriteAllText(Path.Combine(path, "input.sample.txt"), null);
				}
				else
				{
					Log.Line($" {response.StatusCode}", indent: 0, textColor: ConsoleColor.Red);
					return false;
				}
			}

			return true;
		}

		private static void UpdateCodeTemplate(int year, int day, string rootDir, string yearPrefix)
		{
			var path = Path.Combine(rootDir ?? Environment.CurrentDirectory, yearPrefix + year.ToString(), $"Day{day:d2}");
			if (!Directory.Exists(path))
				Directory.CreateDirectory(path);

			if (!Directory.EnumerateFiles(path, "*.cs").Any())
			{
				var puzzleTitle = $"Puzzle{year}{day:d2}";

				var puzzleCs = Path.Combine(path, $"{puzzleTitle}.cs");
				var template = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "Common", "PuzzleTemplate.cs"))
					.Replace("YYYY", year.ToString())
					.Replace("DD", day.ToString("d2"))
					.Replace("PUZZLETITLE", puzzleTitle);

				File.WriteAllText(puzzleCs, template);
				Log.Line($"{puzzleCs}");

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
