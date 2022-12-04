using Ujeby.AoC.Common;

namespace Ujeby.AoC.App
{
	class Program
	{
		static void Main(string[] args)
		{
			new AdventOfCode("https://adventofcode.com")
				.Run(new ISolvable[]
				{
					new Day00.Sample() { Solution = new long?[] { null, null } },
				});
		}
	}
}
