using Ujeby.AoC.Common;

namespace Ujeby.AoC.App.Year2020.Day02
{
	public class PasswordPhilosophy : PuzzleBase
	{
		protected override (string, string) SolvePuzzle(string[] input)
		{
			var i = input.Select(line =>
			{
				var split = line.Split(' ');
				var interval = split[0].Split('-').Select(n => int.Parse(n)).ToArray();
				var policy = split[1][0];
				var password = split[2];

				return (interval, policy, password);
			}).ToArray();

			// part1
			long? answer1 = 0;
			foreach (var (interval, policy, password) in i)
			{
				var count = password.Count(c => c == policy);
				if (interval[0] <= count && count <= interval[1])
					answer1++;
			}

			// part2
			long? answer2 = 0;
			foreach (var (interval, policy, password) in i)
			{
				var p1 = password[interval[0] - 1] == policy;
				var p2 = password[interval[1] - 1] == policy;
				if (p1 && !p2 || !p1 && p2)
					answer2++;
			}

			return (answer1?.ToString(), answer2?.ToString());
		}
	}
}
