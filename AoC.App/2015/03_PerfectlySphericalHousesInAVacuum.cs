using Ujeby.AoC.Common;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2015_03
{
	[AoCPuzzle(Year = 2015, Day = 03, Answer1 = "2565", Answer2 = "2639")]
	public class PerfectlySphericalHousesInAVacuum : PuzzleBase
	{
		private static readonly Dictionary<char, v2i> _dirs = new()
		{
			{ '>', v2i.Right },
			{ 'v', v2i.Down },
			{ '^', v2i.Up },
			{ '<', v2i.Left },
		};

		protected override (string Part1, string Part2) SolvePuzzle(string[] input)
		{
			var directions = input.Single();

			// part1
			var santa = new v2i();
			var houses = new List<v2i>()
			{
				santa 
			};
			foreach (var dir in directions)
			{
				santa += _dirs[dir];
				if (!houses.Contains(santa))
					houses.Add(santa);
			}
			var answer1 = houses.Count;

			// part2
			santa = new v2i();
			var robotSanta = santa;
			houses = new List<v2i>()
			{
				santa
			};
			for (var i = 0; i < directions.Length - 1; i += 2)
			{
				santa += _dirs[directions[i]];
				if (!houses.Contains(santa))
					houses.Add(santa);

				robotSanta += _dirs[directions[i + 1]];
				if (!houses.Contains(robotSanta))
					houses.Add(robotSanta);
			}
			var answer2 = houses.Count;

			return (answer1.ToString(), answer2.ToString());
		}
	}
}
