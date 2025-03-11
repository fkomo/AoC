using Ujeby.AoC.Common;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2018_20;

[AoCPuzzle(Year = 2018, Day = 20, Answer1 = "3879", Answer2 = "8464", Skip = false)]
public class ARegularMap : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		// part1
		CreateMap(input[0], out HashSet<v2i> rooms, out HashSet<v2i> doors);

		var distance = Ujeby.Alg.Graph.SeedFillDistance(v2i.Zero, [.. v2i.UpDownLeftRight.Select(x => x * 2)],
			(from, to) => (!rooms.Contains(to) || !doors.Contains((to + from) / 2)) ? 0 : 1);

		var answer1 = distance.Values.Max();

		// part2
		var answer2 = distance.Count(x => x.Value >= 1000);

		return (answer1.ToString(), answer2.ToString());
	}

	static readonly Dictionary<char, (v2i doorsDir, v2i roomDir)> _dirs = new()
	{
		{ 'N', (v2i.Down, v2i.Down * 2) },
		{ 'S', (v2i.Up, v2i.Up * 2) },
		{ 'E', (v2i.Right, v2i.Right * 2) },
		{ 'W', (v2i.Left, v2i.Left * 2) },
	};

	public static void CreateMap(string regex, out HashSet<v2i> rooms, out HashSet<v2i> doors)
	{
		doors = [];
		rooms = [v2i.Zero];

		var stack = new Stack<v2i>();

		var currentRoom = v2i.Zero;
		foreach (var r in regex)
		{
			if (_dirs.TryGetValue(r, out (v2i doorsDir, v2i roomDir) value))
			{
				var (doorsDir, roomDir) = value;
				rooms.Add(currentRoom + roomDir);
				doors.Add(currentRoom + doorsDir);
				currentRoom += roomDir;
			}
			else if (r == '(')
				stack.Push(currentRoom);

			else if (r == '|')
				currentRoom = stack.Peek();

			else if (r == ')')
				currentRoom = stack.Pop();
		}
	}
}