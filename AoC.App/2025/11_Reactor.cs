using Ujeby.AoC.Common;

namespace Ujeby.AoC.App._2025_11;

[AoCPuzzle(Year = 2025, Day = 11, Answer1 = "668", Answer2 = null, Skip = false)]
public class Reactor : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var devices = input.ToDictionary(x => x[..3], x => x[5..].Split(' '));

		// part1
		var paths = new HashSet<string>();
		var queue = new Queue<List<string>>();
		
		queue.Enqueue(["you"]);

		while (queue.Count > 0)
		{
			var path = queue.Dequeue();

			while (true)
			{
				var last = path.Last();
				if (last == "out")
				{
					var p = string.Join('|', path);
					Debug.Line(p);

					paths.Add(p);
					break;
				}	

				if (!devices.ContainsKey(last))
					break;

				if (devices[last].Length == 1)
				{
					path.Add(devices[last][0]);
					continue;
				}

				foreach (var next in devices[last])
					queue.Enqueue([.. path, next]);

				break;
			}
		}

		var answer1 = paths.Count;

		// part2
		string answer2 = null;

		return (answer1.ToString(), answer2?.ToString());
	}
}