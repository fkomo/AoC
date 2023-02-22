using Ujeby.AoC.Common;

namespace Ujeby.AoC.App._2022_06
{
	[AoCPuzzle(Year = 2022, Day = 06, Answer1 = "1723", Answer2 = "3708")]
	internal class TuningTrouble : PuzzleBase
	{
		protected override (string, string) SolvePuzzle(string[] input)
		{
			// part1
			long? answer1 = FindDistinctSequence(input[0], 4);

			// part2
			long? answer2 = FindDistinctSequence(input[0], 14);

			return (answer1?.ToString(), answer2?.ToString());
		}

		private static int FindDistinctSequence(string data, int length)
		{
			var result = 1;
			for (var sequenceStart = 0; result - sequenceStart < length; result++)
				for (var c = result - 1; c >= sequenceStart; c--)
					if (data[c] == data[result])
					{
						sequenceStart = c + 1;
						break;
					}

			return result;
		}
	}
}
