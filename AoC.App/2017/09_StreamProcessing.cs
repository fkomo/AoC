using Ujeby.AoC.Common;

namespace Ujeby.AoC.App._2017_09;

[AoCPuzzle(Year = 2017, Day = 09, Answer1 = "14212", Answer2 = "6569", Skip = false)]
public class StreamProcessing : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		ParseStream(input[0].AsSpan(), out int score, out int garbageCount);

		// part1
		var answer1 = score;

		// part2
		var answer2 = garbageCount;

		return (answer1.ToString(), answer2.ToString());
	}

	static ReadOnlySpan<char> ParseStream(ReadOnlySpan<char> span, out int score, out int garbageCount, int recursion = 0)
	{
		garbageCount = 0;
		score = recursion;

		var garbage = false;
		while (span.Length > 0)
		{
			for (var s = 0; s < span.Length; s++)
			{
				if (garbage)
				{
					if (span[s] == '>')
						garbage = false;

					else if (span[s] == '!')
						s++;
					else
						garbageCount++;
				}
				else if (span[s] == '{')
				{
					span = ParseStream(span[(s + 1)..], out int recScore, out int recGarbageCount, recursion + 1);
					garbageCount += recGarbageCount;
					score += recScore;
					break;
				}
				else if (span[s] == '}')
					return span[(s + 1)..];

				else if (span[s] == '<')
					garbage = true;
			}
		}

		return [];
	}
}