using Ujeby.AoC.Common;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2023_03;

[AoCPuzzle(Year = 2023, Day = 03, Answer1 = "525911", Answer2 = "75805607", Skip = false)]
public class GearRatios : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		// part1
		var possibleGears = new List<(int Number, v2i SymbolPosition)>();

		var answer1 = 0;
		for (var i = 0; i < input.Length; i++)
		{
			for (var lastDigit = input[i].Length - 1; lastDigit >= 0; lastDigit--)
			{
				if (input[i][lastDigit] == '.')
					continue;

				if (char.IsDigit(input[i][lastDigit]))
				{
					var n = 0;
					var order = 1;
					var firstDigit = lastDigit;
					for (; firstDigit >= 0 && char.IsDigit(input[i][firstDigit]); firstDigit--, order *= 10)
						n += (input[i][firstDigit] - '0') * order;
					firstDigit++;

					Debug.Line($"line {i}[{firstDigit}..{lastDigit}]: {n}");

					var adj = AdjecentSymbol(input, i, firstDigit, lastDigit, out char? symbol, out v2i symbolPosition);
					if (adj)
					{
						answer1 += n;
						if (symbol == '*')
							possibleGears.Add((n, symbolPosition));
					}

					lastDigit = firstDigit;
				}
			}
		}

		// part2
		var answer2 = possibleGears
			.GroupBy(x => x.SymbolPosition)
			.Where(x => x.Count() == 2)
			.Select(x => x.First().Number * x.Last().Number)
			.Sum();

		return (answer1.ToString(), answer2.ToString());
	}

	static bool IsSymbol(char c) => !char.IsDigit(c) && c != '.';

	static bool AdjecentSymbol(string[] input, int lineIdx, int first, int last, 
		out char? symbol, out v2i symbolPosition)
	{
		symbol = null;
		symbolPosition = v2i.Zero;

		if (first > 0 && IsSymbol(input[lineIdx][first - 1]))
		{
			symbol = input[lineIdx][first - 1];
			symbolPosition = new v2i(first - 1, lineIdx);
			return true;
		}

		if (last < input[lineIdx].Length - 1 && IsSymbol(input[lineIdx][last + 1]))
		{
			symbol = input[lineIdx][last + 1];
			symbolPosition = new v2i(last + 1, lineIdx);
			return true;
		}

		var start = first - (first > 0 ? 1 : 0);
		var end = last + (last < input[0].Length - 1 ? 1 : 0);
		if (lineIdx > 0)
		{
			for (var i = start; i <= end; i++)
				if (IsSymbol(input[lineIdx - 1][i]))
				{
					symbol = input[lineIdx - 1][i];
					symbolPosition = new v2i(i, lineIdx - 1);
					return true;
				}
		}

		if (lineIdx < input.Length - 1)
		{
			for (var i = start; i <= end; i++)
				if (IsSymbol(input[lineIdx + 1][i]))
				{
					symbol = input[lineIdx + 1][i];
					symbolPosition = new v2i(i, lineIdx + 1);
					return true;
				}
		}

		return false;
	}
}