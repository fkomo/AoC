using Ujeby.AoC.Common;

namespace Ujeby.AoC.App
{
	class Program
	{
		const string _aocUrl = "https://adventofcode.com";

		static HttpClient httpClient = new ();

		static void Main(string[] args)
		{
			var outputDir = @"c:\Users\filip\source\repos\fkomo\AoC";
			var yearDirPrefix = "AoC.App_";

			try
			{
				Debug.ChristmasHeader("AoC input downloader",
					length: 100);
				Debug.Line();

				var session = ""; // aoc session cookie value
				if (string.IsNullOrEmpty(session))
				{
					var sessionFilename = Path.Combine(Environment.CurrentDirectory, ".session");
					session = File.ReadAllText(sessionFilename);
				}

				if (string.IsNullOrEmpty(session))
				{
					Debug.Line("Missing AoC session", textColor: ConsoleColor.Red);
					return;
				}

				httpClient.DefaultRequestHeaders.Add("Cookie", $"session={session};");
			
				for (var year = 2015; year <= DateTime.Now.Year; year++)
					for (var day = 1; day <= 24; day++)
						DownloadInput(year, day,
							yearPrefix: yearDirPrefix,
							rootDir: outputDir)
							.Wait();
			}
			catch (Exception ex)
			{
				Debug.Line(ex.ToString());
			}
		}

		async static Task DownloadInput(int year, int day,
			string rootDir = null, string yearPrefix = null)
		{
			if (DateTime.Now.Year < year || (DateTime.Now.Year == year && (DateTime.Now.Month != 12 || DateTime.Now.Day < day)))
				return;

			var path = Path.Combine(rootDir ?? Environment.CurrentDirectory, yearPrefix + year.ToString(), $"Day{day:d2}");
			if (!Directory.Exists(path))
				Directory.CreateDirectory(path);

			path = Path.Combine(path, "input.txt");
			if (!File.Exists(path))
			{
				var inputUrl = $"{_aocUrl}/{year}/day/{day}/input";

				Debug.Text($"{inputUrl}", indent: 2, textColor: ConsoleColor.Yellow);
				var input = await httpClient.GetStringAsync(inputUrl);
				Debug.Line($" [{input.Length}B]", indent: 0, textColor: ConsoleColor.Yellow);

				File.WriteAllText(path, input.Substring(0, input.Length - 1));
			}

			Debug.Line(path);
		}
	}
}
