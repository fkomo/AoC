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
							downloaded++;

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

			path = Path.Combine(path, "input.txt");
			if (!File.Exists(path))
			{
				var inputUrl = $"{_aocUrl}/{year}/day/{day}/input";

				Log.Text($"Downloading ");
				Log.Text($"{inputUrl}",
					textColor: ConsoleColor.Yellow, indent: 0);
				var input = await _httpClient.GetStringAsync(inputUrl);
				downloaded = true;

				Log.Line($" [{input.Length}B]", indent: 0, textColor: ConsoleColor.Yellow);

				File.WriteAllText(path, input[..^1]);
			}

			return downloaded;
		}
	}
}
