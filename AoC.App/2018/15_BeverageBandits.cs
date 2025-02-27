using System.Data;
using Ujeby.AoC.Common;
using Ujeby.Extensions;
using Ujeby.Tools;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2018_15;

[AoCPuzzle(Year = 2018, Day = 15, Answer1 = "198744", Answer2 = null, Skip = false)]
public class BeverageBandits : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var map = input.Select(x => x.ToArray()).ToArray();

		var elves = map.EnumAll('E').Select(x => new Elf { Pos = x }).ToArray();
		var goblins = map.EnumAll('G').Select(x => new Goblin { Pos = x }).ToArray();
		var units = elves.Cast<Unit>().Concat(goblins.Cast<Unit>()).ToList();

		foreach (var unitPos in units.Select(x => x.Pos))
			map.Set(unitPos, '.');

		Draw(map, units);

		var readOrdCmp = new ReadingOrderComparer();

		// part1
		var round = 0;
		var combatEnds = false;
		while (!combatEnds)
		{
			var unitsOrdered = units.OrderBy(x => x.Pos, readOrdCmp).ToArray();
			foreach (var unit in unitsOrdered)
			{
				// if unit is already dead
				if (!units.Contains(unit))
					continue;

				// if all targets are dead
				var targets = units.Where(x => x.GetType() != unit.GetType());
				if (!targets.Any())
				{
					combatEnds = true;
					break;
				}

				// move

				var upDownLeftRight = v2i.UpDownLeftRight.Select(x => x + unit.Pos).ToArray();
				var targetsInRange = targets.Where(x => upDownLeftRight.Contains(x.Pos));
				if (!targetsInRange.Any())
				{
					// find new target

					// all empty squares adjecent to targets
					var inRange = targets
						.SelectMany(x => v2i.UpDownLeftRight.Select(xx => xx + x.Pos))
						.Distinct()
						.Where(x => map.Get(x) != '#' && !units.Any(xx => xx.Pos == x))
						.ToArray();

					// no target has empty adjecent tile
					if (inRange.Length == 0)
						continue;

					// create map copy with all units (except current selected unit) as walls
					var visibleMap = CloneMapWithUnits(map, units.Except([unit]));

					var distance = visibleMap.FloodFillNonRecWithDistance(unit.Pos, v2i.UpDownLeftRight, '#');

					// no target-adjecent-tile is reachable
					var reachable = distance.Where(x => inRange.Contains(x.Key));
					if (!reachable.Any())
						continue;

					var nearest = reachable
						.OrderBy(x => x.Value).GroupBy(x => x.Value).First().Select(x => x.Key);

					var chosen = nearest.Order(readOrdCmp).First();

					var distanceFromChosen = visibleMap.FloodFillNonRecWithDistance(chosen, v2i.UpDownLeftRight, '#');

					var step = distanceFromChosen.Where(x => upDownLeftRight.Contains(x.Key))
						.GroupBy(x => x.Value).First()
						.OrderBy(x => x.Key, readOrdCmp).First().Key;

					// step
					unit.Pos = step;
					upDownLeftRight = [.. v2i.UpDownLeftRight.Select(x => x + unit.Pos)];
				}

				// attack

				if (!targetsInRange.Any())
					targetsInRange = targets.Where(x => upDownLeftRight.Contains(x.Pos));

				// no targets in range
				if (!targetsInRange.Any())
					continue;

				var selectedTarget = targetsInRange
					.OrderBy(x => x.Hp).GroupBy(x => x.Hp).First()
					.OrderBy(x => x.Pos, readOrdCmp).First();

				selectedTarget.Hp -= unit.Ap;
				Debug.Line($"#{round}: {selectedTarget} attacked by {unit}");

				// target is dead
				if (selectedTarget.Hp <= 0)
				{
					units.Remove(selectedTarget);
					Debug.Line($"#{round}: {selectedTarget} died");
				}
			}

			if (!combatEnds)
			{
				round++;

				Debug.Line();
				Debug.Line($"After {round} round:");
				Draw(map, units);
			}
		}

		var answer1 = round * units.Sum(x => x.Hp);

		// part2
		string answer2 = null;

		return (answer1.ToString(), answer2?.ToString());
	}

	static char[][] CloneMapWithUnits(char[][] map, IEnumerable<Unit> units)
	{
		var mapWithUnits = map.Select(x => x.ToArray()).ToArray();
		foreach (var u in units)
			mapWithUnits.Set(u.Pos, '#');

		return mapWithUnits;
	}

	static void Draw(char[][] map, IEnumerable<Unit> units)
	{
#if DEBUG
		for (var y = 0; y < map.Length; y++)
		{
			var sb = new StringBuilder();
			for (var x = 0; x < map[y].Length; x++)
			{
				if (units.Where(x => x is Elf).Select(x => x.Pos).Contains(new v2i(x, y)))
					sb.Append('E');

				else if (units.Where(x => x is Goblin).Select(x => x.Pos).Contains(new v2i(x, y)))
					sb.Append('G');

				else
					sb.Append(map[y][x]);
			}
			Debug.Line(sb.ToString());
		}

		Debug.Line();
#endif
	}
}

class Unit
{
	public v2i Pos { get; set; }
	public int Ap { get; set; } = 3;
	public int Hp { get; set; } = 200;

	public override string ToString() => $"{GetType().Name}{Pos}@{Hp}";
}

class Elf : Unit { }

class Goblin : Unit { }