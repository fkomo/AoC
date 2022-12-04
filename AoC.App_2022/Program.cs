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
					new Day01.CalorieCounting()         { Answer = new long ?[] { 66306, 195292 }},
					new Day02.RockPaperScissors()       { Answer = new long ?[] { 13484, 13433 }},
					new Day03.RucksackReorganization()  { Answer = new long ?[] { 8394, 2413 }},
					new Day04.CampCleanup()             { Answer = new long ?[] { 651, 956 }},

					// TODO rest of 2022
				});
		}
	}
}
