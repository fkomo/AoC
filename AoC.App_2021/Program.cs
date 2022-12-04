using Ujeby.AoC.Common;

namespace Ujeby.AoC.App
{
	class Program
	{
		static void Main(string[] args)
		{
			new AdventOfCode("https://adventofcode.com/2021")
				.Run(new ISolvable[]
				{
					new Day01.SonarSweep()				{ SolutionPart1 = 1226,		SolutionPart2 = 1252 },
					new Day02.Dive()					{ SolutionPart1 = 1868935,	SolutionPart2 = 1965970888 },
					new Day03.BinaryDiagnostic()		{ SolutionPart1 = 2250414,	SolutionPart2 = 6085575 },
					new Day04.GiantSquid()				{ SolutionPart1 = 10680,	SolutionPart2 = 31892 },
					new Day05.HydrothermalVenture()		{ SolutionPart1 = 7085,		SolutionPart2 = 20271 },
					new Day06.Lanternfish()				{ SolutionPart1 = 352151,	SolutionPart2 = 1601616884019 },
					new Day07.TheTreacheryOfWhales()	{ SolutionPart1 = 355150,	SolutionPart2 = 98368490 },
					new Day08.SevenSegmentSearch()		{ SolutionPart1 = 247,		SolutionPart2 = null },
					new Day09.SmokeBasin()				{ SolutionPart1 = 444,		SolutionPart2 = 1168440 },

					// TODO 2021
				});
		}
	}
}
