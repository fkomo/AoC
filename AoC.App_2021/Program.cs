using Ujeby.AoC.Common;

namespace Ujeby.AoC.App
{
	class Program
	{
		static void Main(string[] args)
		{
			AdventOfCode.Run("https://adventofcode.com/2021", new ISolvable[]
			{
				new Day01.SonarSweep() { SolutionPart1 = 1226, SolutionPart2 = 1252 },
				new Day02.Dive() { SolutionPart1 = 1868935, SolutionPart2 = 1965970888 },
				new Day03.BinaryDiagnostic() { SolutionPart1 = 2250414, SolutionPart2 = 6085575 },
				new Day04.GiantSquid() { SolutionPart1 = null, SolutionPart2 = null },

				// TODO 2021
			});
		}
	}
}
