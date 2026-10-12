namespace Ujeby.AoC.Common
{
	public class InputProvider
	{
		public static string[] Read(string inputStorage, int year, int day, 
			string suffix = null)
		{
			var input = Array.Empty<string>();
			try
			{
				input = File.ReadLines(GetInputFile(inputStorage, year, day, suffix))
					.ToArray();
			}
			catch (FileNotFoundException ex)
			{
				Log.Line(ex.Message, textColor: ConsoleColor.Gray);
				return input;
			}

			if (input.Any() && string.IsNullOrWhiteSpace(input.Last()))
				return input.SkipLast(1).ToArray();

			return input;
		}

		private static string GetInputFile(string inputStorage, int year, int day, string suffix)
		{
			var inputFile = $"input{suffix}.txt";
			return Path.Combine(inputStorage, year.ToString(), $"{day:d2}_{inputFile}");
		}
	}
}
