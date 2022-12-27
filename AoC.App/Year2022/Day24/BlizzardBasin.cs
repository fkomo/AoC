using Ujeby.AoC.Common;
using Ujeby.Vectors;

namespace Ujeby.AoC.App.Year2022.Day24
{
	public class BlizzardBasin : PuzzleBase
	{
		private static readonly Dictionary<char, v2i> _windDir = new()
		{
			{ '>', new(1, 0) },
			{ 'v', new(0, 1) },
			{ '<', new(-1, 0) },
			{ '^', new(0, -1) }
		};

		protected override (string, string) SolveProblem(string[] input)
		{
			// TODO 2022/24

			// part1
			long? answer1 = null;

			// part2
			long? answer2 = null;

			return (answer1?.ToString(), answer2?.ToString());
		}
	}
}
