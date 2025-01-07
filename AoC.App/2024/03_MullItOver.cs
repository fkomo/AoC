using Ujeby.AoC.Common;
using Ujeby.Extensions;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2024_03;

[AoCPuzzle(Year = 2024, Day = 03, Answer1 = "164730528", Answer2 = "70478672", Skip = false)]
public class MullItOver : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var mem = input
			.SelectMany(x => x.Split("mul("))
			.ToArray();

		// part1
		var answer1 = mem.MulSum();

		// part2
		var dont = false;
		for (var i = 0; i < mem.Length; i++)
		{
			if (dont)
				mem[i] = '*' + mem[i]; // corrupt possible mul

			var dontIndex = mem[i].LastIndexOf("don't()");
			var doIndex = mem[i].LastIndexOf("do()");

			if (!dont && dontIndex > doIndex)
				dont = true;

			else if (dont && dontIndex < doIndex)
				dont = false;
		}

		var answer2 = mem.MulSum();

		return (answer1.ToString(), answer2.ToString());
	}
}

public static class Extensions
{
	public static long MulSum(this string[] mem)
		=> mem
			.Where(x => x.Contains(')') && x[0] != '*')
			.Select(x => x[..x.IndexOf(')')])
			.Where(x => x.Length <= (3 + 3 + 1) && x.Contains(','))
			.Sum(x => new v2i(x.ToNumArray()).Area());
}