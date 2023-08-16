using Ujeby.AoC.Common;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2021_23
{
	[AoCPuzzle(Year = 2021, Day = 23, Answer1 = "10607", Answer2 = "59071", Skip = true)]
	public class Amphipod : PuzzleBase
	{
		private static readonly Dictionary<char, int> _amphipodeEnergy = new()
		{
			{ 'A', 1 },
			{ 'B', 10 },
			{ 'C', 100 },
			{ 'D', 1000 },
		}; 

		internal record struct Amph(char Type, v2i Position)
		{
			public override string ToString() => $"{Type}{Position}";
		}

		internal struct State
		{
			public Amph[] Amphipods;
			public long EnergyUsed;
			private readonly char[][] _map;

			public State(string[] map)
			{
				EnergyUsed = 0L;

				var amphipods = new List<Amph>();
				for (var y = 0; y < map.Length; y++)
					for (var x = 0; x < map[y].Length; x++)
						if (char.IsLetter(map[y][x]))
							amphipods.Add(new Amph(map[y][x], new(x, y)));
				Amphipods = amphipods.ToArray();

				_map = map.Select(l => l.Replace('.', ' ').ToCharArray()).ToArray();
				Draw();
			}

			public State(State prev)
			{
				EnergyUsed = prev.EnergyUsed;
				Amphipods = prev.Amphipods.ToArray();
				_map = prev._map.Select(l => l.ToArray()).ToArray();
			}

			public void Move(int amphIdx, v2i destination)
			{
				_map[Amphipods[amphIdx].Position.Y][(int)Amphipods[amphIdx].Position.X] = ' ';
				_map[destination.Y][(int)destination.X] = Amphipods[amphIdx].Type;

				EnergyUsed += _amphipodeEnergy[Amphipods[amphIdx].Type] * v2i.ManhDistance(destination, Amphipods[amphIdx].Position);
				Amphipods[amphIdx].Position = destination;
			}

			public override string ToString() => $"{EnergyUsed}!{string.Join('+', Amphipods.Select(a => a.ToString()))}";

			public static Dictionary<char, v2i[]> Rooms;

			public void Draw()
			{
				Debug.Line(ToString());
				for (var y = 0; y < _map.Length; y++)
					Debug.Line(new string(_map[y]));
				Debug.Line();
			}

			internal bool Empty(v2i position)
				=> _map[position.Y][(int)position.X] == ' ';

			internal char OccupiedBy(v2i position)
				=> _map[position.Y][(int)position.X];

			internal bool Complete()
			{
				for (var ia = 0; ia < Amphipods.Length; ia++)
				{
					var amph = Amphipods[ia];
					if (amph.Position.Y < 2)
						return false;

					if (Rooms[amph.Type][0].X != amph.Position.X)
						return false;
				}

				return true;
			}
		}

		protected override (string, string) SolvePuzzle(string[] input)
		{
			Debug.Line();

			// TODO 2021/23 OPTIMIZE (13s)

			// part1
			State.Rooms = new Dictionary<char, v2i[]>
			{
				{ 'A', Enumerable.Range(2, 2).Select(y => new v2i(3, y)).ToArray() },
				{ 'B', Enumerable.Range(2, 2).Select(y => new v2i(5, y)).ToArray() },
				{ 'C', Enumerable.Range(2, 2).Select(y => new v2i(7, y)).ToArray() },
				{ 'D', Enumerable.Range(2, 2).Select(y => new v2i(9, y)).ToArray() },
			};
			long? answer1 = Step(new State(input));

			// part2
			State.Rooms = new Dictionary<char, v2i[]>
			{
				{ 'A', Enumerable.Range(2, 4).Select(y => new v2i(3, y)).ToArray() },
				{ 'B', Enumerable.Range(2, 4).Select(y => new v2i(5, y)).ToArray() },
				{ 'C', Enumerable.Range(2, 4).Select(y => new v2i(7, y)).ToArray() },
				{ 'D', Enumerable.Range(2, 4).Select(y => new v2i(9, y)).ToArray() },
			};
			_cache.Clear();
			long? answer2 = Step(
				new State(
					input.Take(3).Concat(new string[]
					{
						"  #D#C#B#A#",
						"  #D#B#A#C#",
					}).Concat(input.Skip(3)).ToArray()));

			return (answer1?.ToString(), answer2?.ToString());
		}

		private static readonly Dictionary<string, long> _cache = new();

		private static long Step(State state)
		{
			var cacheKey = state.ToString();
			if (_cache.ContainsKey(cacheKey))
				return _cache[cacheKey];

			// amphipods in hallway
			var amphipodsInHallway = new List<int>();
			for (var ia = 0; ia < state.Amphipods.Length; ia++)
				if (state.Amphipods[ia].Position.Y == 1)
					amphipodsInHallway.Add(ia);

			// try to move all amphipods from hallway to their rooms
			var movementInHallway = true;
			while (movementInHallway)
			{
				movementInHallway = false;
				foreach (var ia in amphipodsInHallway)
				{
					var amph = state.Amphipods[ia];
					var amphRooms = State.Rooms[amph.Type];

					var p = amph.Position;
					var rx = amphRooms[0].X;
					var dir = new v2i(amph.Position.X > rx ? -1 : 1, 0);
					for (p += dir; p.X != rx && state.Empty(p); p += dir)
					{
					}

					// if path to desired room is blocked
					if (p.X != rx)
						continue;

					for (var ir = amphRooms.Length - 1; ir >= 0; ir--)
					{
						// if destination room is empty
						if (state.OccupiedBy(amphRooms[ir]) == ' ' &&
							Enumerable.Range(ir + 1, amphRooms.Length - ir - 1)
								.All(i => state.OccupiedBy(amphRooms[i]) == amph.Type))
						{
							state.Move(ia, amphRooms[ir]);

							amphipodsInHallway.Remove(ia);
							movementInHallway = true;
							break;
						}
					}

					if (movementInHallway)
						break;
				}
			}

			if (state.Complete())
				return state.EnergyUsed;

			var leastEnergyUsed = long.MaxValue;
			for (var ia = 0; ia < state.Amphipods.Length; ia++)
			{
				var amph = state.Amphipods[ia];
				var amphRooms = State.Rooms[amph.Type];

				// skip those that stayed in hallway
				if (amphipodsInHallway.Contains(ia))
					continue;

				// amph cannot move out of room
				if (!state.Empty(amph.Position + v2i.Down))
					continue;

				// amph in wrong room, or in correct room but blocking somebody else who shouldnt be there
				if (amphRooms[0].X != amph.Position.X ||
					Enumerable.Range((int)amph.Position.Y + 1, amphRooms.Length - ((int)amph.Position.Y - 2) - 1)
						.Any(ry => state.OccupiedBy(new v2i(amph.Position.X, ry)) != amph.Type))
				{
					var apx = state.Amphipods[ia].Position.X;

					var hall = new v2i(apx - 1, 1);
					for (; hall.X > 0 && state.Empty(hall); hall.X--)
						leastEnergyUsed = StepToHallway(state, ia, hall, leastEnergyUsed);

					hall = new v2i(apx + 1, 1);
					for (; hall.X < 12 && state.Empty(hall); hall.X++)
						leastEnergyUsed = StepToHallway(state, ia, hall, leastEnergyUsed);
				}
			}

			_cache[state.ToString()] = leastEnergyUsed;
			return leastEnergyUsed;
		}

		private static long StepToHallway(State state, int amphIdx, v2i hallway, long leastEnergyUsed)
		{
			if (hallway.X == 3 || hallway.X == 5 || hallway.X == 7 || hallway.X == 9)
				return leastEnergyUsed;

			return StepTo(state, amphIdx, hallway, leastEnergyUsed);
		}

		private static long StepTo(State state, int amphIdx, v2i destination, long leastEnergyUsed)
		{
			var nextState = new State(state);
			nextState.Move(amphIdx, destination);

			if (nextState.Complete())
				return nextState.EnergyUsed;

			return Math.Min(Step(nextState), leastEnergyUsed);
		}
	}
}
