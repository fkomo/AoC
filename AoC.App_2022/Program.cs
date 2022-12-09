using Ujeby.AoC.Common;

namespace Ujeby.AoC.App
{
	class Program
	{
		static void Main(string[] args)
		{
			new AdventOfCode("https://adventofcode.com/2022")
				.Run(new ISolvable[]
				{
					new Day01.CalorieCounting()         { Answer = new string[] { "66306", "195292" }},
					new Day02.RockPaperScissors()       { Answer = new string[] { "13484", "13433" }},
					new Day03.RucksackReorganization()  { Answer = new string[] { "8394", "2413" }},
					new Day04.CampCleanup()             { Answer = new string[] { "651", "956" }},
					new Day05.SupplyStacks()			{ Answer = new string[] { "TLFGBZHCN", "QRQFHFWCL" }},
					new Day06.TuningTrouble()			{ Answer = new string[] { "1723", "3708" }},
					new Day07.NoSpaceLeftOnDevice()		{ Answer = new string[] { "1086293", "366028" }},
					new Day08.TreetopTreeHouse()		{ Answer = new string[] { "1782", "474606" }},
					new Day09.RopeBridge()				{ Answer = new string[] { null, null }},

					// TODO rest of 2022
				});
		}
	}
}
