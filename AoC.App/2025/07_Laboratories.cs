using Ujeby.AoC.Common;
using Ujeby.Extensions;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2025_07;

[AoCPuzzle(Year = 2025, Day = 07, Answer1 = "1678", Answer2 = null, Skip = false)]
public class Laboratories : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var map = input.Select(x => x.ToArray()).ToArray();
		var splitters = map.EnumAll('^').ToArray();
		map.Find('S', out v2i start);

		// part1
		var beams = new Queue<v2i>();
		beams.Enqueue(start);

		while (beams.Count > 0)
		{
			var beam = beams.Dequeue();

			var beamEnd = beam.Y + 1;
			for (; beamEnd < map.Length && map[beamEnd][beam.X] == '.'; beamEnd++)
				map[beamEnd][beam.X] = '|';

			if (beamEnd == map.Length || map[beamEnd][beam.X] != '^')
				continue;

			foreach (var nextBeam in 
				new v2i[]
				{ 
					new(beam.X - 1, beamEnd), 
					new(beam.X + 1, beamEnd)
				}
				.Where(x => map.Get(x) == '.' && !beams.Contains(x)))
			{
				map[nextBeam.Y][nextBeam.X] = '|';
				beams.Enqueue(nextBeam);
			}
		}

		var answer1 = splitters.Count(x => map.Get(new v2i(x.X, x.Y - 1)) == '|');

		// part2
		string answer2 = null;

		return (answer1.ToString(), answer2?.ToString());
	}
}