using Ujeby.AoC.Common;

namespace Ujeby.AoC.App
{
	class Program
	{
		static void Main(string[] args)
		{
			AdventOfCode.DownloadMissingInput(@"c:\Users\filip\source\repos\fkomo\AoC\AoC.App\", "Year");

			// TODO 2015
			// TODO 2016
			// TODO 2017
			// TODO 2018
			// TODO 2019
			// TODO 2020

			new AdventOfCode("https://adventofcode.com/2021")
				.Run(new ISolvable[]
				{
					new Year2021.Day01.SonarSweep()					{ Answer = new string[] { "1226", "1252" } },
					new Year2021.Day02.Dive()						{ Answer = new string[] { "1868935", "1965970888" } },
					new Year2021.Day03.BinaryDiagnostic()			{ Answer = new string[] { "2250414", "6085575" } },
					new Year2021.Day04.GiantSquid()					{ Answer = new string[] { "10680", "31892" } },
					new Year2021.Day05.HydrothermalVenture()		{ Answer = new string[] { "7085", "20271" } },
					new Year2021.Day06.Lanternfish()				{ Answer = new string[] { "352151", "1601616884019" } },
					new Year2021.Day07.TheTreacheryOfWhales()		{ Answer = new string[] { "355150", "98368490" } },
					new Year2021.Day08.SevenSegmentSearch()			{ Answer = new string[] { "247", null } },
					new Year2021.Day09.SmokeBasin()					{ Answer = new string[] { "444", "1168440" } },
					new Year2021.Day10.SyntaxScoring()				{ Answer = new string[] { "369105", "3999363569" } },
					new Year2021.Day11.DumboOctopus()				{ Answer = new string[] { "1741", "440" } },
					new Year2021.Day12.PassagePathing()				{ Answer = new string[] { "4792", "133360" }},
					new Year2021.Day13.TransparentOrigami()			{ Answer = new string[] { "751", "PGHRKLKL" }},
					new Year2021.Day14.ExtendedPolymerization()		{ Answer = new string[] { "2657", "2911561572630" }},
					new Year2021.Day15.Chitron()					{ Answer = new string[] { "811", null }},
					new Year2021.Day16.PacketDecoder()				{ Answer = new string[] { "945", "10637009915279" }},

					// TODO 2021
				});

			new AdventOfCode("https://adventofcode.com/2022")
				.Run(new ISolvable[]
				{
					new Year2022.Day01.CalorieCounting()			{ Answer = new string[] { "66306", "195292" }},
					new Year2022.Day02.RockPaperScissors()			{ Answer = new string[] { "13484", "13433" }},
					new Year2022.Day03.RucksackReorganization()		{ Answer = new string[] { "8394", "2413" }},
					new Year2022.Day04.CampCleanup()				{ Answer = new string[] { "651", "956" }},
					new Year2022.Day05.SupplyStacks()				{ Answer = new string[] { "TLFGBZHCN", "QRQFHFWCL" }},
					new Year2022.Day06.TuningTrouble()				{ Answer = new string[] { "1723", "3708" }},
					new Year2022.Day07.NoSpaceLeftOnDevice()		{ Answer = new string[] { "1086293", "366028" }},
					new Year2022.Day08.TreetopTreeHouse()			{ Answer = new string[] { "1782", "474606" }},
					new Year2022.Day09.RopeBridge()					{ Answer = new string[] { "5619", "2376" }},
					new Year2022.Day10.CathodeRayTube()             { Answer = new string[] { "14540", "EHZFZHCZ" }},

					// TODO 2022
				});
		}
	}
}
