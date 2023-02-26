using Ujeby.AoC.Common;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2015_18
{
	[AoCPuzzle(Year = 2015, Day = 18, Answer1 = null, Answer2 = null)]
	public class LikeAGIFForYourYard : PuzzleBase
	{
		private static readonly v2i[] _dir = new v2i[]
		{
			v2i.Down + v2i.Left,    v2i.Down,		v2i.Down + v2i.Right,
			v2i.Left,               /*v2i.Zero,*/   v2i.Right,
			v2i.Up + v2i.Left,      v2i.Up,			v2i.Up + v2i.Right,
		};

		protected override (string Part1, string Part2) SolvePuzzle(string[] input)
		{
			string answer1 = null, answer2 = null;

			var lights = input.Select(i => i.ToCharArray()).ToArray();

			// part1
			var steps = 100;
			for (var i = 0; i < steps; i++)
			{
				var tmp = lights.Select(l => l.ToArray()).ToArray();

				var p = new v2i();
				for (p.Y = 0; p.Y < input.Length; p.Y++)
				{
					for (p.X = 0; p.X < input.Length; p.X++)
					{
						var neighbours = 0;
						foreach (var dir in _dir)
						{
							var p1 = p + dir;
							if (p1.X < 0 || p.Y < 0 || p.X == input.Length || p.Y == input.Length)
								continue;

							if (lights[p.Y][p.X] == '#')
								neighbours++;
						}

						if (lights[p.Y][p.X] == '#')
						{
							if (neighbours == 2 || neighbours == 3)
								tmp[p.Y][p.X] = '#';
							else
								tmp[p.Y][p.X] = '.';
						}
						else if (neighbours == 3)
							tmp[p.Y][p.X] = '#';
						else
							tmp[p.Y][p.X] = '.';
					}
				}

				lights = tmp;
			}

			// part2

			return (answer1, answer2);
		}
	}
}
