﻿using Ujeby.AoC.Common;

namespace Ujeby.AoC.App
{
	class Program
	{
		static void Main(string[] args)
		{
			AdventOfCode.Run("https://adventofcode.com/2022", new ISolvable[]
			{
				new Day01.CalorieCounting()			{ SolutionPart1 = 66306,	SolutionPart2 = 195292 },

				// TODO 2022
			});
		}
	}
}
