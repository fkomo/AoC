using System.Data;
using Ujeby.AoC.Common;
using Ujeby.Extensions;
using Ujeby.Tools;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2018_15;

[AoCPuzzle(Year = 2018, Day = 15, Answer1 = "198744", Answer2 = "66510", Skip = true)]
public class BeverageBandits : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var map = CreateMap(input, out v2i[] elves, out v2i[] goblins);

		// part1
		var units = CreateUnitsList(elves, goblins);
		var rounds = Combat(map, units);
		var answer1 = rounds * units.Sum(x => x.Hp);

		// part2
		// TODO 2018/15 OPTIMIZE p2 (2s)

		var lifeTimeIf4GoblinsAttacking = 200 / (3 * 4);
		var maxApNeededApToKill4Goblins = (4 * 200) / lifeTimeIf4GoblinsAttacking;

		var answer2 = 0;
		for (var ap = 4; ap < maxApNeededApToKill4Goblins; ap++)
		{
			units = CreateUnitsList(elves, goblins, elvesAp: ap);
			rounds = Combat(map, units, endOnElfDeath: true);

			// all elves survived
			if (units.Count(x => x is Elf) == elves.Length)
			{
				answer2 = rounds * units.Sum(x => x.Hp);
				break;
			}
		}

		return (answer1.ToString(), answer2.ToString());
	}

	public static char[][] CreateMap(string[] input, out v2i[] elves, out v2i[] goblins)
	{
		var map = input.Select(x => x.ToArray()).ToArray();
		
		goblins = map.EnumAll('G').ToArray();
		elves = map.EnumAll('E').ToArray();

		// clean map from units
		foreach (var unitPos in elves.Concat(goblins))
			map.Set(unitPos, '.');

		return map;
	}

	public static List<Unit> CreateUnitsList(v2i[] elves, v2i[] goblins, int elvesAp = 3) =>
		elves.Select(x => new Elf { Pos = x, Ap = elvesAp }).Cast<Unit>()
			.Concat(goblins.Select(x => new Goblin { Pos = x }).Cast<Unit>())
			.ToList();

	static int Combat(char[][] map, List<Unit> units, bool endOnElfDeath = false)
	{
		var round = 0;

		var combatEnds = false;
		while (!combatEnds)
		{
			if (!Round(units, map, endOnElfDeath: endOnElfDeath))
				return round;

			round++;
		}

		return round;
	}

	public static bool Round(List<Unit> units, char[][] map, bool endOnElfDeath = false)
	{
		var unitsOrdered = units.OrderBy(x => x.Pos, new ReadingOrderComparer()).ToArray();
		foreach (var unit in unitsOrdered)
		{
			// if unit is already dead
			if (!units.Contains(unit))
				continue;

			// if all targets are dead
			var targets = units.Where(x => x.GetType() != unit.GetType()).ToArray();
			if (targets.Length == 0)
				return false;

			if (!Move(unit, targets, units, map))
				continue;

			Attack(unit, targets, units, out bool elfDied);

			if (elfDied && endOnElfDeath)
				return false;
		}

		return true;
	}

	static bool Move(Unit unit, Unit[] targets, List<Unit> units, char[][] map)
	{
		var upDownLeftRight = v2i.UpDownLeftRight.Select(x => x + unit.Pos).ToArray();
	
		// if target is already in range
		if (targets.Any(x => upDownLeftRight.Contains(x.Pos)))
			return true;

		// find new target

		// all empty squares adjecent to targets
		var inRange = targets
			.SelectMany(x => v2i.UpDownLeftRight.Select(xx => xx + x.Pos))
			.Distinct()
			.Where(x => map.Get(x) != '#' && !units.Any(xx => xx.Pos == x))
			.ToArray();

		// no target has empty adjecent tile
		if (inRange.Length == 0)
			return false;

		// create map copy with all units (except current selected unit) as walls
		var visibleMap = CloneMapWithUnits(map, units.Except([unit]));

		// no target-adjecent-tile is reachable
		var reachable = visibleMap.FloodFillNonRecWithDistance(unit.Pos, v2i.UpDownLeftRight, '#').Where(x => inRange.Contains(x.Key));
		if (!reachable.Any())
			return false;

		var chosen = reachable
			.OrderBy(x => x.Value).GroupBy(x => x.Value).First()
			.Select(x => x.Key).Order(new ReadingOrderComparer()).First();

		var step = visibleMap.FloodFillNonRecWithDistance(chosen, v2i.UpDownLeftRight, '#').Where(x => upDownLeftRight.Contains(x.Key))
			.GroupBy(x => x.Value).First()
			.OrderBy(x => x.Key, new ReadingOrderComparer()).First().Key;

		// step
		unit.Pos = step;

		return true;
	}

	static void Attack(Unit unit, Unit[] targets, List<Unit> units, out bool elfDied)
	{
		elfDied = false;

		var upDownLeftRight = v2i.UpDownLeftRight.Select(x => x + unit.Pos).ToArray();
		var targetsInRange = targets.Where(x => upDownLeftRight.Contains(x.Pos)).ToArray();
		if (targetsInRange.Length == 0)
			return;

		var selectedTarget = targetsInRange
			.OrderBy(x => x.Hp).GroupBy(x => x.Hp).First()
			.OrderBy(x => x.Pos, new ReadingOrderComparer()).First();

		selectedTarget.Hp -= unit.Ap;

		// target is dead
		if (selectedTarget.Hp <= 0)
		{
			units.Remove(selectedTarget);

			if (selectedTarget is Elf)
				elfDied = true;
		}
	}

	static char[][] CloneMapWithUnits(char[][] map, IEnumerable<Unit> units)
	{
		var mapWithUnits = map.Select(x => x.ToArray()).ToArray();
		foreach (var u in units)
			mapWithUnits.Set(u.Pos, '#');

		return mapWithUnits;
	}
}

public class Unit
{
	public v2i Pos { get; set; }
	public int Ap { get; set; } = 3;
	public int Hp { get; set; } = 200;

	public override string ToString() => $"{GetType().Name}{Pos}@{Hp}";
}

public class Elf : Unit { }

public class Goblin : Unit { }