using Ujeby.AoC.Common;

namespace Ujeby.AoC.App
{
	class Program
	{
		static void Main(string[] args)
		{
			AdventOfCode.Run("https://adventofcode.com/2022", new ISolvable[]
			{
				new Day01.CalorieCounting()			{ SolutionPart1 = 66306,	SolutionPart2 = 195292 },
				new Day02.RockPaperScissors()		{ SolutionPart1 = 13484,	SolutionPart2 = 13433 },
				new Day03.RucksackReorganization()  { SolutionPart1 = 8394,		SolutionPart2 = null },

				// TODO 2022
			});
		}
	}
}
