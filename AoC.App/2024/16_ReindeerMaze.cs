using System.IO;
using Ujeby.AoC.Common;
using Ujeby.Grid.CharMapExtensions;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2024_16;

[AoCPuzzle(Year = 2024, Day = 16, Answer1 = null, Answer2 = null, Skip = true)]
public class ReindeerMaze : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var map = input.Select(x => x.ToArray()).ToArray();
		map.Find('S', out v2i start);

		// part1
		var answer1 = FindBestPathNoRec(map);

		// part2
		string answer2 = null;

		return (answer1.ToString(), answer2?.ToString());
	}

	record struct PathOption(v2i Dir, v2i[] Path, long PathScore = 0);

	static long FindBestPathNoRec(char[][] map)
	{
		map.Find('S', out v2i start);

		var queue = new Queue<PathOption>();
		queue.Enqueue(new PathOption(v2i.Right, [start]));

		var minScore = long.MaxValue;
		while (queue.Count > 0)
		{
			var opt = queue.Dequeue();

			if (opt.PathScore >= minScore)
				continue;

			var p = opt.Path[^1];

			var c = map.Get(p);
			if (c == '#')
				continue;

			if (c == 'E')
			{
				minScore = System.Math.Min(opt.PathScore, minScore);
				Debug.Line($"{minScore}");
				continue;
			}

			var front = opt.Dir;
			var left = front.RotateCW();
			var right = front.RotateCCW();

			if (!opt.Path.Contains(p + front))
				queue.Enqueue(new PathOption(front, [.. opt.Path, p + front], opt.PathScore + 1));

			if (!opt.Path.Contains(p + left))
				queue.Enqueue(new PathOption(left, [.. opt.Path, p + left], opt.PathScore + 1001));

			if (!opt.Path.Contains(p + right))
				queue.Enqueue(new PathOption(right, [.. opt.Path, p + right], opt.PathScore + 1001));
		}

		return minScore;
	}
}