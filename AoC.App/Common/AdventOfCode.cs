using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Ujeby.AoC.Common
{
	public class AdventOfCode
	{
		public const int ConsoleWidth = 99;

		private const string _aocUrl = "https://adventofcode.com";
		private const string _puzzleFilenameTemplate = "DD_PUZZLETITLE.cs";

		private readonly static HttpClient _httpClient = new();

		public static void RunAll(
			string session = null, string inputStorage = null, string code = null)
		{
			var sessionFilename = Path.Combine(Environment.CurrentDirectory, ".session");
			if (File.Exists(sessionFilename))
				session = File.ReadAllText(sessionFilename);

			foreach (var aocYear in AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes())
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

				if (!string.IsNullOrEmpty(session) && !string.IsNullOrEmpty(inputStorage))
				{
					_httpClient.DefaultRequestHeaders.Add("Cookie", $"session={session};");
					DownloadMissingInput(inputStorage, aocYear.Key);
				}

#if _DEBUG
				if (!string.IsNullOrEmpty(code))
					GeneratePuzzleCode(code, aocYear.Key);
#endif
				Run(aocYear.Key,
					inputStorage ?? Environment.CurrentDirectory,
					aocYear.Select(p => (IPuzzle)Activator.CreateInstance(p)).ToArray());
			}
		}

		public static void Run(int year, string inputStorage, params IPuzzle[] problemsToSolve)
		{
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

		public static void DownloadMissingInput(string inputStorage, int year)
		{
			try
			{
				Log.Indent += 2;

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

		public static void GeneratePuzzleCode(string codePath, int year)
		{
			try
			{
				Log.Indent += 2;

				for (var day = 1; day <= 25; day++)
					GeneratePuzzleCodeTemplate(codePath, year, day);
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
				var inputUrl = $"{_aocUrl}/{year}/day/{day}/input";

				Log.Text($"{inputUrl}",
					textColor: ConsoleColor.Yellow);

				var response = await _httpClient.GetAsync(inputUrl);
				if (response.IsSuccessStatusCode)
				{
					var input = await response.Content.ReadAsStringAsync();

					Log.Line($" [{input.Length}B]", indent: 0, textColor: ConsoleColor.Yellow);

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

		private static void GeneratePuzzleCodeTemplate(string codePath, int year, int day)
		{
			var path = Path.Combine(codePath, year.ToString());
			if (!Directory.Exists(path))
				Directory.CreateDirectory(path);

			if (!Directory.EnumerateFiles(path, $"{day:d2}_*.cs").Any())
			{
				var puzzleTitle = $"ToDo";

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

		private async static Task<bool> SendAnswer(int year, int day, int part, string answer)
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
