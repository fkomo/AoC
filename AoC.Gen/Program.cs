using Microsoft.Extensions.Configuration;
using Ujeby.AoC.Common;
using Ujeby.Extensions;

namespace Ujeby.AoC.Gen
{
	class Program
	{
		internal class Settings
		{
			public const string ConfigSection = "AoC";

			/// <summary>
			/// path to AoC.App directory
			/// </summary>
			public string Code { get; set; }

			/// <summary>
			/// path to AoC.Input directory
			/// </summary>
			public string Input { get; set; }

			/// <summary>
			/// path to .session file
			/// </summary>
			public string Session { get; set; }
		}

		static void Main(string[] args)
		{
			var config = new ConfigurationBuilder()
				.AddJsonFile($"appsettings.json")
				.Build();

			var settings = new Settings();
			config.Bind(Settings.ConfigSection, settings);

			// set aoc session cookie
			if (!string.IsNullOrEmpty(settings.Session) && File.Exists(settings.Session))
			{
				var session = File.ReadAllText(settings.Session);
				AoCHttpClient.SetSession(session);
				Log.Line($"Session cookie: {session}");
			}

			// download input files
			if (!string.IsNullOrEmpty(settings.Input))
			{
				Log.Line("Downloading missing input ...");
				for (var year = 2015; year <= DateTime.Now.Year; year++)
					DownloadMissingInput(settings.Input, year);
			}

			// generate (empty) puzzle .cs classes
			if (!string.IsNullOrEmpty(settings.Code))
			{
				Log.Line("Generating puzzle code ...");
				for (var year = 2015; year <= DateTime.Now.Year; year++)
					GeneratePuzzleCode(settings.Code, year);
			}

			Log.Line("All set!");
		}

		private static void GeneratePuzzleCode(string codePath, int year)
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
				var template = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "Template", _puzzleFilenameTemplate))
					.Replace("YYYY", year.ToString())
					.Replace("DD", day.ToString("d2"))
					.Replace("PUZZLETITLE", puzzleTitle);

				File.WriteAllText(puzzleFilePath, template);
				Log.Line($"{puzzleFilePath}");
			}
		}

		private static void DownloadMissingInput(string inputStorage, int year)
		{
			try
			{
				Log.Indent += 2;

				if (string.IsNullOrEmpty(inputStorage))
					throw new ArgumentNullException(nameof(inputStorage), $"Input storage not set!");

				for (var day = 1; day <= 25; day++)
					DownloadInput(inputStorage, year, day)
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

		private async static Task<bool> DownloadInput(string inputStorage, int year, int day)
		{
			if (DateTime.Now.Year < year || (DateTime.Now.Year == year && (DateTime.Now.Month != 12 || DateTime.Now.Day < day)))
				return false;

			var path = Path.Combine(inputStorage, year.ToString());
			if (!Directory.Exists(path))
				Directory.CreateDirectory(path);

			var inputPath = Path.Combine(path, $"{day:d2}_input.txt");
			if (!File.Exists(inputPath))
			{
				var inputUrl = $"{AoCHttpClient.BaseUrl}/{year}/day/{day}/input";

				Log.Text($"{inputUrl}",
					textColor: ConsoleColor.Yellow);

				var response = await AoCHttpClient.GetAsync(inputUrl);
				if (response.IsSuccessStatusCode)
				{
					var input = await response.Content.ReadAsStringAsync();

					Log.Line($" [{input.Length}B]", indent: 0, textColor: ConsoleColor.White);

					File.WriteAllText(inputPath, input);

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
	}
}