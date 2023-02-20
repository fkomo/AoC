namespace Ujeby.AoC.Common
{
	public class InputProvider
	{
		public static void DownloadMissingInput(string inputStorage, int year)
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

		public async static Task<bool> DownloadInput(string inputStorage, int year, int day)
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

		public static string[] Read(string inputStorage, int year, int day)
		{
			var input = File.ReadLines(GetInputFile(inputStorage, year, day))
				.ToArray();

			if (string.IsNullOrWhiteSpace(input.Last()))
				return input.SkipLast(1).ToArray();

			return input;
		}

		private static string GetInputFile(string inputStorage, int year, int day)
		{
			var inputFile = "input.txt";
#if _DEBUG_SAMPLE
			inputFile = "input.sample.txt";
#endif
			return Path.Combine(inputStorage, year.ToString(), $"{day:d2}_{inputFile}");
		}
	}
}
