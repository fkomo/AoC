using Ujeby.AoC.Common;

namespace Ujeby.AoC.App
{
	class Program
	{
		static void Main(string[] args)
		{
			AdventOfCode.Run("https://adventofcode.com/2021", new ISolvable[]
			{
				//new Day00.Sample() { Solution1 = null, Solution2 = null },
				
				new Day01.SonarSweep() { Solution1 = 1226, Solution2 = 1252 },
				new Day02.Dive() { Solution1 = 1868935, Solution2 = 1965970888 },
				new Day03.BinaryDiagnostic() { Solution1 = 2250414, Solution2 = 6085575 },
				new Day04.GiantSquid() { Solution1 = null, Solution2 = null },

				// TODO 2021
			});
		}
	}
}
