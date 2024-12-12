using Ujeby.AoC.Common;
using Ujeby.Grid.CharMapExtensions;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2024_12;

[AoCPuzzle(Year = 2024, Day = 12, Answer1 = "1450422", Answer2 = "906606", Skip = false)]
public class GardenGroups : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var map = input.Select(x => x.ToCharArray()).ToArray();

		var mapArea = new aab2i(v2i.Zero, new v2i(map.Length - 1));

		var plots = new List<Plot>();
		foreach (var p in mapArea.EnumPoints())
		{
			if (!char.IsLetter(map[p.Y][p.X]))
				continue;

			// garden plot
			map = CreateGardenPlot(map, p, mapArea, out Plot plot);
			plots.Add(plot);
		}

		// part1
		var answer1 = plots.Sum(x => x.Plants.Length * x.Perimeter);

		// part2
		map = input.Select(x => x.ToCharArray()).ToArray();

		var plotSides = plots.ToDictionary(x => x, x => CountOutterSides(x.Plants));

		// add inner sides to plots that can contain inner plots
		foreach (var plot in plots.Where(x => x.Plants.Length >= 8))
		{
			var plotArea = aab2i.FromPointArray(plot.Plants);
			var plotMap = map.TakeSub(plotArea);

			// fill everything outside of plot
			foreach (var b in plotArea.EnumBorderPoints().Where(x => map[x.Y][x.X] != plot.PlantName))
				plotMap.FloodFillNonRec(b - plotArea.Min, _outside, v2i.PlusMinusOne, plot.PlantName);

			// check if plot area contains any inner plots
			Plot innerPlot = null;
			var usedInnerPlots = new HashSet<Plot>();
			foreach (var p in plotArea.EnumPoints().Where(x => map[x.Y][x.X] != plot.PlantName && plotMap[x.Y - plotArea.Min.Y][x.X - plotArea.Min.X] != _outside))
			{
				if (innerPlot == null || !innerPlot.Plants.Contains(p))
					innerPlot = plots.Single(x => x.Plants.Contains(p));

				if (!usedInnerPlots.Add(innerPlot))
					continue;

				// add inner plot sides to main plot
				plotSides[plot] += plotSides[innerPlot];
			}
		}

		var answer2 = plotSides.Sum(x => x.Key.Plants.Length * x.Value);

		return (answer1.ToString(), answer2.ToString());
	}

	const char _outside = 'x';

	record class Plot(char PlantName, v2i[] Plants, int Perimeter);

	static char[][] CreateGardenPlot(char[][] map, v2i plotStart, aab2i mapSize, out Plot plot)
	{
		plot = null;
		var plant = map[plotStart.Y][plotStart.X];

		var result = new HashSet<v2i>
		{
			plotStart
		};

		var queue = new Queue<v2i>();
		queue.Enqueue(plotStart);

		int Perimeter(v2i p) => v2i.UpDownLeftRight.Count(x => !mapSize.Contains(p + x) || map[p.Y + x.Y][p.X + x.X] != plant);

		var perimeter = Perimeter(plotStart);
		while (queue.Count != 0)
		{
			var p = queue.Dequeue();
			foreach (var dir in v2i.UpDownLeftRight)
			{
				var p2 = p + dir;
				if (!mapSize.Contains(p2) || map[p2.Y][p2.X] != plant || !result.Add(p2))
					continue;

				queue.Enqueue(p2);
				perimeter += Perimeter(p2);
			}
		}

		foreach (var p in result)
			map[p.Y][p.X] = '.';

		plot = new Plot(plant, [.. result], perimeter);
		return map;
	}

	/// <summary>
	/// count outter sides of point region by traversing over outside edge
	/// </summary>
	/// <param name="plants"></param>
	/// <returns></returns>
	static long CountOutterSides(v2i[] plants)
	{
		if (plants.Length == 1 || plants.Length == 2)
			return 4;

		var sides = 0;
		var dirs = v2i.RightDownLeftUp.Reverse().ToArray();

		var p0 = plants[0];
		var edge0 = Array.IndexOf(dirs, dirs.First(x => !plants.Contains(p0 + x)));

		var p = p0;
		var edge = edge0;
		while (true)
		{
			var dir = dirs[(edge + 1) % dirs.Length];

			if (plants.Contains(p + dir))
			{
				var nextLeft = p + dir + dirs[edge];
				if (plants.Contains(nextLeft))
				{
					p = nextLeft;
					edge = edge = (edge - 1 + dirs.Length) % dirs.Length;
					sides++;
				}
				else
					p += dir;
			}
			else
			{
				edge = (edge + 1) % dirs.Length;
				sides++;
			}

			if (p == p0 && edge == edge0)
				break;
		}

		return sides;
	}
}