using Ujeby.AoC.Common;
using Ujeby.Tools.StringExtensions;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2015_22
{
	[AoCPuzzle(Year = 2015, Day = 22, Answer1 = "953", Answer2 = "1289")]
	public class WizardSimulator20XX : PuzzleBase
	{
		public const int Hp = 0;
		public const int Mana = 1;
		public const int Armor = 2;
		public const int Dmg = 3;

		public enum SpellEnum
		{
			MagicMissile,
			Drain,
			Shield,
			Poison,
			Recharge
		};

		public record struct Spell(SpellEnum Type, long ManaCost, long Damage, int EffectDuration)
		{
			internal bool IsEffect() => EffectDuration > 0;
			
			public override string ToString()
				=> Type.ToString();
		}

		/// <summary>
		/// order makes difference! ideal order makes best result (min mana cost) sooner
		/// </summary>
		public static readonly Spell[] _spells = new Spell[]
		{
			new Spell(SpellEnum.Poison, 173, 0, 6),
			new Spell(SpellEnum.Recharge, 229, 0, 5),
			new Spell(SpellEnum.MagicMissile, 53, 4, 0),
			new Spell(SpellEnum.Drain, 73, 2, 0),
			new Spell(SpellEnum.Shield, 113, 0, 6),
		};

		public struct ActiveEffect
		{
			public SpellEnum Effect;
			public int TurnsLeft;

			public ActiveEffect(Spell effect)
			{
				Effect = effect.Type;
				TurnsLeft = effect.EffectDuration;
			}

			public override string ToString() => $"{Effect}/{TurnsLeft}";
		}

		public struct FightState
		{
			public v4i Boss;
			public v4i Player;
			public ActiveEffect[] ActiveEffects;

			public long Turn;
			public long ManaSpent;
			public long MinManaSpent;

			public bool IsBossTurn => Turn % 2 == 0;

			public FightState NextTurn()
			{
				Debug.Line(ToString());
				Debug.Indent++;

				return new()
				{
					Turn = Turn + 1,
					Boss = Boss,
					Player = Player,
					ManaSpent = ManaSpent,
					MinManaSpent = MinManaSpent,
					ActiveEffects = ActiveEffects.ToArray(),
				};
			}

			public FightState Copy() => new()
			{
				Turn = Turn,
				Boss = Boss,
				Player = Player,
				ManaSpent = ManaSpent,
				MinManaSpent = MinManaSpent,
				ActiveEffects = ActiveEffects.ToArray(),
			};

			public override string ToString() =>
				$"#{Turn} p{Player} x b{Boss} m[{ManaSpent}/{MinManaSpent}]" +
				$" ae[{string.Join(", ", ActiveEffects.Select(ae => ae.ToString()))}]";
		}

		protected override (string Part1, string Part2) SolvePuzzle(string[] input)
		{
			var initState = new FightState
			{
				Turn = 1,
				Player = new(50, 500, 0, 0),
				Boss = new(input[0].ToNumArray()[0], 0, 0, input[1].ToNumArray()[0]),
				ActiveEffects = Array.Empty<ActiveEffect>(),
				MinManaSpent = long.MaxValue,
				ManaSpent = 0,
			};

			// part1
			var answer1 = SimMinManaCost(initState);

			// part2
			var answer2 = SimMinManaCost(initState, true);

			return (answer1.ToString(), answer2.ToString());
		}

		private static long SimMinManaCost(FightState state,
			bool hardMode = false)
		{
			if (state.ManaSpent >= state.MinManaSpent)
				return state.MinManaSpent;

			// start of turn
			// remove expired effects
			state.ActiveEffects = state.ActiveEffects.Where(e => e.TurnsLeft > 0).ToArray();

			if (hardMode && !state.IsBossTurn)
			{
				state.Player[Hp] -= 1;
				if (state.Player[Hp] <= 0)
				{
					Debug.Line("player died");
					Debug.Indent--;

					return long.MaxValue;
				}
			}

			// apply effects
			for (var ae = 0; ae < state.ActiveEffects.Length; ae++)
				state.ActiveEffects[ae].TurnsLeft--;

			if (state.ActiveEffects.Any(ae => ae.Effect == SpellEnum.Recharge))
				state.Player[Mana] += 101;

			if (state.ActiveEffects.Any(ae => ae.Effect == SpellEnum.Poison))
			{
				state.Boss[Hp] -= 3;
				if (state.Boss[Hp] <= 0)
				{
					Debug.Line($"boss died from {SpellEnum.Poison}");
					Debug.Indent--;

					return state.ManaSpent;
				}
			}

			// boss turn
			if (state.IsBossTurn)
			{
				var playerArmor = state.Player[Armor];
				if (state.ActiveEffects.Any(ae => ae.Effect == SpellEnum.Shield))
					playerArmor += 7;

				state.Player[Hp] -= System.Math.Max(1, state.Boss[Dmg] - playerArmor);
				if (state.Player[Hp] <= 0)
				{
					Debug.Line("player died");
					Debug.Indent--;

					return long.MaxValue;
				}

				return SimMinManaCost(state.NextTurn(), hardMode);
			}

			// player turn
			foreach (var spell in _spells)
			{
				if (state.Player[Mana] <= spell.ManaCost)
					continue;

				if (spell.IsEffect() && state.ActiveEffects.Any(ae => ae.Effect == spell.Type && ae.TurnsLeft > 0))
					continue;

				var fork = state.Copy();
				fork.Player[Mana] -= spell.ManaCost;
				fork.ManaSpent += spell.ManaCost;

				if (spell.IsEffect())
					fork.ActiveEffects = new ActiveEffect[] { new ActiveEffect(spell) }.Concat(fork.ActiveEffects).ToArray();

				else
				{
					if (spell.Type == SpellEnum.Drain)
						fork.Player[Hp] += 2;

					fork.Boss[Hp] -= spell.Damage;
				}

				var forkResult = fork.ManaSpent;
				if (fork.Boss[Hp] > 0)
					forkResult = SimMinManaCost(fork.NextTurn(), hardMode);
				else
				{
					Debug.Line($"boss died from {spell}");
					Debug.Indent--;
				}

				state.MinManaSpent = System.Math.Min(forkResult, state.MinManaSpent);
			}

			Debug.Indent -= 2;
			return state.MinManaSpent;
		}
	}
}
