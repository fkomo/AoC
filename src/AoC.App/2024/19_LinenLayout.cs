using Ujeby.AoC.Common;

namespace Ujeby.AoC.App._2024_19;

[AoCPuzzle(Year = 2024, Day = 19, Answer1 = "236", Answer2 = "643685981770598", Skip = false)]
public class LinenLayout : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var designs = input.Skip(2).ToArray();
		Debug.Line($"{designs.Length} designs");

		var patterns = input[0].Split(", ")
			.Where(x => designs.Any(d => d.Contains(x)))
			.ToArray();
		Debug.Line($"{patterns.Length} usable patterns");

		// part1
		var validDesigns = designs.Where(x => IsValidDesignRec(x, patterns)).ToArray();
		var answer1 = validDesigns.Length;

		// part2
		var validPatterns = patterns
			.Where(x => validDesigns.Any(d => d.Contains(x)))
			.ToArray();
		var answer2 = validDesigns.Sum(x => CountAllArrangementsRec(x, validPatterns));

		return (answer1.ToString(), answer2.ToString());
	}

	public static bool IsValidDesignRec(string design, string[] patterns)
	{
		if (design.Length == 0)
			return true;

		patterns = patterns.Where(design.Contains).ToArray();
		return patterns.Where(design.StartsWith).Any(x => IsValidDesignRec(design[x.Length..], patterns));
	}

	static readonly Dictionary<string, long> _cntCache = [];

	public static long CountAllArrangementsRec(string design, string[] patterns)
	{
		if (_cntCache.TryGetValue(design, out var result))
			return result;

		patterns = patterns.Where(design.Contains).ToArray();

		result += patterns
			.Where(design.StartsWith)
			.Select(x => x.Length)
			.Sum(x => design.Length == x ? 1 : CountAllArrangementsRec(design[x..], patterns));

		_cntCache.TryAdd(design, result);

		return result;
	}
}