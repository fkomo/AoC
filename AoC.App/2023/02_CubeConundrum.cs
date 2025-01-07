using Ujeby.AoC.Common;
using Ujeby.Extensions;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2023_02;

[AoCPuzzle(Year = 2023, Day = 02, Answer1 = "1853", Answer2 = "72706", Skip = false)]
public class CubeConundrum : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var colors = new string[] { "red", "green", "blue" };

		var games = input.ToDictionary(
			x => x.Split(':')[0].ToNumArray()[0],
			x => {
				return x[(x.IndexOf(": ") + 2)..]
					.Split(';')
					.Select(s => s
						.Split(',')
						.Select(c => c.Trim())
						.ToDictionary(
							k => Array.IndexOf(colors, k.Split(' ')[1]), 
							v => int.Parse(v.Split(' ')[0])))
					.ToArray();
			});

		// part1
		var bag = new v3i(12, 13, 14);
		var answer1 = games
			.Where(x => x.Value.All(g => IsValidGame(g, bag)))
			.Select(x => x.Key)
			.Sum();

		// part2
		var answer2 = games
			.Select(x => Power(x.Value))
			.Sum();

		return (answer1.ToString(), answer2.ToString());
	}

	static long Power(Dictionary<int, int>[] sets)
	{
		var minSet = new v3i();

		foreach(var s in sets)
			for (var i = 0; i < 3; i++)
				if (s.ContainsKey(i))
					minSet[i] = System.Math.Max(minSet[i], s[i]);

		return minSet.Volume();
	}

	static bool IsValidGame(Dictionary<int, int> game, v3i bag)
	{
		for (var i = 0; i < 3; i++)
			if (game.ContainsKey(i) && game[i] > bag[i])
				return false;

		return true;
	}
}