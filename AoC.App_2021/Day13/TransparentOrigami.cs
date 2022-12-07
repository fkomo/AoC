using Ujeby.AoC.Common;

namespace Ujeby.AoC.App.Day13
{
	internal class TransparentOrigami : ProblemBase
	{
		protected override (string, string) SolveProblem(string[] input)
		{
			var points = new List<int[]>();
			var i = 0;
			for (; input[i].Length != 0; i++)
			{
				var split = input[i].Split(',');
				points.Add(new[]{ int.Parse(split[0]), int.Parse(split[1]) });
			}

			// part1
			var instr = input[i + 1];
			var fold = int.Parse(instr["fold along x=".Length..]);
			var axis = (instr["fold along ".Length] == 'x') ? 0 : 1;
			long? answer1 = points.Count(
				p => (p[axis] < fold) || !points.Any(lp => lp[axis] == fold - (p[axis] - fold) && lp[(axis + 1) % 2] == p[(axis + 1) % 2]));

			// part2
			var size = new[] { points.Max(p => p[0]) + 1, points.Max(p => p[1]) + 1 };
			var paper = new bool[size[1], size[0]];
			foreach (var p in points)
				paper[p[1], p[0]] = true;

			// TODO 2021/13 part2
			long? answer2 = null;

			return (answer1?.ToString(), answer2?.ToString());
		}
	}
}
