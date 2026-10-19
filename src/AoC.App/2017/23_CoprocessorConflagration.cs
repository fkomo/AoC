using Ujeby.AoC.Common;

namespace Ujeby.AoC.App._2017_23;

[AoCPuzzle(Year = 2017, Day = 23, Answer1 = "8281", Answer2 = "911", Skip = false)]
public class CoprocessorConflagration : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		// part1
		var answer1 = AssemblyInput();

		// part2
		var answer2 = AssemblyInput_Optimized();

		return (answer1.ToString(), answer2.ToString());
	}

	static long AssemblyInput()
	{
		var mulCnt = 0;
		long a, b, c, d, e, f, g, h = 0;

		a = 0;
		b = 93;
		c = b;

		if (a != 0) // jnz a 2 + jnz 1 5
		{
			b = (b * 100) + 100_000;
			c = b + 17_000;
		}

		while (true)
		{
			f = 1;
			d = 2;

			do
			{
				e = 2;
				do
				{
					mulCnt++;
					g = d * e - b;
					if (g == 0) // jnz g 2
						f = 0;
					e++;
					g = e - b;
				}
				while (g != 0); // jnz g -8
				d++;
				g = d - b;
			}
			while (g != 0); // jnz g -13

			if (f == 0) // jnz f 2
				h++;

			g = b - c;
			if (g == 0) // jnz g 2 + jnz 1 3
				break;

			b += 17;
		}

		return mulCnt;
	}

	static long AssemblyInput_Optimized()
	{
		long a, b, c, d, f, g, h = 0;

		a = 1;
		b = 93;
		c = b;

		if (a != 0) // jnz a 2 + jnz 1 5
		{
			b = (b * 100) + 100_000;
			c = b + 17_000;
		}

		while (true)
		{
			f = 1;

			// ----
			//d = 2;
			//do
			//{
			//	e = 2;
			//	do
			//	{
			//		g = d * e - b;
			//		if (g == 0) // jnz g 2
			//			f = 0;
			//		e++;
			//		g = e - b;
			//	}
			//	while (g != 0); // jnz g -8
			//	d++;
			//	g = d - b;
			//}
			//while (g != 0); // jnz g -13
			// ----
			for (d = 2; d < b; d++)
			{
				if (b % d == 0)
				{
					f = 0;
					break;
				}
			}
			// ----

			if (f == 0) // jnz f 2
				h++;

			g = b - c;
			if (g == 0) // jnz g 2 + jnz 1 3
				break;

			b += 17;
		}

		return h;
	}
}