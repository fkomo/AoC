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
					new Day01.SonarSweep()              { Solution = new long?[] { 1226, 1252 } },
					new Day02.Dive()                    { Solution = new long?[] { 1868935, 1965970888 } },
					new Day03.BinaryDiagnostic()        { Solution = new long?[] { 2250414, 6085575 } },
					new Day04.GiantSquid()              { Solution = new long?[] { 10680, 31892 } },
					new Day05.HydrothermalVenture()     { Solution = new long?[] { 7085, 20271 } },
					new Day06.Lanternfish()             { Solution = new long?[] { 352151, 1601616884019 } },
					new Day07.TheTreacheryOfWhales()    { Solution = new long?[] { 355150, 98368490 } },
					new Day08.SevenSegmentSearch()      { Solution = new long?[] { 247, null } },
					new Day09.SmokeBasin()              { Solution = new long?[] { 444, 1168440 } },
					new Day10.SyntaxScoring()           { Solution = new long?[] { 369105, 3999363569 } },
					new Day11.DumboOctopus()            { Solution = new long?[] { 1741, 440 }},

					// TODO 2021
				});
		}
	}
}
