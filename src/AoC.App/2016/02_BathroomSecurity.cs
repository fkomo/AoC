using Ujeby.AoC.Common;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2016_02
{
	[AoCPuzzle(Year = 2016, Day = 02, Answer1 = "69642", Answer2 = "8CB23")]
	public class BathroomSecurity : PuzzleBase
	{
		protected override (string Part1, string Part2) SolvePuzzle(string[] input)
		{
			// part1
			var answer1 = GetBathroomCode(input, new v2i(1, 1),
				new char[,]
				{
					{ '1', '2', '3' },
					{ '4', '5', '6' },
					{ '7', '8', '9' }
				});

			// part2
			var answer2 = GetBathroomCode(input, new v2i(0, 2),
				new char[,]
				{
					{ 'x', 'x', '1', 'x', 'x' },
					{ 'x', '2', '3', '4', 'x' },
					{ '5', '6', '7', '8', '9' },
					{ 'x', 'A', 'B', 'C', 'x' },
					{ 'x', 'x', 'D', 'x', 'x' },
				});

			return (answer1, answer2);
		}

		public static string GetBathroomCode(string[] instructions, v2i start, char[,] keypad)
		{
			string code = null;
			var pos = start;
			foreach (var line in instructions)
			{
				foreach (var i in line)
				{
					switch (i)
					{
						case 'U':
							if (pos.Y > 0 && keypad[pos.Y - 1, pos.X] != 'x')
								pos += v2i.Down;
							break;
						case 'D':
							if (pos.Y < keypad.GetLength(0) - 1 && keypad[pos.Y + 1, pos.X] != 'x')
								pos += v2i.Up;
							break;
						case 'L':
							if (pos.X > 0 && keypad[pos.Y, pos.X - 1] != 'x')
								pos += v2i.Left;
							break;
						case 'R':
							if (pos.X < keypad.GetLength(0) - 1 && keypad[pos.Y, pos.X + 1] != 'x')
								pos += v2i.Right;
							break;
					}
				}

				code += keypad[pos.Y, pos.X];
			}

			return code;
		}
	}
}
