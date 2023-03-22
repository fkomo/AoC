using Ujeby.AoC.Common;
using Ujeby.Tools.StringExtensions;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2015_22
{
	[AoCPuzzle(Year = 2015, Day = 22, Answer1 = "953", Answer2 = null)]
	public class WizardSimulator20XX : PuzzleBase
	{
		public enum SpellEnum : int
		{
			MagicMissile = 0,
			Drain
		}

		public enum EffectEnum : int
		{
			Shield = 2,
			Poison,
			Recharge
		};

		public const int Hp = 0;
		public const int Mana = 1;
		public const int Armor = 2;
		public const int Dmg = 3;

		public static readonly int[] _manaCost = new int[]
		{
			53, 73, 113, 173, 229
		};

		public static readonly int[] _spellDamage = new int[]
		{
			4, 2, 0, 0, 0
		};

		public static readonly int[] _effectTurns = new int[]
		{
			0, 0, 6, 6, 5
		};

		public struct ActiveEffect
		{
			public EffectEnum Effect;
			public int TurnsLeft;

			public ActiveEffect(EffectEnum effect)
			{
				Effect = effect;
				TurnsLeft = _effectTurns[(int)effect];
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
					ActiveEffects = ActiveEffects.ToArray(),
				};
			}

			public FightState Copy() => new()
			{
				Turn = Turn,
				Boss = Boss,
				Player = Player,
				ManaSpent = ManaSpent,
				ActiveEffects = ActiveEffects.ToArray(),
			};

			public override string ToString()
				=> $"#{Turn} p{Player} x b{Boss} m={ManaSpent} ae[{string.Join(", ", ActiveEffects.Select(ae => ae.ToString()))}]";
		}

		protected override (string Part1, string Part2) SolvePuzzle(string[] input)
		{
			var boss = new v4i(input[0].ToNumArray()[0], 0, 0, input[1].ToNumArray()[0]);
			var player = new v4i(50, 500, 0, 0);

			var initState = new FightState
			{
				Boss = boss,
				Player = player,
				ActiveEffects = Array.Empty<ActiveEffect>(),
				ManaSpent = 0,
				Turn = 1,
			};

			// part1
			var answer1 = SimMinManaCost(initState);

			// part2
			string answer2 = null;

			return (answer1.ToString(), answer2?.ToString());
		}

		private static long _manaSpent = long.MaxValue;

		private static long SimMinManaCost(FightState state)
		{
			if (state.ManaSpent >= _manaSpent)
				return _manaSpent;

			// start of turn
			// remove expired effects
			state.ActiveEffects = state.ActiveEffects.Where(e => e.TurnsLeft > 0).ToArray();

			// apply effects
			for (var ae = 0; ae < state.ActiveEffects.Length; ae++)
				state.ActiveEffects[ae].TurnsLeft--;

			if (state.ActiveEffects.Any(ae => ae.Effect == EffectEnum.Recharge))
				state.Player[Mana] += 101;
			
			if (state.ActiveEffects.Any(ae => ae.Effect == EffectEnum.Poison))
			{
				state.Boss[Hp] -= 3;
				if (state.Boss[Hp] <= 0)
				{
					Debug.Line("boss died from poison");
					Debug.Indent--;

					return state.ManaSpent; // boss died from poison
				}
			}

			if (state.IsBossTurn)
			{
				var playerArmor = state.Player[Armor];
				if (state.ActiveEffects.Any(ae => ae.Effect == EffectEnum.Shield))
					playerArmor += 7;

				// boss attack
				state.Player[Hp] -= Math.Max(1, state.Boss[Dmg] - playerArmor);
				if (state.Player[Hp] <= 0)
				{
					Debug.Line("player died");
					Debug.Indent--;

					return long.MaxValue; // player died
				}

				return SimMinManaCost(state.NextTurn());
			}

			// player turn
			var result = _manaSpent;

			foreach (var spell in Enum.GetValues<EffectEnum>())
			{
				if (state.Player[Mana] <= _manaCost[(int)spell] ||
					state.ActiveEffects.Any(ae => ae.Effect == spell && ae.TurnsLeft > 0))
					continue;

				var fork = state.Copy();
				fork.Player[Mana] -= _manaCost[(int)spell];
				fork.ManaSpent += _manaCost[(int)spell];

				fork.ActiveEffects = new ActiveEffect[] { new ActiveEffect(spell) }.Concat(fork.ActiveEffects).ToArray();

				var forkResult = SimMinManaCost(fork.NextTurn());

				result = Math.Min(forkResult, result);
				_manaSpent = Math.Min(_manaSpent, result);
			}

			foreach (var spell in Enum.GetValues<SpellEnum>())
			{
				if (state.Player[Mana] <= _manaCost[(int)spell])
					continue;

				var fork = state.Copy();
				fork.Player[Mana] -= _manaCost[(int)spell];
				fork.ManaSpent += _manaCost[(int)spell];

				if (spell == SpellEnum.Drain)
					fork.Player[Hp] += 2;

				fork.Boss[Hp] -= _spellDamage[(int)spell];

				var forkResult = fork.ManaSpent;
				if (fork.Boss[Hp] > 0) // boss survived
					forkResult = SimMinManaCost(fork.NextTurn());
				else
				{
					Debug.Line($"boss died from {spell}");
					Debug.Indent--;
				}

				result = Math.Min(forkResult, result);
				_manaSpent = Math.Min(_manaSpent, result);
			}

			Debug.Indent -= 2;
			return result;
		}
	}
}
