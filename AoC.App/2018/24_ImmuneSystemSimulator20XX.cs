using Ujeby.AoC.Common;
using Ujeby.Extensions;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2018_24;

[AoCPuzzle(Year = 2018, Day = 24, Answer1 = "21070", Answer2 = "7500", Skip = true)]
public class ImmuneSystemSimulator20XX : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		// part1
		Fight(CreateArmies(input), out _, out long answer1);

		// part2
		// TODO 2018/24 OPTIMIZE p2 (1s)
		var boost = 0L;
		long unitsLeft;
		while (!Fight(CreateArmies(input, immuneSystemBoost: boost++), out ArmyEnum winner, out unitsLeft) || winner != ArmyEnum.ImmuneSystem)
		{ 
		}
		
		var answer2 = unitsLeft;

		return (answer1.ToString(), answer2.ToString());
	}

	static Group[] CreateArmies(string[] input, long immuneSystemBoost = 0)
	{
		var split = input.Split(string.Empty);
		return
		[
			.. split[0].Skip(1).Select(x => new Group(x, ArmyEnum.ImmuneSystem, dmgBoost: immuneSystemBoost)),
			.. split[1].Skip(1).Select(x => new Group(x, ArmyEnum.Infection)),
		];
	}

	static bool Fight(Group[] groups, out ArmyEnum winner, out long unitsLeft)
	{
		winner = ArmyEnum.Unspecified;
		unitsLeft = -1;

		var armyUnits = groups.GroupBy(x => x.Army).ToDictionary(x => x.Key, x => x.Sum(xx => xx.Units[_cnt]));
		
		while (groups.GroupBy(x => x.Army).All(x => x.Any(xx => !xx.NoUnits))) // while both armies alive
		{
			var fights = new List<(Group att, Group def)>();

			var aliveOrdered = groups
				.Where(x => !x.NoUnits)
				.OrderByDescending(x => x.EffectivePower)
				.ThenByDescending(x => x.Units[_init])
				.ToArray();

			// target selection
			foreach (var attacker in aliveOrdered)
			{
				var target = aliveOrdered
					// enemy not targeted and not immune to attacker's dmg type
					.Where(x => x.Army != attacker.Army && !fights.Any(xx => xx.def == x) && !x.ImmuneTo.Contains(attacker.DmgType))
					.OrderByDescending(attacker.ExpectedDmg)
					.ThenByDescending(x => x.EffectivePower)
					.ThenByDescending(x => x.Units[_init])
					.FirstOrDefault();

				if (target != null)
					fights.Add((attacker, target));
			}

			// attacking
			foreach (var (attacker, defender) in fights.OrderByDescending(x => x.att.Units[_init]))
			{
				if (attacker.NoUnits || defender.NoUnits)
					continue;

				defender.Units[_cnt] = System.Math.Max(defender.Units[_cnt] - attacker.ExpectedDmg(defender) / defender.Units[_hp], 0);
			}

			// update army count + check for tie (no change in unit count)
			var noChange = true;
			foreach (var army in armyUnits)
			{
				var currentCnt = groups.Where(x => x.Army == army.Key).Sum(x => x.Units[_cnt]);
				if (currentCnt != army.Value)
					noChange = false;

				armyUnits[army.Key] = currentCnt;
			}

			if (noChange)
				return false;
		}

		winner = armyUnits.First(x => x.Value > 0).Key;
		unitsLeft = armyUnits.Sum(x => x.Value);

		return true;
	}

	class Group
	{
		public ArmyEnum Army;
		public v4i Units;
		public string DmgType;
		public string[] ImmuneTo;
		public string[] WeakTo;

		public bool NoUnits => Units[_cnt] == 0;

		public Group(string line, ArmyEnum army, long dmgBoost = 0)
		{
			var bracket = line.Contains('(') ? line.Split(['(', ')'], StringSplitOptions.RemoveEmptyEntries)[1].Split("; ") : null;

			ImmuneTo = bracket?.SingleOrDefault(x => x.StartsWith("immune to "))
				?.Replace("immune to ", string.Empty).Split(' ').Select(x => x.Trim(',')).ToArray() ?? [];

			WeakTo = bracket?.SingleOrDefault(x => x.StartsWith("weak to "))
				?.Replace("weak to ", string.Empty).Split(' ').Select(x => x.Trim(',')).ToArray() ?? [];

			Army = army;

			Units = new v4i(line.ToNumArray());
			Units[_dmg] += dmgBoost;

			DmgType = line.Split(' ').Reverse().Skip(4).First();
		}

		public long EffectivePower => Units[_cnt] * Units[_dmg];

		public override string ToString()
			=> $"({Army}{Units}{DmgType} i[{string.Join(',', ImmuneTo)}] w[{string.Join(',', WeakTo)}])";

		public long ExpectedDmg(Group def)
		{
			var dmg = EffectivePower;

			if (def.WeakTo.Contains(DmgType))
				dmg *= 2;

			else if (def.ImmuneTo.Contains(DmgType))
				dmg = 0;

			return dmg;
		}
	}

	const int _cnt = 0;
	const int _hp = 1;
	const int _dmg = 2;
	const int _init = 3;

	public enum ArmyEnum
	{
		Unspecified = 0,
		ImmuneSystem,
		Infection
	}
}