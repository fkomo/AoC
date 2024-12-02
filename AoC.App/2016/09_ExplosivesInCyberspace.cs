using System.Text;
using Ujeby.AoC.Common;
using Ujeby.Extensions;

namespace Ujeby.AoC.App._2016_09;

[AoCPuzzle(Year = 2016, Day = 09, Answer1 = "120765", Answer2 = "11658395076", Skip = true)]
public class ExplosivesInCyberspace : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var compressed = input.Single();

		// part1
		var decompressed = DecompressV1(compressed);
		var answer1 = decompressed.Length;

		// part2
		// TODO 2016/09 OPTIMIZE p2 (46min)
		var answer2 = DecompressLengthV2(decompressed);

		return (answer1.ToString(), answer2.ToString());
	}

	private static string DecompressV1(string compressed)
	{
		var sb = new StringBuilder();
		for (var i = 0; i < compressed.Length; i++)
		{
			if (compressed[i] == '(')
			{
				var closing = compressed.IndexOf(')', i + 1);
				var param = compressed[(i + 1)..closing].ToNumArray();

				var repeat = Enumerable.Repeat(compressed[(closing + 1)..((int)param[0] + closing + 1)], (int)param[1]);
				sb.Append(string.Join(string.Empty, repeat));
				i = closing + (int)param[0];
			}
			else
				sb.Append(compressed[i]);
		}

		return sb.ToString();
	}

	private static string TrimDecompressed(string compressed, out int trimmed)
	{
		trimmed = compressed.IndexOf('(');
		if (trimmed == -1)
		{
			trimmed = compressed.Length;
			return null;
		}

		return compressed[trimmed..];
	}

	private static long DecompressLengthV2(string compressed)
	{
		var length = 0L;

		var sb = new StringBuilder();
		while (compressed?.Length > 0)
		{
			Debug.Line($"{length}/{compressed.Length}");
			//Debug.Line(compressed + Environment.NewLine);

			if (compressed[0] != '(')
			{
				compressed = TrimDecompressed(compressed, out int trimmed);
				length += trimmed;
				continue;
			}

			var closing = compressed.IndexOf(')');
			var param = compressed[1..closing].ToNumArray();
			var seq = compressed[(closing + 1)..((int)param[0] + closing + 1)];
			if (!seq.Contains('('))
			{
				length += param[0] * param[1];
				compressed = compressed[(closing + (int)param[0] + 1)..];
				continue;
			}

			var repeat = Enumerable.Repeat(seq, (int)param[1]);
			sb.Append(string.Join(string.Empty, repeat));

			compressed = sb.ToString() + compressed[(closing + (int)param[0] + 1)..];
			sb.Clear();
		}

		return length;
	}
}