using Ujeby.AoC.Common;

namespace Ujeby.AoC.App
{
	class Program
	{
		static void Main(string[] args)
		{
			new AdventOfCode(2021)
				.Run(new IPuzzle[]
				{
					new Year2021.Day01.SonarSweep()                 { Answer = new string[] { "1226", "1252" } },
					new Year2021.Day02.Dive()                       { Answer = new string[] { "1868935", "1965970888" } },
					new Year2021.Day03.BinaryDiagnostic()           { Answer = new string[] { "2250414", "6085575" } },
					new Year2021.Day04.GiantSquid()                 { Answer = new string[] { "10680", "31892" } },
					new Year2021.Day05.HydrothermalVenture()        { Answer = new string[] { "7085", "20271" } },
					new Year2021.Day06.Lanternfish()                { Answer = new string[] { "352151", "1601616884019" } },
					new Year2021.Day07.TheTreacheryOfWhales()       { Answer = new string[] { "355150", "98368490" } },
					new Year2021.Day08.SevenSegmentSearch()         { Answer = new string[] { "247", null } },
					new Year2021.Day09.SmokeBasin()                 { Answer = new string[] { "444", "1168440" } },
					new Year2021.Day10.SyntaxScoring()              { Answer = new string[] { "369105", "3999363569" } },
					new Year2021.Day11.DumboOctopus()               { Answer = new string[] { "1741", "440" } },
					new Year2021.Day12.PassagePathing()             { Answer = new string[] { "4792", "133360" }},
					new Year2021.Day13.TransparentOrigami()         { Answer = new string[] { "751", "PGHRKLKL" }},
					new Year2021.Day14.ExtendedPolymerization()     { Answer = new string[] { "2657", "2911561572630" }},
					new Year2021.Day15.Chitron()                    { Answer = new string[] { "811", "3012" }},
					new Year2021.Day16.PacketDecoder()              { Answer = new string[] { "945", "10637009915279" }},
					new Year2021.Day17.TrickShot()					{ Answer = new string[] { "10011", "2994" } },
					new Year2021.Day18.Puzzle202118()               { Answer = new string[] { null, null } },
					new Year2021.Day19.Puzzle202119()               { Answer = new string[] { null, null } },
					new Year2021.Day20.Puzzle202120()               { Answer = new string[] { null, null } },
					new Year2021.Day21.Puzzle202121()               { Answer = new string[] { null, null } },
					new Year2021.Day22.Puzzle202122()               { Answer = new string[] { null, null } },
					new Year2021.Day23.Puzzle202123()               { Answer = new string[] { null, null } },
					new Year2021.Day24.Puzzle202124()               { Answer = new string[] { null, null } },
					new Year2021.Day25.Puzzle202125()               { Answer = new string[] { null, null } },
				});

			new AdventOfCode(2022)
				.Run(new IPuzzle[]
				{
					new Year2022.Day01.CalorieCounting()            { Answer = new string[] { "66306", "195292" }},
					new Year2022.Day02.RockPaperScissors()          { Answer = new string[] { "13484", "13433" }},
					new Year2022.Day03.RucksackReorganization()     { Answer = new string[] { "8394", "2413" }},
					new Year2022.Day04.CampCleanup()                { Answer = new string[] { "651", "956" }},
					new Year2022.Day05.SupplyStacks()               { Answer = new string[] { "TLFGBZHCN", "QRQFHFWCL" }},
					new Year2022.Day06.TuningTrouble()              { Answer = new string[] { "1723", "3708" }},
					new Year2022.Day07.NoSpaceLeftOnDevice()        { Answer = new string[] { "1086293", "366028" }},
					new Year2022.Day08.TreetopTreeHouse()           { Answer = new string[] { "1782", "474606" }},
					new Year2022.Day09.RopeBridge()                 { Answer = new string[] { "5619", "2376" }},
					new Year2022.Day10.CathodeRayTube()             { Answer = new string[] { "14540", "EHZFZHCZ" }},
					new Year2022.Day11.MonkeyInTheMiddle()          { Answer = new string[] { "62491", "17408399184" }},
					new Year2022.Day12.HillClimbingAlgorithm()      { Answer = new string[] { "497", "492" }},
					new Year2022.Day13.DistressSignal()             { Answer = new string[] { "5682", "20304" }},
					new Year2022.Day14.RegolithReservoir()          { Answer = new string[] { "774", "22499" }},
					new Year2022.Day15.BeaconExclusionZone()        { Answer = new string[] { "5166077", "13071206703981" }},
					new Year2022.Day16.ProboscideaVolcanium()       { Answer = new string[] { null, null }},
					new Year2022.Day17.PyroclasticFlow()            { Answer = new string[] { "3202", "1591977077352" }},
					new Year2022.Day18.BoilingBoulders()            { Answer = new string[] { "4288", "2494" }},
					new Year2022.Day19.NotEnoughMinerals()          { Answer = new string[] { null, null } },
					new Year2022.Day20.GrovePositioningSystem()     { Answer = new string[] { "2622", "1538773034088" } },
					new Year2022.Day21.MonkeyMath()                 { Answer = new string[] { "43699799094202", "3375719472770" } },
					new Year2022.Day22.MonkeyMap()                  { Answer = new string[] { "27492", "78291" } },
					new Year2022.Day23.UnstableDiffusion()          { Answer = new string[] { "3920", "889" } },
					new Year2022.Day24.BlizzardBasin()              { Answer = new string[] { "343", "960" } },
					new Year2022.Day25.FullOfHotAir()               { Answer = new string[] { "121=2=1==0=10=2-20=2", null } },
				});
		}
	}
}
