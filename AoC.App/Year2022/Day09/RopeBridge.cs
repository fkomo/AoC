using Ujeby.AoC.Common;
using Ujeby.Vectors;

namespace Ujeby.AoC.App.Year2022.Day09
{
	public class RopeBridge : PuzzleBase
	{
		protected override (string, string) SolvePuzzle(string[] input)
		{
			// part1
			long? answer1 = RopePhysics(input, 2);

			// part2
			long? answer2 = RopePhysics(input, 10);

			return (answer1?.ToString(), answer2?.ToString());
		}

		private static Dictionary<char, v2i> _directions = new()
		{
			{ 'R', v2i.Left },
			{ 'L', v2i.Right },
			{ 'D', v2i.Down},
			{ 'U', v2i.Up },
		};

		private static long RopePhysics(string[] input, int ropeLength)
		{
			var rope = new v2i[ropeLength];
			
			HashSet<v2i> visited = new()
			{
				rope.Last()
			};

			foreach (var mov in input)
			{
				var dir = _directions[mov[0]];
				var steps = int.Parse(mov[2..]);

				for (var step = 0; step < steps; step++)
				{
					rope = SimulateRope(rope, dir);
					visited.Add(rope.Last());
				}
			}

			return visited.Count;
		}

		public static v2i[] SimulateRope(v2i[] rope, v2i d)
		{
			rope[0] += d;

			for (var p = 1; p < rope.Length; p++)
			{
				d = rope[p - 1] - rope[p];
				var dAbs = d.Abs();

				if (dAbs.X <= 1 && dAbs.Y <= 1)
					continue;

				if (d.X == 0)
					rope[p].Y += d.Y / dAbs.Y;

				else if (d.Y == 0)
					rope[p].X += d.X / dAbs.X;

				else
					rope[p] += d / dAbs;
			}

			return rope;
		}
	}
}
