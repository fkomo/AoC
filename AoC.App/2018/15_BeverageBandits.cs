using System.Data;
using Ujeby.AoC.Common;
using Ujeby.Extensions;
using Ujeby.Tools;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2018_15;

[AoCPuzzle(Year = 2018, Day = 15, Answer1 = "198744", Answer2 = "66510", Skip = false)]
public class BeverageBandits : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var map = input.Select(x => x.ToArray()).ToArray();

		var elves = map.EnumAll('E').ToArray();
		var goblins = map.EnumAll('G').ToArray();

		foreach (var unitPos in elves.Concat(goblins))
			map.Set(unitPos, '.');

		// part1
		var units = CreateUnitsList(elves, goblins);
		var rounds = Fight(map, units);
		var answer1 = rounds * units.Sum(x => x.Hp);

		// part2
		var lifeTimeIf4GoblinsAttacking = 200 / (3 * 4);
		var maxApNeededApToKill4Goblins = (4 * 200) / lifeTimeIf4GoblinsAttacking;

		var answer2 = 0;
		for (var ap = 4; ap < maxApNeededApToKill4Goblins; ap++)
		{
			units = CreateUnitsList(elves, goblins, elvesAp: ap);
			rounds = Fight(map, units, endOnElfDeath: true);

			// all elves survived
			if (units.Count(x => x is Elf) == elves.Length)
			{
				answer2 = rounds * units.Sum(x => x.Hp);
				break;
			}
		}

		return (answer1.ToString(), answer2.ToString());
	}

	static List<Unit> CreateUnitsList(v2i[] elves, v2i[] goblins, int elvesAp = 3) =>
		elves.Select(x => new Elf { Pos = x, Ap = elvesAp }).Cast<Unit>()
			.Concat(goblins.Select(x => new Goblin { Pos = x }).Cast<Unit>())
			.ToList();

	static int Fight(char[][] map, List<Unit> units, bool endOnElfDeath = false)
	{
		var readOrdCmp = new ReadingOrderComparer();

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
					Debug.Line($"#{round}: {selectedTarget} died");
					units.Remove(selectedTarget);

					if (selectedTarget is Elf && endOnElfDeath)
						return round;
				}
			}

			if (!combatEnds)
				round++;
		}

		return round;
	}

	static char[][] CloneMapWithUnits(char[][] map, IEnumerable<Unit> units)
	{
		var mapWithUnits = map.Select(x => x.ToArray()).ToArray();
		foreach (var u in units)
			mapWithUnits.Set(u.Pos, '#');

		return mapWithUnits;
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