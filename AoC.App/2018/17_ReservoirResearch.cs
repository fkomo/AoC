using Ujeby.AoC.Common;
using Ujeby.Extensions;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2018_17;

[AoCPuzzle(Year = 2018, Day = 17, Answer1 = "34541", Answer2 = "28000", Skip = false)]
public class ReservoirResearch : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		// part1
		var map = CreateMap(input, new v2i(500, 0));
		var area = map.ToAAB2i();

		var springs = new Queue<v2i>();

		map.Find('+', out v2i spring);
		springs.Enqueue(spring);

		var minY = 0;
		for (minY = 0; minY < map.Length; minY++)
		{
			if (map[minY].Contains('#'))
				break;
		}

		var answer1 = 0;
		while (true)
		{
			ProcessSpring(map, springs, area);

			if (springs.Count == 0)
			{
				var before = answer1;
				answer1 = map.Skip(minY).Sum(x => x.Count(xx => xx == '|' || xx == '~'));
				if (answer1 == before)
					break;

				springs.Enqueue(spring);
			}
		}

		// part2
		var answer2 = map.Skip(minY).Sum(x => x.Count(xx => xx == '~'));

		return (answer1.ToString(), answer2.ToString());
	}

	public static void ProcessSpring(char[][] map, Queue<v2i> springs, aab2i bounds)
	{
		if (springs.Count == 0)
			return;

		bool IsEmptyOrFlow(v2i t) => map.Get(t) == '\0' || map.Get(t) == '|';
		bool IsSolidOrStill(v2i t) => map.Get(t) == '#' || map.Get(t) == '~';

		var spring = springs.Dequeue();

		// spring already flooded
		if (map.Get(spring) == '~')
			return;

		var flow = spring;
		flow.Y++;

		while (bounds.Contains(flow) && IsEmptyOrFlow(flow))
		{
			map.Set(flow, '|');
			flow.Y++;
		}

		// spring flows out of bounds
		if (!bounds.Contains(flow))
			return;

		var bottom = map.Get(flow);
		if (bottom == '#' || bottom == '~')
		{
			flow.Y--;

			var left = new v2i(flow.X - 1, flow.Y);
			for (; IsEmptyOrFlow(left) && bounds.Contains(left + v2i.Up) && IsSolidOrStill(left + v2i.Up); left.X--)
				map.Set(left, '|');

			var right = new v2i(flow.X + 1, flow.Y);
			for (; IsEmptyOrFlow(right) && bounds.Contains(right + v2i.Up) && IsSolidOrStill(right + v2i.Up); right.X++)
				map.Set(right, '|');

			// turn to still water
			if (map.Get(left) == '#' && map.Get(right) == '#')
			{
				for (var i = left.X + 1; i < right.X; i++)
					map[left.Y][i] = '~';

				if (!springs.Contains(spring))
					springs.Enqueue(spring);

				return;
			}

			// make new spring left
			if (IsEmptyOrFlow(left))
			{
				map.Set(left, '|');

				if (!springs.Contains(left))
					springs.Enqueue(left);
			}

			// make new spring right
			if (IsEmptyOrFlow(right))
			{
				map.Set(right, '|');

				if (!springs.Contains(right))
					springs.Enqueue(right);
			}
		}
	}

	public static char[][] CreateMap(string[] input, v2i spring)
	{
		var allTiles = ParseInput(input)
			.Prepend(spring)
			.ToArray();

		var area = aab2i.FromPoints(allTiles);

		var map = allTiles
			.Select(x => x - area.Min)
			.AsJaggedArray(x => x, x => '#', area.Size);

		spring -= area.Min;
		map.Set(spring, '+');

		return [.. map.Select(x => x.Prepend('\0').Append('\0').ToArray())];
	}

	static IEnumerable<v2i> ParseInput(string[] input) => input
		.Select(x => x.ToNumArray().Prepend(x[0] == 'x' ? 0 : 1).ToArray())
		.SelectMany(x => Enumerable.Range((int)x[2], (int)(x[3] - x[2]) + 1).Select(xx => x[0] == 0 ? new v2i(x[1], xx) : new v2i(xx, x[1])))
		.Distinct();
}