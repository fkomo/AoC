using Ujeby.AoC.Common;
using Ujeby.Extensions;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2018_23;

[AoCPuzzle(Year = 2018, Day = 23, Answer1 = "691", Answer2 = "126529978", Skip = false)]
public class ExperimentalEmergencyTeleportation : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var nanobots = input
			.Select(x => new v4i(x.ToNumArray()))
			.ToArray();

		// part1
		var largest = nanobots.OrderByDescending(x => x.W).First();
		var answer1 = nanobots.Count(x => v3i.ManhDistance(x.ToV3i(), largest.ToV3i()) <= largest.W);

		// part2
		var answer2 = FindCoordinateWithLargestNumOfNanobots(nanobots);

		return (answer1.ToString(), answer2.ToString());
	}

	static long FindCoordinateWithLargestNumOfNanobots(v4i[] nanobots)
	{
		var min = new v3i(
			nanobots.Select(x => x.X - x.W).Min(),
			nanobots.Select(x => x.Y - x.W).Min(),
			nanobots.Select(x => x.Z - x.W).Min());

		var max = new v3i(
			nanobots.Select(x => x.X + x.W).Max(),
			nanobots.Select(x => x.Y + x.W).Max(),
			nanobots.Select(x => x.Z + x.W).Max());

		var maxAabb = new aab3i([min, max]);

		var queue = new PriorityQueue<(aab3i, v4i[]), int>();
		queue.Enqueue((maxAabb, nanobots), nanobots.Length);

		var result = v3i.Zero;

		while (queue.Count > 0)
		{
			var (aabb, nbs) = queue.Dequeue();

			if (aabb.Size == 1)
			{
				result = aabb.Min;
				break;
			}

			foreach (var sub in aabb.OctDiv())
			{
				var subNbs = nbs.Where(x => sub.IntersectsWithOctahedron(x)).ToArray();
				if (subNbs.Length > 0)
					queue.Enqueue((sub, subNbs), -subNbs.Length);
			}
		}

		return result.ManhLength();
	}
}