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
			var header = $"  ~#[ {_aocUrl} ]#";
			header += string.Join("", Enumerable.Repeat("~", 101 - header.Length));
			Debug.Line(header);
			Debug.Line();

			var solved = 0;
			try
			{
				if (_session == null)
				{
					var sessionFilename = Path.Combine(Environment.CurrentDirectory, ".session");
					if (File.Exists(sessionFilename))
						_session = File.ReadAllText(sessionFilename);
				}

				foreach (var problem in problemsToSolve)
					solved += problem.Solve(
						inputUrl: $"{_aocUrl}/day/{problem.Day}/input",
						session: _session) 
						? 1 : 0;

				Debug.Line();
			}
			catch (Exception ex)
			{
				Debug.Line(ex.ToString());
			}
			finally
			{
				var result = $"  ~#[ Solved { solved }/{ problemsToSolve.Length } problems ]#";
				result += string.Join("", Enumerable.Repeat("~", 101 - result.Length));

				Debug.Line(result);
			}
		}
	}
}
