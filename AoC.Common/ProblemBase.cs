﻿using System.Diagnostics;

namespace Ujeby.AoC.Common
{
	public abstract class ProblemBase : ISolvable
	{
		public long?[] Solution { get; set; }

		public int Day => int.Parse(GetType().Namespace.Split('.').Last().Replace("Day", null));

		public bool Solve(
			string inputUrl = null, string session = null)
		{
			var result = false;

			var title = $"--=[ { GetType().FullName.Substring("Ujeby.AoC.App.".Length) } ]=";
			title += string.Join("", Enumerable.Repeat("-", Debug.LineLength - title.Length));
			Debug.Line(title);

			var sw = new Stopwatch();
			try
			{
				DebugLine();

				sw.Start();

				var answer = SolveProblem(ReadInput(inputUrl, session));

				var elapsed = (int)sw.Elapsed.TotalMilliseconds;
				DebugLine($"Solved in ~{ elapsed }ms { (elapsed > 250 ? "[> 250ms!]" : null) }");
				DebugLine(AnswerMessage(1, answer.Item1, Solution[0]));
				DebugLine(AnswerMessage(2, answer.Item2, Solution[1]));

				result = true;

				if (!Solution[0].HasValue || Solution[0].Value != answer.Item1)
					result = false;

				if (!Solution[1].HasValue || Solution[1].Value != answer.Item2)
					result = false;
			}
			catch (Exception ex)
			{
				DebugLine(ex.ToString());
			}
			finally
			{
				sw.Stop();
			}

			DebugLine();

			return result;
		}

		private string AnswerMessage(int part, long answer, long? solution)
		{
			var answerMessage = $"Part{part} answer = { answer }";

			if (solution.HasValue && solution.Value != answer)
				answerMessage += $" [!= {solution.Value}]";
			
			else if (!solution.HasValue)
				answerMessage += $" [?]";

			return answerMessage;
		}

		protected abstract (long, long) SolveProblem(string[] input);

		protected static void DebugLine(string message = null)
		{
			Debug.Line("  " + message);
		}

		protected string _workingDir => Path.Combine(Environment.CurrentDirectory, GetType().FullName.Split('.')[3]);

#if _DEBUG_SAMPLE
		protected string _inputFilename => Path.Combine(_workingDir, "input.sample.txt");
#else
		protected string _inputFilename => Path.Combine(_workingDir, "input.txt");
#endif

		private string[] ReadInput(string inputUrl, string session)
		{
#if _DEBUG_SAMPLE
#else
			// download input
			if (!File.Exists(_inputFilename) && session != null)
			{
				var httpClient = new HttpClient();
				httpClient.DefaultRequestHeaders.Add("Cookie", $"session={session};");
				var input = httpClient.GetStringAsync(inputUrl).Result;
				DebugLine($"Input downloaded from {inputUrl}");

				if (!Directory.Exists(_workingDir))
					Directory.CreateDirectory(_workingDir);

				File.WriteAllText(_inputFilename, input);
			}
#endif
			return File.ReadLines(_inputFilename).ToArray();
		}
	}
}