using Ujeby.AoC.Common;

namespace Ujeby.AoC.App
{
	class Program
	{
		const string _aocUrl = "https://adventofcode.com";

		static HttpClient httpClient = new ();

		static void Main(string[] args)
		{
			var outputDir = @"c:\Users\filip\source\repos\fkomo\AoC\AoC.App\";
			var yearDirPrefix = "Year";

			try
			{
				Log.ChristmasHeader("AoC input downloader",
					length: 100);

				var session = ""; // aoc session cookie value
				if (string.IsNullOrEmpty(session))
				{
					var sessionFilename = Path.Combine(Environment.CurrentDirectory, ".session");
					session = File.ReadAllText(sessionFilename);
				}

				if (string.IsNullOrEmpty(session))
				{
					Console.WriteLine("Missing AoC session");
					return;
				}

				httpClient.DefaultRequestHeaders.Add("Cookie", $"session={session};");

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

				Log.ChristmasHeader($"{ downloaded }/{ total } downloaded",
					length: 100);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}
		}

		async static Task<bool> DownloadInput(int year, int day,
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

				Log.Text($"{inputUrl}", indent: 4, textColor: ConsoleColor.Yellow);
				var input = await httpClient.GetStringAsync(inputUrl);
				downloaded = true;

				Log.Line($" [{input.Length}B]", indent: 0, textColor: ConsoleColor.Yellow);

				File.WriteAllText(path, input.Substring(0, input.Length - 1));
			}

			Log.Line(path, indent: 4);
			return downloaded;
		}
	}
}
