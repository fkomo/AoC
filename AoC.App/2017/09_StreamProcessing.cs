using Ujeby.AoC.Common;

namespace Ujeby.AoC.App._2017_09;

[AoCPuzzle(Year = 2017, Day = 09, Answer1 = "14212", Answer2 = "6569", Skip = false)]
public class StreamProcessing : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		ParseStream(input[0], 1, out _, out int score, out int garbageCount, 1);

		// part1
		var answer1 = score;

		// part2
		var answer2 = garbageCount;

		return (answer1.ToString(), answer2.ToString());
	}

	static void ParseStream(string stream, int start, out int end, out int score, out int garbageCount, int recursion)
	{
		garbageCount = 0;

		end = start;
		score = recursion;

		var garbage = false;
		for (; end < stream.Length; end++)
		{
			if (garbage)
			{
				if (stream[end] == '>')
					garbage = false;

				else if (stream[end] == '!')
					end++;
				else
					garbageCount++;
			}
			else if (stream[end] == '{')
			{
				ParseStream(stream, end + 1, out end, out int recScore, out int recGarbageCount, recursion + 1);

				garbageCount += recGarbageCount;
				score += recScore;
			}

			else if (stream[end] == '}')
				return;

			else if (stream[end] == '<')
				garbage = true;
		}
	}
}