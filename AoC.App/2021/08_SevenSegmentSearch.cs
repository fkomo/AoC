using Ujeby.AoC.Common;

namespace Ujeby.AoC.App._2021_08
{
	[AoCPuzzle(Year = 2021, Day = 08, Answer1 = "247", Answer2 = "933305")]
	internal class SevenSegmentSearch : PuzzleBase
	{
		private static Dictionary<string, int> _digits = new()
		{
			{ "abcefg", 0 },
			{ "cf", 1 },
			{ "acdeg", 2 },
			{ "acdfg", 3 },
			{ "bcdf", 4 },
			{ "abdfg", 5 },
			{ "abdefg", 6 },
			{ "acf", 7 },
			{ "abcdefg", 8 },
			{ "abcdfg", 9 },
		};

		private static int[][] _segmentsMutations = new int[][]
		{
			new [] { 0, 0, 0, 1, 0, 1, 1 },
			new [] { 0, 1, 0, 0, 0, 1, 1 },
			new [] { 0, 1, 1, 0, 0, 0, 1 },
			new [] { 0, 1, 1, 0, 1, 0, 0 },
			new [] { 0, 1, 0, 0, 1, 1, 0 },
			new [] { 0, 0, 1, 1, 1, 0, 0 },
			new [] { 0, 0, 1, 1, 0, 0, 1 },
			new [] { 0, 0, 0, 1, 1, 1, 0 }
		};

		protected override (string, string) SolvePuzzle(string[] input)
		{
			// part1
			long? answer1 = input.Sum(line => line[line.IndexOf("|")..].Split(' ').Count(d => new[] { 2, 3, 4, 7 }.Contains(d.Length)));

			// part2
			long? answer2 = 0; 
			foreach (var line in input)
			{
				var patterns = line[..line.IndexOf(" |")].Split(' ')
					.Where(p => p.Length < 7).ToArray();

				//  0
				// 1 2
				//  3
				// 4 5
				//  6
				var seg = Enumerable.Repeat("abcdefg", 7).ToArray();

				// 2x = 1
				var _1 = patterns.SingleOrDefault(p => p.Length == 2);
				if (_1 != null)
				{
					seg[2] = new string(seg[2].Intersect(_1).ToArray());
					seg[5] = new string(seg[5].Intersect(_1).ToArray());

					seg[0] = new string(seg[0].Except(_1).ToArray());
					seg[1] = new string(seg[1].Except(_1).ToArray());
					seg[3] = new string(seg[3].Except(_1).ToArray());
					seg[4] = new string(seg[4].Except(_1).ToArray());
					seg[6] = new string(seg[6].Except(_1).ToArray());
				}

				// 3x = 7
				var _7 = patterns.SingleOrDefault(p => p.Length == 3);
				if (_7 != null)
				{
					seg[0] = new string(seg[0].Intersect(_7).ToArray());
					seg[2] = new string(seg[2].Intersect(_7).ToArray());
					seg[5] = new string(seg[5].Intersect(_7).ToArray());

					seg[1] = new string(seg[1].Except(_7).ToArray());
					seg[3] = new string(seg[3].Except(_7).ToArray());
					seg[4] = new string(seg[4].Except(_7).ToArray());
					seg[6] = new string(seg[6].Except(_7).ToArray());
				}

				// 4x = 4
				var _4 = patterns.SingleOrDefault(p => p.Length == 4);
				if (_4 != null)
				{
					seg[1] = new string(seg[1].Intersect(_4).ToArray());
					seg[2] = new string(seg[2].Intersect(_4).ToArray());
					seg[3] = new string(seg[3].Intersect(_4).ToArray());
					seg[5] = new string(seg[5].Intersect(_4).ToArray());

					seg[0] = new string(seg[0].Except(_4).ToArray());
					seg[4] = new string(seg[4].Except(_4).ToArray());
					seg[6] = new string(seg[6].Except(_4).ToArray());
				}

				answer2 += GetOutputDigitsValue(seg, patterns, line[(line.IndexOf(" | ") + " | ".Length)..].Split(" "));
			}

			return (answer1?.ToString(), answer2?.ToString());
		}

		private static int GetOutputDigitsValue(string[] seg, string[] patterns, string[] digits)
		{
			foreach (var sm in _segmentsMutations)
			{
				var s = new char[sm.Length];
				for (var i = 0; i < sm.Length; i++)
					s[i] = seg[i][sm[i]];

				var allOk = true;
				foreach (var p in patterns)
				{
					if (_digits.ContainsKey(TransformPattern(p, s)))
						continue;

					allOk = false;
					break;
				}

				if (!allOk)
					continue;

				var result = 0;
				foreach (var d in digits)
					result = 10 * result + _digits[TransformPattern(d, s)];

				return result;
			}

			return 0;
		}

		private static string TransformPattern(string pattern, char[] segMap)
		{
			var tp = new char[pattern.Length];
			for (var i = 0; i < pattern.Length; i++)
				tp[i] = (char)('a' + Array.IndexOf(segMap, pattern[i]));

			return new string(tp.OrderBy(i => i).ToArray());
		}
	}
}
