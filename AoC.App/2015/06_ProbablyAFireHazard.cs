using Ujeby.AoC.Common;
using Ujeby.Tools.StringExtensions;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2015_06
{
	[AoCPuzzle(Year = 2015, Day = 06, Answer1 = "400410", Answer2 = "15343601")]
	public class ProbablyAFireHazard : PuzzleBase
	{
		public record struct Instruction(bool? Action, v4i Area);

		protected override (string Part1, string Part2) SolvePuzzle(string[] input)
		{
			//[x,y]---[z,y]
			//  |		|
			//[x,w]---[z,w]
			var instructions = input.Select(i =>
				new Instruction(i.StartsWith("turn on") ? true : i.StartsWith("turn off") ? false : null, new v4i(i.ToNumArray())))
				.ToArray();

			var gridSize = 1000;

			var lights = new bool[gridSize, gridSize];
			var lights2 = new int[gridSize, gridSize];

			// part1
			// part2
			foreach (var instr in instructions)
			{
				for (var y = instr.Area.Y; y <= instr.Area.W; y++)
					for (var x = instr.Area.X; x <= instr.Area.Z; x++)
					{
						switch (instr.Action)
						{
							case true:
								lights[y, x] = true;
								lights2[y, x]++;
								break;

							case false:
								lights[y, x] = false;
								lights2[y, x] = Math.Max(0, lights2[y, x] - 1);
								break;

							case null:
								lights[y, x] = !lights[y, x];
								lights2[y, x] += 2;
								break;
						}
					}
			}

			var answer1 = 0;
			var answer2 = 0;
			for (var y = 0; y < gridSize; y++)
				for (var x = 0; x < gridSize; x++)
				{
					answer2 += lights2[y, x];
					if (lights[y, x])
						answer1++;
				}

			return (answer1.ToString(), answer2.ToString());
		}
	}
}
