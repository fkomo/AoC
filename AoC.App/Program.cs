using Microsoft.Extensions.Configuration;
using Ujeby.AoC.Common;

namespace Ujeby.AoC.App
{
	class Program
	{
		static void Main(string[] args)
		{
			var config = new ConfigurationBuilder().AddJsonFile($"appsettings.json").Build();

			var input = config["aoc:input"];
			var session = string.Empty;
			if (File.Exists(config["aoc:session"]))
				session = File.ReadAllText(config["aoc:session"]);

#if _2022
			new AdventOfCode(2022,
				session: session,
				inputFilesBase: input)
				.Run(new IPuzzle[]
				{
					new _2022_01.CalorieCounting()			{ Answer = new string[] { "66306", "195292" }},
					new _2022_02.RockPaperScissors()		{ Answer = new string[] { "13484", "13433" }},
					new _2022_03.RucksackReorganization()	{ Answer = new string[] { "8394", "2413" }},
					new _2022_04.CampCleanup()				{ Answer = new string[] { "651", "956" }},
					new _2022_05.SupplyStacks()				{ Answer = new string[] { "TLFGBZHCN", "QRQFHFWCL" }},
					new _2022_06.TuningTrouble()			{ Answer = new string[] { "1723", "3708" }},
					new _2022_07.NoSpaceLeftOnDevice()		{ Answer = new string[] { "1086293", "366028" }},
					new _2022_08.TreetopTreeHouse()			{ Answer = new string[] { "1782", "474606" }},
					new _2022_09.RopeBridge()				{ Answer = new string[] { "5619", "2376" }},
					new _2022_10.CathodeRayTube()			{ Answer = new string[] { "14540", "EHZFZHCZ" }},
					new _2022_11.MonkeyInTheMiddle()		{ Answer = new string[] { "62491", "17408399184" }},
					new _2022_12.HillClimbingAlgorithm()	{ Answer = new string[] { "497", "492" }},
					new _2022_13.DistressSignal()			{ Answer = new string[] { "5682", "20304" }},
					new _2022_14.RegolithReservoir()		{ Answer = new string[] { "774", "22499" }},
					new _2022_15.BeaconExclusionZone()		{ Answer = new string[] { "5166077", "13071206703981" }},
					new _2022_16.ProboscideaVolcanium()		{ Answer = new string[] { "2029", "2723" }},
					new _2022_17.PyroclasticFlow()			{ Answer = new string[] { "3202", "1591977077352" }},
					new _2022_18.BoilingBoulders()			{ Answer = new string[] { "4288", "2494" }},
					new _2022_19.NotEnoughMinerals()		{ Answer = new string[] { "1466", "8250" } },
					new _2022_20.GrovePositioningSystem()	{ Answer = new string[] { "2622", "1538773034088" } },
					new _2022_21.MonkeyMath()				{ Answer = new string[] { "43699799094202", "3375719472770" } },
					new _2022_22.MonkeyMap()				{ Answer = new string[] { "27492", "78291" } },
					new _2022_23.UnstableDiffusion()		{ Answer = new string[] { "3920", "889" } },
					new _2022_24.BlizzardBasin()			{ Answer = new string[] { "343", "960" } },
					new _2022_25.FullOfHotAir()				{ Answer = new string[] { "121=2=1==0=10=2-20=2", "*" } },
				});
#endif

#if _2021
			new AdventOfCode(2021,
				session: session,
				inputFilesBase: input)
				.Run(new IPuzzle[]
				{
					new _2021_01.SonarSweep()				{ Answer = new string[] { "1226", "1252" } },
					new _2021_02.Dive()						{ Answer = new string[] { "1868935", "1965970888" } },
					new _2021_03.BinaryDiagnostic()			{ Answer = new string[] { "2250414", "6085575" } },
					new _2021_04.GiantSquid()				{ Answer = new string[] { "10680", "31892" } },
					new _2021_05.HydrothermalVenture()		{ Answer = new string[] { "7085", "20271" } },
					new _2021_06.Lanternfish()				{ Answer = new string[] { "352151", "1601616884019" } },
					new _2021_07.TheTreacheryOfWhales()		{ Answer = new string[] { "355150", "98368490" } },
					new _2021_08.SevenSegmentSearch()		{ Answer = new string[] { "247", "933305" } },
					new _2021_09.SmokeBasin()				{ Answer = new string[] { "444", "1168440" } },
					new _2021_10.SyntaxScoring()			{ Answer = new string[] { "369105", "3999363569" } },
					new _2021_11.DumboOctopus()				{ Answer = new string[] { "1741", "440" } },
					new _2021_12.PassagePathing()			{ Answer = new string[] { "4792", "133360" }},
					new _2021_13.TransparentOrigami()		{ Answer = new string[] { "751", "PGHRKLKL" }},
					new _2021_14.ExtendedPolymerization()	{ Answer = new string[] { "2657", "2911561572630" }},
					new _2021_15.Chitron()					{ Answer = new string[] { "811", "3012" }},
					new _2021_16.PacketDecoder()			{ Answer = new string[] { "945", "10637009915279" }},
					new _2021_17.TrickShot()				{ Answer = new string[] { "10011", "2994" } },
					new _2021_18.Snailfish()				{ Answer = new string[] { "4641", "4624" } },
					new _2021_19.BeaconScanner()			{ Answer = new string[] { "483", "14804" } },
					new _2021_20.TrenchMap()				{ Answer = new string[] { "5571", "17965" } },
					new _2021_21.DiracDice()				{ Answer = new string[] { "1196172", "106768284484217" } },
					new _2021_22.ReactorReboot()			{ Answer = new string[] { "591365", "1211172281877240" } },
					new _2021_23.Amphipod()					{ Answer = new string[] { "10607", "59071" } },
					new _2021_24.ArithmeticLogicUnit()		{ Answer = new string[] { "99196997985942", "84191521311611" } },
					new _2021_25.SeaCucumber()				{ Answer = new string[] { "530", "*" } },
				});
#endif

#if _2020
			new AdventOfCode(2020,
				session: session,
				inputFilesBase: input)
				.Run(new IPuzzle[]
				{
					new _2020_01.ReportRepair()				{ Answer = new string[] { "927684", "292093004" } },
					new _2020_02.PasswordPhilosophy()		{ Answer = new string[] { "474", "745" } },
					new _2020_03.TobogganTrajectory()		{ Answer = new string[] { "223", "3517401300" } },
					new _2020_04.PassportProcessing()		{ Answer = new string[] { "245", "133" } },
					new _2020_05.BinaryBoarding()			{ Answer = new string[] { "826", "678" } },
					new _2020_06.CustomCustoms()			{ Answer = new string[] { "6297", "3158" } },
					new _2020_07.HandyHaversacks()			{ Answer = new string[] { "128", "20189" } },
					new _2020_08.HandheldHalting()			{ Answer = new string[] { "2034", "672" } },
					new _2020_17.ConwayCubes()				{ Answer = new string[] { null, null } },
				});
#endif
		}
	}
}
