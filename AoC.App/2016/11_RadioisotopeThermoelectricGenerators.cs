using Ujeby.AoC.Common;

namespace Ujeby.AoC.App._2016_11;

[AoCPuzzle(Year = 2016, Day = 11, Answer1 = null, Answer2 = null, Skip = false)]
public class RadioisotopeThermoelectricGenerators : PuzzleBase
{
	private enum ComponentyType : int
	{
		Generator = 0,
		Microchip = 1
	}

	private int[] _elevatorMovements = new int[] { 1, -1 };

	private struct Component
	{
		public string Name;
		public ComponentyType Type;
		public int Floor;

		public Component(string name, ComponentyType type, int floor)
		{
			Name = name;
			Type = type;
			Floor = floor;
		}

		public override string ToString()
			=> $"{Name}-{Type}-{Floor}";
	}

	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var components = new Component[]
		{
			new("hydrogen", ComponentyType.Microchip, 1),
			new("hydrogen", ComponentyType.Generator, 2),
			new("lithium", ComponentyType.Microchip, 1),
			new("lithium", ComponentyType.Generator, 3),
		};

		// part1
		string answer1 = null;// CountSteps(components);

		// part2
		string answer2 = null;

		return (answer1?.ToString(), answer2?.ToString());
	}

	private static Dictionary<string, long> _cache = new();

	private long CountSteps(Component[] components, 
		long minSteps = long.MaxValue, string prevCompHash = null, int elevator = 1, long steps = 0)
	{
		var hash = GetCompHash(components);
		if (_cache.TryGetValue(hash, out long cachedSteps))
			return cachedSteps;

		// all components at top floor
		if (elevator == 4 && components.All(c => c.Floor == 4))
			return steps;

		// check floor
		var singleComponents = components
			.Where(c => c.Floor == elevator)
			.GroupBy(c => c.Name)
			.Where(g => g.Count() == 1)
			.ToArray();
		if (singleComponents.Length > 1 &&
			singleComponents.Any(c => c.Single().Type == ComponentyType.Microchip) &&
			singleComponents.Any(c => c.Single().Type == ComponentyType.Generator))
			return long.MaxValue;

		Debug.Line($"steps={steps}, elevator={elevator}");
		PrintFloors(components);

		var floorCompIds = Enumerable.Range(0, components.Length).Where(x => components[x].Floor == elevator).ToArray();

		// fork: move every 2 components with elevator
		if (floorCompIds.Length > 1)
		{
			var floorPairCompIds = Alg.Combinatorics.Combinations(floorCompIds, 2).ToArray();
			foreach (var c2Id in floorPairCompIds)
				minSteps = MoveElevator(components, prevCompHash, elevator + 1, steps + 1, minSteps, c2Id.ToArray());
		}

		// fork: move every component with elevator
		foreach (var cId in floorCompIds)
			foreach (var mov in _elevatorMovements)
				minSteps = MoveElevator(components, prevCompHash, elevator + mov, steps + 1, minSteps, cId);

		_cache.Add(hash, minSteps);

		return minSteps;
	}

	private static void PrintFloors(Component[] components)
	{
		Debug.Line($"F4 {string.Join(" ", components.Where(c => c.Floor == 4).Select(c => c.ToString()[..^2]))}");
		Debug.Line($"F3 {string.Join(" ", components.Where(c => c.Floor == 3).Select(c => c.ToString()[..^2]))}");
		Debug.Line($"F2 {string.Join(" ", components.Where(c => c.Floor == 2).Select(c => c.ToString()[..^2]))}");
		Debug.Line($"F1 {string.Join(" ", components.Where(c => c.Floor == 1).Select(c => c.ToString()[..^2]))}");
		Debug.Line();
	}

	private long MoveElevator(Component[] components, string prevCompHash, int destFloor, long steps, long minSteps,
		params int[] cIdToMove)
	{
		if (destFloor > 4 || destFloor < 1)
			return minSteps;

		// move selected components to destination floor
		var tmp = components.ToArray();
		foreach (var cId in cIdToMove)
			tmp[cId].Floor = destFloor;

		if (prevCompHash == GetCompHash(tmp))
			return minSteps;

		return System.Math.Min(minSteps, CountSteps(tmp, minSteps, GetCompHash(components), destFloor, steps));
	}

	private static string GetCompHash(Component[] components)
		=> string.Join(":", components.OrderBy(c => c.ToString()));
}