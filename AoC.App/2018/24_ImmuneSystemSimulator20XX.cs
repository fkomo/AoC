using Ujeby.AoC.Common;
using Ujeby.Extensions;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2018_24;

[AoCPuzzle(Year = 2018, Day = 24, Answer1 = "21070", Answer2 = null, Skip = false)]
public class ImmuneSystemSimulator20XX : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var split = input.Split(string.Empty);
		var groups = split[0].Skip(1).Select(x => new Group(x, ArmyEnum.ImmuneSystem))
			.Concat(split[1].Skip(1).Select(x => new Group(x, ArmyEnum.Infection)))
			.ToArray();

		// part1
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
		}

		PrintGroups(groups);

		var answer1 = groups.Sum(x => x.Units[_cnt]);

		// part2
		string answer2 = null;

		return (answer1.ToString(), answer2?.ToString());
	}

	static void PrintGroups(Group[] groups)
	{
#if DEBUG
		foreach (var grp in groups)
			Debug.Line(grp.ToString());
#endif
	}

	class Group
	{
		public ArmyEnum Army;
		public v4i Units;
		public string DmgType;
		public string[] ImmuneTo;
		public string[] WeakTo;

		public bool NoUnits => Units[_cnt] == 0;

		public Group(string line, ArmyEnum army)
		{
			var bracket = line.Contains('(') ? line.Split(['(', ')'], StringSplitOptions.RemoveEmptyEntries)[1].Split("; ") : null;

			ImmuneTo = bracket?.SingleOrDefault(x => x.StartsWith("immune to "))
				?.Replace("immune to ", string.Empty).Split(' ').Select(x => x.Trim(',')).ToArray() ?? [];

			WeakTo = bracket?.SingleOrDefault(x => x.StartsWith("weak to "))
				?.Replace("weak to ", string.Empty).Split(' ').Select(x => x.Trim(',')).ToArray() ?? [];

			Army = army;
			Units = new v4i(line.ToNumArray());
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
		ImmuneSystem,
		Infection
	}
}