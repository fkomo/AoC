using Ujeby.AoC.Common;
using Ujeby.Grid.CharMapExtensions;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2017_21;

[AoCPuzzle(Year = 2017, Day = 21, Answer1 = "142", Answer2 = "1879071", Skip = true)]
public class FractalArt : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var startPattern = ".#./..#/###".ToPattern();
		DebugPrint(startPattern);

		var rules = input.ToDictionary(
			x => x.Split(" => ")[1].ToPattern(),
			x => FlipAndRotatePattern(x.Split(" => ")[0].ToPattern()));

		// part1
		var patternAfter5 = IteratePattern(startPattern, rules, 5);
		var answer1 = patternAfter5.Sum(x => x.Count(xx => xx == '#'));

		// part2
		var patternAfter18 = IteratePattern(patternAfter5, rules, 18 - 5);
		var answer2 = patternAfter18.Sum(x => x.Count(xx => xx == '#'));

		return (answer1.ToString(), answer2.ToString());
	}

	static char[][] IteratePattern(char[][] pattern, Dictionary<char[][], char[][][]> rules, int iterations)
	{
		for (var i = 0; i < iterations; i++)
		{
			var size = pattern.Length;
			char[][] newPattern;

			int subCnt;
			int newSize;
			v2i subSize;
			v2i newSubSize;
			if (size % 2 == 0)
			{
				subCnt = size / 2;
				newSize = subCnt * 3;
				subSize = new v2i(2);
				newSubSize = new v2i(3);
			}
			else // size % 3 == 0
			{
				subCnt = size / 3;
				newSize = subCnt * 4;
				subSize = new v2i(3);
				newSubSize = new v2i(4);
			}

			newPattern = CreateEmptyPattern(newSize);

			var start = v2i.Zero;
			for (start.Y = 0; start.Y < subCnt; start.Y++)
				for (start.X = 0; start.X < subCnt; start.X++)
				{
					var sub = pattern.TakeSub(start * subSize, subSize);

					var p = rules.Single(x => x.Value.Any(xx => xx.EqualTo(sub)));
					newPattern.InsertSubAt(start * newSubSize, p.Key);
				}

			pattern = newPattern;
			DebugPrint(pattern);
		}

		return pattern;
	}

	static void DebugPrint(char[][] map)
	{
#if DEBUG
		foreach (var line in map)
		Debug.Line(new string(line));
		Debug.Line();
#endif
	}

	static char[][] CreateEmptyPattern(int size)
	{
		var map = new char[size][];
		for (var s = 0; s < size; s++)
			map[s] = new char[size];

		return map;
	}

	static char[][][] FlipAndRotatePattern(char[][] pattern)
	{
		var patterns = new List<char[][]>() { pattern };

		void AddIfNotPresent(char[][] newPattern)
		{
			foreach (var p in patterns)
				if (newPattern.EqualTo(p))
					return;

			patterns.Add(newPattern);
		}

		pattern = pattern.RotateCW();
		AddIfNotPresent(pattern);
		AddIfNotPresent(pattern.FlipH());
		AddIfNotPresent(pattern.FlipV());

		pattern = pattern.RotateCW();
		AddIfNotPresent(pattern);
		AddIfNotPresent(pattern.FlipH());
		AddIfNotPresent(pattern.FlipV());

		pattern = pattern.RotateCW();
		AddIfNotPresent(pattern);
		AddIfNotPresent(pattern.FlipH());
		AddIfNotPresent(pattern.FlipV());

		return [.. patterns];
	}
}

static class Extensions
{
	public static char[][] ToPattern(this string input) => input.Split('/').Select(x => x.ToCharArray()).ToArray();
}