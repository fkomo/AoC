using Ujeby.AoC.Common;

namespace Ujeby.AoC.App.Year2022.Day09
{
	public class RopeBridge : ProblemBase
	{
		protected override (string, string) SolveProblem(string[] input)
		{
			// part1
			long? answer1 = RopePhysics(input, 2);

			// part2
			long? answer2 = RopePhysics(input, 10);

			return (answer1?.ToString(), answer2?.ToString());
		}

		private long RopePhysics(string[] input, int ropeLength)
		{
			var rope = new (int x, int y)[ropeLength];
			
			HashSet<(int x, int y)> visited = new()
			{
				rope.Last()
			};

			foreach (var mov in input)
			{
				var dir = Directions.NSWE[mov.Replace('U', 'N').Replace('D', 'S').Replace('L', 'W').Replace('R', 'E')[0]];

				var steps = int.Parse(mov[2..]);
				for (var step = 0; step < steps; step++)
				{
					rope = SimulateRope(rope, dir[0], dir[1]);
					visited.Add(rope.Last());
				}
			}

			return visited.Count;
		}

		public static (int x, int y)[] SimulateRope((int x, int y)[] rope, int dx, int dy)
		{
			rope[0].x += dx;
			rope[0].y += dy;

			for (var p = 1; p < rope.Length; p++)
			{
				dx = rope[p - 1].x - rope[p].x;
				dy = rope[p - 1].y - rope[p].y;

				if (Math.Abs(dx) <= 1 && Math.Abs(dy) <= 1)
					continue;

				if (dx == 0)
					rope[p].y += dy / Math.Abs(dy);
				else if (dy == 0)
					rope[p].x += dx / Math.Abs(dx);
				else
				{
					rope[p].x += dx / Math.Abs(dx);
					rope[p].y += dy / Math.Abs(dy);
				}
			}

			return rope;
		}
	}
}
