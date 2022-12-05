namespace Ujeby.AoC.Common
{
	public class AdventOfCode
	{
		private string _aocUrl;
		private string _session;

		public AdventOfCode(string url, 
			string session = null)
		{
			_aocUrl = url;
			_session = session;
		}

		public void Run(ISolvable[] problemsToSolve)
		{
			Debug.ChristmasHeader(_aocUrl, 
				length: 101);
			Debug.Line();

			var stars = 0;
			try
			{
				if (_session == null)
				{
					var sessionFilename = Path.Combine(Environment.CurrentDirectory, ".session");
					if (File.Exists(sessionFilename))
						_session = File.ReadAllText(sessionFilename);
				}

				foreach (var problem in problemsToSolve)
					stars += problem.Solve(
						inputUrl: $"{_aocUrl}/day/{problem.Day}/input",
						session: _session);

				Debug.Line();
			}
			catch (Exception ex)
			{
				Debug.Line(ex.ToString());
			}
			finally
			{
				Debug.ChristmasHeader($"{stars}/{problemsToSolve.Length * 2} stars",
					textColor: ConsoleColor.Yellow,
					length: 101);
				Debug.Line();
			}
		}
	}
}
