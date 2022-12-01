using Ujeby.AoC.Common;

namespace Ujeby.AoC.App
{
	class Program
	{
		static void Main(string[] args)
		{
			AdventOfCode.Run("https://adventofcode.com", new ISolvable[]
			{
				new Day00.Sample() { SolutionPart1 = null, SolutionPart2 = null },
			});
		}
	}
}
