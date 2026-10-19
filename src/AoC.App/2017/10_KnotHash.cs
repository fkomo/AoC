using Ujeby.AoC.Common;
using Ujeby.Extensions;

namespace Ujeby.AoC.App._2017_10;

[AoCPuzzle(Year = 2017, Day = 10, Answer1 = "54675", Answer2 = "a7af2706aa9a09cf5d848c1e6605dd2a", Skip = false)]
public class KnotHash : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		// part1
		var elements = SparseHash(
			input[0].ToNumArray().Select(x => (byte)x).ToArray(),
			rounds: 1);
		
		var answer1 = elements[0] * elements[1];

		// part2
		var sparseHash = SparseHash(
			input[0].Select(x => (byte)x).Concat(new byte[] { 17, 31, 73, 47, 23 }).ToArray(),
			rounds: 64);

		var answer2 = string.Concat(DenseHash(sparseHash).Select(x => x.ToString("x2")));

		return (answer1.ToString(), answer2.ToString());
	}

	public static string Compute(string input)
	{
		var sparse = SparseHash(input.Select(x => (byte)x).Concat(new byte[] { 17, 31, 73, 47, 23 }).ToArray(), rounds: 64);
		
		var dense = DenseHash(sparse);

		var hash = string.Concat(dense.Select(x => x.ToString("x2")));

		return hash;
	}

	static byte[] DenseHash(byte[] elements)
	{
		var result = new byte[16];
		for (var i = 0; i < 16; i++)
			result[i] = elements.Skip(i * 16).Take(16).Aggregate((x, y) => (byte)(x ^ y));

		return result;
	}

	static byte[] SparseHash(byte[] lengths, int rounds = 1)
	{
		var elements = Enumerable.Range(0, 256).Select(x => (byte)x).ToArray();

		var skip = 0;
		var position = 0;

		for (var round = 0; round < rounds; round++)
		{
			for (var len = 0; len < lengths.Length; len++, skip++)
			{
				for (var i = 0; i < lengths[len] / 2; i++)
				{
					var left = (position + i) % elements.Length;
					var right = (position + lengths[len] - 1 - i) % elements.Length;
					(elements[right], elements[left]) = (elements[left], elements[right]);
				}

				position += (lengths[len] + skip) % elements.Length;
			}
		}

		return elements;
	}
}