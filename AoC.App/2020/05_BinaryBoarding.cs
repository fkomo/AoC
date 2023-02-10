using Ujeby.AoC.Common;

namespace Ujeby.AoC.App._2020_05
{
	public class BinaryBoarding : PuzzleBase
	{
		protected override (string, string) SolvePuzzle(string[] input)
		{
			// part1
			var seatIds = new List<int>();
			foreach (var bp in input)
			{
				var rowStart = 0;
				var rowEnd = 127;

				var columnStart = 0;
				var columnEnd = 7;

				var i = 0;
				for (; i < bp.Length - 3; i++)
				{
					var mid = (rowEnd + rowStart) /  2;
					if (bp[i] == 'F')
						rowEnd = mid;
					else if (bp[i] == 'B')
						rowStart = mid + 1;
				}

				for (; i < bp.Length; i++)
				{
					var mid = (columnEnd + columnStart) / 2;
					if (bp[i] == 'L')
						columnEnd = mid;
					else if (bp[i] == 'R')
						columnStart = mid + 1;
				}

				seatIds.Add(8 * rowStart + columnStart);
			}
			long? answer1 = seatIds.Max();

			// part2
			long? answer2 = null;
			var orderedSeats = seatIds.OrderBy(sid => sid).ToArray();
			for (var i = 1; i < orderedSeats.Length; i++)
				if (orderedSeats[i] - orderedSeats[i-1] == 2)
				{
					answer2 = orderedSeats[i - 1] + 1;
					break;
				}

			return (answer1?.ToString(), answer2?.ToString());
		}
	}
}
