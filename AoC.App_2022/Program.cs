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
					new Day01.CalorieCounting()         {Solution = new long ?[] { 66306, 195292 }},
					new Day02.RockPaperScissors()       {Solution = new long ?[] { 13484, 13433 }},
					new Day03.RucksackReorganization()  {Solution = new long ?[] { 8394, 2413 }},

					// TODO 2022
				});
		}
	}
}
