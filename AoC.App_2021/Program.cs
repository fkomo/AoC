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
					new Day01.SonarSweep()              { Answer = new string[] { "1226", "1252" } },
					new Day02.Dive()                    { Answer = new string[] { "1868935", "1965970888" } },
					new Day03.BinaryDiagnostic()        { Answer = new string[] { "2250414", "6085575" } },
					new Day04.GiantSquid()              { Answer = new string[] { "10680", "31892" } },
					new Day05.HydrothermalVenture()     { Answer = new string[] { "7085", "20271" } },
					new Day06.Lanternfish()             { Answer = new string[] { "352151", "1601616884019" } },
					new Day07.TheTreacheryOfWhales()    { Answer = new string[] { "355150", "98368490" } },
					new Day08.SevenSegmentSearch()      { Answer = new string[] { "247", "null" } },
					new Day09.SmokeBasin()              { Answer = new string[] { "444", "1168440" } },
					new Day10.SyntaxScoring()           { Answer = new string[] { "369105", "3999363569" } },
					new Day11.DumboOctopus()            { Answer = new string[] { "1741", "440" } },
					new Day12.PassagePathing()          { Answer = new string[] { "4792", "133360" }},
					new Day13.TransparentOrigami()      { Answer = new string[] { "751", null }},

					// TODO rest of 2021
				});
		}
	}
}
