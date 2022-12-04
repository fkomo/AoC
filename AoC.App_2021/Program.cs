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
					new Day01.SonarSweep()              { Answer = new long?[] { 1226, 1252 } },
					new Day02.Dive()                    { Answer = new long?[] { 1868935, 1965970888 } },
					new Day03.BinaryDiagnostic()        { Answer = new long?[] { 2250414, 6085575 } },
					new Day04.GiantSquid()              { Answer = new long?[] { 10680, 31892 } },
					new Day05.HydrothermalVenture()     { Answer = new long?[] { 7085, 20271 } },
					new Day06.Lanternfish()             { Answer = new long?[] { 352151, 1601616884019 } },
					new Day07.TheTreacheryOfWhales()    { Answer = new long?[] { 355150, 98368490 } },
					new Day08.SevenSegmentSearch()      { Answer = new long?[] { 247, null } },
					new Day09.SmokeBasin()              { Answer = new long?[] { 444, 1168440 } },
					new Day10.SyntaxScoring()           { Answer = new long?[] { 369105, 3999363569 } },
					new Day11.DumboOctopus()            { Answer = new long?[] { 1741, 440 }},
					new Day12.PassagePathing()          { Answer = new long?[] { null, null }},

					// TODO rest of 2021
				});
		}
	}
}
