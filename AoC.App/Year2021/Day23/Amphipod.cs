using Ujeby.AoC.Common;
using Ujeby.Vectors;

namespace Ujeby.AoC.App.Year2021.Day23
{
	public struct Amph
	{
		public char Type;
		public v2i Position;

		public Amph(char type, v2i position)
		{
			Type = type;
			Position = position;
		}

		public override string ToString() => $"{Type}{Position}";

		public static readonly char[] Types = new char[] { 'A', 'B', 'C', 'D' };
		public static readonly Dictionary<char, int> Energy = new Dictionary<char, int>
		{
			{ 'A', 1 },
			{ 'B', 10 },
			{ 'C', 100 },
			{ 'D', 1000 },
		};
	}

	public struct State
	{
		public Amph[] Amphipods;
		public long EnergyUsed;

		public State(State prev)
		{
			EnergyUsed = prev.EnergyUsed;
			Amphipods = prev.Amphipods.ToArray();
		}

		public State(State prev, int amphIdx, v2i destination) : this(prev)
		{
			var energy = Amph.Energy[Amphipods[amphIdx].Type] * v2i.ManhDistance(destination, Amphipods[amphIdx].Position);

			EnergyUsed += energy;
			Amphipods[amphIdx].Position = destination;
		}

		public override string ToString() => $"{EnergyUsed}{string.Join('+', Amphipods.Select(a => a.ToString()))}";

		public static string[] Map;
		public static Dictionary<char, v2i[]> Rooms;

		public static readonly v2i[] Hallway =
			Enumerable.Range(1, 11).Except(new int[] { 3, 5, 7, 9 }).Select(x => new v2i(x, 1)).ToArray();

		public void Draw()
		{
			Debug.Line(ToString());
			for (var y = 0; y < Map.Length; y++)
			{
				var line = Map[y].ToCharArray();
				foreach (var a in Amphipods)
					if (a.Position.Y == y)
						line[(int)a.Position.X] = a.Type;
				Debug.Line(new string(line));
			}

			Debug.Line($"{nameof(EnergyUsed)}={EnergyUsed}");
			Debug.Line();
		}

		internal bool RoomComplete(char amphipodType)
			=> Amphipods.Where(a => a.Type == amphipodType).All(a => Rooms[amphipodType].Contains(a.Position));

		internal bool IsEmpty(v2i position)
			=> Map[position.Y][(int)position.X] != '#' && Amphipods.All(a => a.Position != position);

		internal char OccupiedBy(v2i position)
			=> Amphipods.SingleOrDefault(a => a.Position == position).Type;
	}

	public class Amphipod : PuzzleBase
	{
		protected override (string, string) SolvePuzzle(string[] input)
		{
			// part1
			State.Map = new string[]
			{
				"#############",
				"#           #",
				"### # # # ###",
				"  # # # # #",
				"  #########"
			};
			State.Rooms = new Dictionary<char, v2i[]>
			{
				{ 'A', new v2i[] { new(3, 2), new(3, 3) } },
				{ 'B', new v2i[] { new(5, 2), new(5, 3) } },
				{ 'C', new v2i[] { new(7, 2), new(7, 3) } },
				{ 'D', new v2i[] { new(9, 2), new(9, 3) } },
			};
			var start = new State
			{
				Amphipods = new Amph[]
				{
					new Amph('A', new(3, 2)),
					new Amph('A', new(3, 3)),
					new Amph('B', new(5, 2)),
					new Amph('B', new(5, 3)),
					new Amph('C', new(7, 2)),
					new Amph('C', new(7, 3)),
					new Amph('D', new(9, 2)),
					new Amph('D', new(9, 3)),
				}.Select(fa => new Amph(input[fa.Position.Y][(int)fa.Position.X], fa.Position)).ToArray(),
			};
			long? answer1 = Step(start);

			// part2
			//var input2 = input.Take(3).Concat(new string[]
			//{
			//	"  #D#C#B#A#",
			//	"  #D#B#A#C#",
			//}).Concat(input.Skip(3)).ToArray();
			//State.Map = new string[]
			//{
			//	"#############",
			//	"#           #",
			//	"### # # # ###",
			//	"  # # # # #",
			//	"  # # # # #",
			//	"  # # # # #",
			//	"  #########"
			//};
			//State.Rooms = new Dictionary<char, v2i[]>
			//{
			//	{ 'A', new v2i[] { new(3, 2), new(3, 3), new(3, 4), new(3, 5) } },
			//	{ 'B', new v2i[] { new(5, 2), new(5, 3), new(5, 4), new(5, 5) } },
			//	{ 'C', new v2i[] { new(7, 2), new(7, 3), new(7, 4), new(7, 5) } },
			//	{ 'D', new v2i[] { new(9, 2), new(9, 3), new(9, 4), new(9, 5) } },
			//};

			//var start2 = new State
			//{
			//	Amphipods = new Amph[]
			//	{
			//		new Amph('A', new(3, 2)),
			//		new Amph('A', new(3, 3)),
			//		new Amph('A', new(3, 4)),
			//		new Amph('A', new(3, 5)),
			//		new Amph('B', new(5, 2)),
			//		new Amph('B', new(5, 3)),
			//		new Amph('B', new(5, 4)),
			//		new Amph('B', new(5, 5)),
			//		new Amph('C', new(7, 2)),
			//		new Amph('C', new(7, 3)),
			//		new Amph('C', new(7, 4)),
			//		new Amph('C', new(7, 5)),
			//		new Amph('D', new(9, 2)),
			//		new Amph('D', new(9, 3)),
			//		new Amph('D', new(9, 4)),
			//		new Amph('D', new(9, 5)),
			//	}.Select(fa => new Amph(input2[fa.Position.Y][(int)fa.Position.X], fa.Position)).ToArray(),
			//};
			long? answer2 = null;// Step(start2);

			return (answer1?.ToString(), answer2?.ToString());
		}

		private static Dictionary<string, long> _cache = new Dictionary<string, long>();

		public static long Step(State state)
		{
			var cacheKey = state.ToString();
			if (_cache.ContainsKey(cacheKey))
				return _cache[cacheKey];

			// if all rooms are completed
			if (Amph.Types.All(at => state.RoomComplete(at)))
			{
				_cache[cacheKey] = state.EnergyUsed;
				return state.EnergyUsed;
			}

			var leastEnergyUsed = long.MaxValue;
			for (var ia = 0; ia < state.Amphipods.Length; ia++)
			{
				var amph = state.Amphipods[ia];
				var amphRooms = State.Rooms[amph.Type];

				// if amphipode is in correct back room, or room is completed
				if (amph.Position == amphRooms[1] || state.RoomComplete(amph.Type))
					continue;

				// if amphipode cant move in any direction
				if (v2i.RightDownLeftUp.All(d => !state.IsEmpty(amph.Position + d)))
					continue;

				// amphipode in hallway
				if (State.Hallway.Contains(amph.Position))
				{
					// check if path to room is clear
					var rx = amphRooms[0].X;
					var dir = new v2i(amph.Position.X > rx ? -1 : 1, 0);
					var p = amph.Position;
					for (p += dir; p.X != rx; p += dir)
						if (!state.IsEmpty(p))
							break;

					// if path to room is cleared
					if (p.X == rx)
					{
						for (var ir = amphRooms.Length - 1; ir >= 0; ir--)
						{
							// if back room is empty
							if (state.OccupiedBy(amphRooms[ir]) == 0 && 
								Enumerable.Range(ir + 1, amphRooms.Length - ir - 1).All(i => state.OccupiedBy(amphRooms[i]) == amph.Type))
							{
								leastEnergyUsed = MoveTo(state, ia, amphRooms[ir], leastEnergyUsed, amph);
								break;
							}
						}

						//// if back room is empty
						//if (state.OccupiedBy(amphRooms[1]) == 0)
						//{
						//	leastEnergyUsed = MoveTo(state, ia, amphRooms[1], leastEnergyUsed, amph);
						//	break;
						//}
						//// if backroom is occupied by correct type and frontroom is empty	
						//else if (state.OccupiedBy(amphRooms[0]) == 0 && state.OccupiedBy(amphRooms[1]) == amph.Type)
						//{
						//	leastEnergyUsed = MoveTo(state, ia, amphRooms[0], leastEnergyUsed, amph);
						//	break;
						//}
					}
				}
				else
				{
					// amphipode in wrong room, or maybe right, but blocking somebody else
					// should move to hallway
					var hall = new v2i(amph.Position.X - 1, 1);
					for (; hall.X > 0 && state.IsEmpty(hall); hall.X--)
						leastEnergyUsed = MoveToHallway(state, ia, hall, leastEnergyUsed, amph);

					hall = new v2i(amph.Position.X + 1, 1);
					for (; hall.X < 12 && state.IsEmpty(hall); hall.X++)
						leastEnergyUsed = MoveToHallway(state, ia, hall, leastEnergyUsed, amph);
				}
			}

			_cache[cacheKey] = leastEnergyUsed;
			return leastEnergyUsed;
		}

		private static long MoveToHallway(State state, int amphIdx, v2i hallway, long leastEnergyUsed, Amph amph)
		{
			if (hallway.X == 3 || hallway.X == 5 || hallway.X == 7 || hallway.X == 9)
				return leastEnergyUsed;

			return MoveTo(state, amphIdx, hallway, leastEnergyUsed, amph);
		}

		private static long MoveTo(State state, int amphIdx, v2i destination, long leastEnergyUsed, Amph amph)
		{
			return Math.Min(Step(new State(state, amphIdx, destination)), leastEnergyUsed);
		}
	}
}
