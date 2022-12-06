using Ujeby.AoC.Common;

namespace Ujeby.AoC.App.Day13
{
	internal class TransparentOrigami : ProblemBase
	{
		internal struct Point
		{
			public int X, Y;
		}

		protected override (string, string) SolveProblem(string[] input)
		{
			var points = new List<Point>();
			var i = 0;
			for (; input[i].Length != 0; i++)
			{
				var split = input[i].Split(',');
				points.Add(new Point { X = int.Parse(split[0]), Y = int.Parse(split[1]) });
			}
			var size = new Point { X = points.Max(p => p.X), Y = points.Max(p => p.Y) };

			// part1
			long? answer1 = 0;
			var instr = input[i + 1];
			var fold = int.Parse(instr["fold along x=".Length..]);
			if (instr["fold along ".Length] == 'x')
			{
				answer1 = points.Count(p =>
				{
					if (p.X < fold)
						return true;

					return !points.Any(lp => lp.X == fold - (p.X - fold) && lp.Y == p.Y);
				});
			}
			else //if (instr["fold along ".Length] == 'y')
			{
				answer1 = points.Count(p =>
				{
					if (p.Y < fold)
						return true;

					return !points.Any(lp => lp.X == p.X && lp.Y == fold - (p.Y - fold));
				});
			}

			// part2
			// TODO 2021/13 part2
			long? answer2 = null;

			return (answer1?.ToString(), answer2?.ToString());
		}
	}
}
