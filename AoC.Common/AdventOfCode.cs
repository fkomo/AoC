namespace Ujeby.AoC.Common
{
	public class AdventOfCode
	{
		private string _title;

		public AdventOfCode(string title)
		{
			_title = title;
		}

		public void Run(ISolvable[] problemsToSolve)
		{
			Log.ChristmasHeader(_title, 
				length: 115);

			var stars = 0;
			try
			{
				foreach (var problem in problemsToSolve)
					stars += problem.Solve();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}
			finally
			{
				Log.ChristmasHeader($"{stars}/{problemsToSolve.Length * 2} stars",
					textColor: ConsoleColor.Yellow,
					length: 115);
			}
		}
	}
}
