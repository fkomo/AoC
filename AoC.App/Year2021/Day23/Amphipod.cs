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

		public void Move(int amphIdx, v2i destination)
		{
			EnergyUsed += Amph.Energy[Amphipods[amphIdx].Type] * v2i.ManhDistance(destination, Amphipods[amphIdx].Position);
			Amphipods[amphIdx].Position = destination;
		}

		public override string ToString() => $"{EnergyUsed}!{string.Join('+', Amphipods.Select(a => a.ToString()))}";

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
			Debug.Line();
		}

		internal bool RoomComplete(char amphipodType)
			=> Amphipods.Where(a => a.Type == amphipodType).All(a => Rooms[amphipodType].Contains(a.Position));

		internal bool Empty(v2i position)
			=> Map[position.Y][(int)position.X] != '#' && !Amphipods.Any(a => a.Position == position);

		internal char OccupiedBy(v2i position)
			=> Amphipods.SingleOrDefault(a => a.Position == position).Type;

		internal bool Complete()
		{
			var t = this;
			return Amph.Types.All(at => t.RoomComplete(at));
		}
	}

	public class Amphipod : PuzzleBase
	{
		protected override (string, string) SolvePuzzle(string[] input)
		{
			Debug.Line();

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
			start.Draw();

			_cache.Clear();
			long? answer1 = Step(start);
			//long? answer1 = null;

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
			//start2.Draw();

			//_cache.Clear();
			//long? answer2 = Step(start2);
			long? answer2 = null;

			return (answer1?.ToString(), answer2?.ToString());
		}

		private static Dictionary<string, long> _cache = new Dictionary<string, long>();

		public static long Step(State state)
		{
			//state.Draw();

			var cacheKey = state.ToString();
			if (_cache.ContainsKey(cacheKey))
				return _cache[cacheKey];

			var amphipodsToMove = new List<int>();

			// first move amphs from hallway to correct rooms
			for (var ia = 0; ia < state.Amphipods.Length; ia++)
			{
				var amph = state.Amphipods[ia];
				var amphRooms = State.Rooms[amph.Type];

				// if amphipode is in correct room
				if (amph.Position.Y > 1 && amph.Position.X == amphRooms[0].X)
				{
					//if (amph.Position == amphRooms[1])
					//	continue;

					var ir = (int)amph.Position.Y - 2;
					if (Enumerable.Range(ir + 1, amphRooms.Length - ir - 1)
						.All(i => state.OccupiedBy(amphRooms[i]) == amph.Type))
						continue;
				}

				// if room is completed
				if (state.RoomComplete(amph.Type))
					continue;

				// if amphipode cant move in any direction
				if (v2i.RightDownLeftUp.All(d => !state.Empty(amph.Position + d)))
					continue;

				// amphipode in hallway
				if (!State.Hallway.Contains(amph.Position))
				{
					amphipodsToMove.Add(ia);
					continue;
				}

				var p = amph.Position;
				var rx = amphRooms[0].X;
				var dir = new v2i(amph.Position.X > rx ? -1 : 1, 0);
				for (p += dir; p.X != rx && state.Empty(p); p += dir)
				{
				}

				// if path to room is blocked
				if (p.X != rx)
					continue;

				for (var ir = amphRooms.Length - 1; ir >= 0; ir--)
				{
					// if correct destination room is empty
					if (state.OccupiedBy(amphRooms[ir]) == 0 &&
						Enumerable.Range(ir + 1, amphRooms.Length - ir - 1).All(i => state.OccupiedBy(amphRooms[i]) == amph.Type))
					{
						state.Move(ia, amphRooms[ir]);
						//state.Draw();
						break;
					}
				}
			}

			cacheKey = state.ToString();
			if (state.Complete())
			{
				_cache[cacheKey] = state.EnergyUsed; 
				return state.EnergyUsed;
			}

			// go through all remaining possibilities
			var leastEnergyUsed = long.MaxValue;
			foreach (var ia in amphipodsToMove)
			{
				var apx = state.Amphipods[ia].Position.X;

				// amphipode in wrong room, or maybe right, but blocking somebody else, should move to hallway
				var hall = new v2i(apx - 1, 1);
				for (; hall.X > 0 && state.Empty(hall); hall.X--)
					leastEnergyUsed = MoveToHallway(state, ia, hall, leastEnergyUsed);

				hall = new v2i(apx + 1, 1);
				for (; hall.X < 12 && state.Empty(hall); hall.X++)
					leastEnergyUsed = MoveToHallway(state, ia, hall, leastEnergyUsed);
			}

			_cache[cacheKey] = leastEnergyUsed;
			return leastEnergyUsed;
		}

		private static long MoveToHallway(State state, int amphIdx, v2i hallway, long leastEnergyUsed)
		{
			if (hallway.X == 3 || hallway.X == 5 || hallway.X == 7 || hallway.X == 9)
				return leastEnergyUsed;

			return MoveTo(state, amphIdx, hallway, leastEnergyUsed);
		}

		private static long MoveTo(State state, int amphIdx, v2i destination, long leastEnergyUsed)
		{
			var nextState = new State(state);
			nextState.Move(amphIdx, destination);

			if (nextState.Complete())
				return nextState.EnergyUsed;

			return Math.Min(Step(nextState), leastEnergyUsed);
		}
	}
}
