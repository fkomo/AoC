using Ujeby.AoC.Common;

namespace Ujeby.AoC.App.Day09
{
    internal class SmokeBasin : ProblemBase
	{
		protected override (long, long) SolveProblem(string[] input)
		{
			DebugLine($"map size { input.Length}x{input[0].Length}");

			// part1
			long result1 = 0;
			for (var y = 1; y < input.Length - 1; y++)
			{
				// middle
				for (var x = 1; x < input[0].Length - 1; x++)
				{
					if ((input[y][x] < input[y - 1][x]) &&
						(input[y][x] < input[y + 1][x]) &&
						(input[y][x] < input[y][x - 1]) &&
						(input[y][x] < input[y][x + 1]))
						result1 += input[y][x] - '0' + 1;
                }
            }

            for (var x = 1; x < input[0].Length - 1; x++)
            {
                // top edge
                var t = input[0][x];
                if ((t < input[1][x]) &&
                    (t < input[0][x - 1]) &&
                    (t < input[0][x + 1]))
                    result1 += t - '0' + 1;

                // bottom edge
                var b = input[input.Length - 1][x];
                if ((b < input[input.Length - 2][x]) &&
                    (b < input[input.Length - 1][x - 1]) &&
                    (b < input[input.Length - 1][x + 1]))
                    result1 += b - '0' + 1;
            }

            for (var y = 1; y < input.Length - 1; y++)
            {
                // left edge
                var l = input[y][0];
                if ((l < input[y - 1][0]) &&
                    (l < input[y][1]) &&
                    (l < input[y + 1][0]))
                    result1 += l - '0' + 1;

                // right edge
                var r = input[y][input[y].Length - 1];
                if ((r < input[y - 1][input[y].Length - 1]) &&
                    (r < input[y][input[y].Length - 2]) &&
                    (r < input[y + 1][input[y].Length - 1]))
                    result1 += r - '0' + 1;
            }

            // top left
            if (input[0][0] < input[0][1] && input[0][0] < input[1][0])
                result1 += input[0][0] - '0' + 1;

            // bottom left
            if (input[input.Length - 1][0] < input[input.Length - 2][0] && 
                input[input.Length - 1][0] < input[input.Length - 1][1])
                result1 += input[0][0] - '0' + 1;

            // top right
            if (input[0][input[0].Length - 1] < input[0][input[0].Length - 2] &&
                input[0][input[0].Length - 1] < input[1][input[0].Length - 1])
                result1 += input[0][input[0].Length - 1] - '0' + 1;

            // bottom right
            if (input[input.Length - 1][input.Last().Length - 1] < input[input.Length - 1][input.Last().Length - 2] &&
                input[input.Length - 1][input.Last().Length - 1] < input[input.Length - 2][input.Last().Length - 1])
                result1 += input[input.Length - 1][input.Last().Length - 1] - '0' + 1;

            // part2
            long result2 = 0;

			return (result1, result2);
		}
	}
}
