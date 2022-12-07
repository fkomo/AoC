﻿namespace Ujeby.AoC.Common
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
			Debug.ChristmasHeader(_title, 
				length: 105);

			var stars = 0;
			try
			{
				foreach (var problem in problemsToSolve)
					stars += problem.Solve();
			}
			catch (Exception ex)
			{
				Debug.Line(ex.ToString());
			}
			finally
			{
				Debug.ChristmasHeader($"{stars}/{problemsToSolve.Length * 2} stars",
					textColor: ConsoleColor.Yellow,
					length: 105);
			}
		}
	}
}
