using Ujeby.AoC.Common;
using Ujeby.Tools.StringExtensions;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2015_22
{
	[AoCPuzzle(Year = 2015, Day = 22, Answer1 = null, Answer2 = null)]
	public class WizardSimulator20XX : PuzzleBase
	{
		public enum SpellsEnum
		{
			MagicMissile = 0,
			Drain,
			Shield,
			Poison,
			Recharge
		};

		public const int Hp = 0;
		public const int Mana = 1;
		public const int Armor = 2;
		public const int Dmg = 3;

		public struct ActiveEffect
		{
			public SpellsEnum Effect;
			public int TurnsLeft;

			public ActiveEffect(SpellsEnum effect)
			{
				Effect = effect;
				TurnsLeft = (Spell.Get(effect) as Effect).Duration;
			}

			public override string ToString() => $"{Effect}: {TurnsLeft} turns left";
		}

		public struct FightState
		{
			public v4i Player;
			public v4i Boss;
			public ActiveEffect[] ActiveEffects;

			public long Turn;
			public long ManaSpent;
			public bool BossTurn;

			public FightState(FightState state)
			{
				Turn = state.Turn + 1;
				Player = state.Player;
				Boss = state.Boss;
				ManaSpent = state.ManaSpent;
				BossTurn = !state.BossTurn;
				ActiveEffects = state.ActiveEffects.ToArray();
			}

			public FightState(long manaSpent)
			{
				Turn = 0;
				Player = new();
				Boss = new();
				ManaSpent = manaSpent;
				BossTurn = false;
				ActiveEffects = Array.Empty<ActiveEffect>();
			}

			public static FightState Default => new(manaSpent: long.MaxValue);

			internal FightState EndOfTurn()
			{
				for (var ae = 0; ae < ActiveEffects.Length; ae++)
				{
					var effect = Effect.Get(ActiveEffects[ae].Effect);

					// effect not started yet
					if (effect.Duration == ActiveEffects[ae].TurnsLeft)
						continue;

					Player += effect.ToCasterAtTurnEnd();
					Boss += effect.ToTargetAtTurnEnd();
				}

				return this;
			}

			public override string ToString()
				=> $"#{Turn}/{(BossTurn ? "B" : "P")}: Player={Player}, Boss={Boss}, ManaSpent={ManaSpent}" + Environment.NewLine
				+ string.Join(" | ", ActiveEffects.Select(ae => ae.ToString()));
		}

		public abstract class Spell
		{
			public long ManaCost { get; init; }

			public Spell(long manaCost)
			{
				ManaCost = manaCost;
			}

			public virtual v4i ToCasterAfterCast() => new(0, -ManaCost, 0, 0);
			public virtual v4i ToTargetAfterCast() => new();

			protected static readonly Dictionary<SpellsEnum, Spell> _allSpells = new()
			{
				{ SpellsEnum.MagicMissile, new MagicMissile() },
				{ SpellsEnum.Drain, new Drain() },
				{ SpellsEnum.Shield, new Shield() },
				{ SpellsEnum.Poison, new Poison() },
				{ SpellsEnum.Recharge, new Recharge() }
			};

			public static Spell Get(SpellsEnum spell) => _allSpells[spell];
		}

		public class MagicMissile : Spell
		{
			public long Damage { get; init; }

			public MagicMissile(long manaCost = 53, long damage = 4) : base(manaCost)
			{
				Damage = damage;
			}

			public override v4i ToTargetAfterCast() => new(-Damage, 0, 0, 0);
		}

		public class Drain : Spell
		{
			public long Damage { get; init; }
			public long Heal { get; init; }

			public Drain(long manaCost = 73, long damage = 2, long heal = 2) : base(manaCost)
			{
				Damage = damage;
				Heal = heal;
			}

			public override v4i ToCasterAfterCast() => new(Heal, -ManaCost, 0, 0);
			public override v4i ToTargetAfterCast() => new(-Damage, 0, 0, 0);
		}

		public abstract class Effect : Spell
		{
			public int Duration { get; init; }

			protected Effect(long manaCost, int duration) : base(manaCost)
			{
				Duration = duration;
			}

			public static new Effect Get(SpellsEnum spell) => _allSpells[spell] as Effect;

			public virtual v4i ToCasterAtTurnStart() => new();
			public virtual v4i ToCasterAtTurnEnd() => new();
			public virtual v4i ToTargetAtTurnStart() => new();
			public virtual v4i ToTargetAtTurnEnd() => new();
		}

		public class Shield : Effect
		{
			public long Armor { get; init; }

			public Shield(long manaCost = 113, int duration = 6, long armor = 7) : base(manaCost, duration)
			{
				Armor = armor;
			}

			public override v4i ToCasterAtTurnStart() => new(0, 0, Armor, 0);
			public override v4i ToCasterAtTurnEnd() => new(0, 0, -Armor, 0);
		}

		public class Poison : Effect
		{
			public long Damage { get; init; }

			public Poison(long manaCost = 173, int duration = 6, long damage = 3) : base(manaCost, duration)
			{
				Damage = damage;
			}

			public override v4i ToTargetAtTurnStart() => new(-Damage, 0, 0, 0);
		}

		public class Recharge : Effect
		{
			public long Mana { get; init; }

			public Recharge(long manaCost = 229, int duration = 5, long mana = 101) : base(manaCost, duration)
			{
				Mana = mana;
			}

			public override v4i ToCasterAtTurnStart() => new(0, Mana, 0, 0);
		}

		protected override (string Part1, string Part2) SolvePuzzle(string[] input)
		{
			var boss = new v4i(input[0].ToNumArray()[0], 0, 0, input[1].ToNumArray()[0]);
#if _DEBUG_SAMPLE
			var player = new v4i(10, 250, 0, 0);
#else
			var player = new v4i(50, 500, 0, 0);
#endif
			// part1
			string answer1 = null;
			//var answer1 = SimMinManaCost(new FightState
			//{
			//	Boss = boss,
			//	Player = player,
			//	ManaSpent = 0,
			//	BossTurn = false,
			//	ActiveEffects = Array.Empty<ActiveEffect>(),
			//}).ManaSpent;

			// 943 too low
			string answer2 = null;

			// part2

			return (answer1?.ToString(), answer2?.ToString());
		}

		private FightState SimMinManaCost(FightState state)
		{
			// start of turn

			// remove expired effects
			state.ActiveEffects = state.ActiveEffects.Where(e => e.TurnsLeft > 0).ToArray();

			// apply active effects
			for (var ae = 0; ae < state.ActiveEffects.Length; ae++)
			{
				var effect = Effect.Get(state.ActiveEffects[ae].Effect);
				state.Player += effect.ToCasterAtTurnStart();
				if (state.Player[Hp] <= 0)
					return FightState.Default; // player died

				state.Boss += effect.ToTargetAtTurnStart();
				if (state.Boss[Hp] <= 0)
					return state; // boss died

				state.ActiveEffects[ae].TurnsLeft--;
			}

			// boss turn, just do damage to player
			if (state.BossTurn)
			{
				state.Player[Hp] -= Math.Max(1, state.Boss[Dmg] - state.Player[Armor]);
				if (state.Player[Hp] <= 0)
					return FightState.Default; // player died

				// end of boss turn
				state = state.EndOfTurn();
				state.BossTurn = false;
				state.Turn++;

				Debug.Line(state.ToString());

				// next turn
				return SimMinManaCost(state);
			}

			// player turn, fork all possible spells and find best result
			var bestState = FightState.Default;
			foreach (var spellEnum in Enum.GetValues<SpellsEnum>())
			{
				// cast new spell
				var newSpell = Spell.Get(spellEnum);

				// not enough mana
				if (state.Player[Mana] < newSpell.ManaCost)
					continue;

				var newState = new FightState(state);
				newState.ManaSpent += newSpell.ManaCost;

				// effect
				if (newSpell is Effect effect)
				{
					// cant cast effect that is already active
					if (newState.ActiveEffects.Any(ae => ae.TurnsLeft > 0 && ae.Effect == spellEnum))
						continue;

					// add new effect
					newState.ActiveEffects = new ActiveEffect[] { new ActiveEffect(spellEnum) }.Concat(newState.ActiveEffects).ToArray();
				}
				else // spell
				{
					newState.Player += newSpell.ToCasterAfterCast();
					if (newState.Player[Hp] <= 0 || newState.Player[Mana] <= 0) // player died or mana spent
						return FightState.Default;

					newState.Boss += newSpell.ToTargetAfterCast();
					if (newState.Boss[Hp] <= 0) // boss died
						return newState;
				}

				// end of player turn
				newState = newState.EndOfTurn();

				Debug.Line(newState.ToString());

				// next turn
				var finalState = SimMinManaCost(newState);
				if (finalState.ManaSpent < bestState.ManaSpent)
					bestState = finalState;
			}

			return bestState;
		}
	}
}
