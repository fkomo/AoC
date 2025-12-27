using Ujeby.AoC.Common;

namespace Ujeby.AoC.App._2025_11;

[AoCPuzzle(Year = 2025, Day = 11, Answer1 = "668", Answer2 = null, Skip = false)]
public class Reactor : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var devices = input.ToDictionary(x => x[..3], x => x[5..].Split(' '));

		// part1
		var answer1 = devices.AllPaths("you", "out").Count;

		// part2
		//var svr_dac = devices.AllPaths("svr", "dac"); // too long
		//var dac_fft = devices.AllPaths("dac", "fft"); // 0
		//var fft_out = devices.AllPaths("fft", "out"); // too long
		//var svr_fft = devices.AllPaths("svr", "fft"); // too long
		//var fft_dac = devices.AllPaths("fft", "dac"); // too long

		//var dac_out = devices.AllPaths("dac", "out"); // 25644

		// svr|...|fft|...|dac|...|out paths
		//var answer2 = devices.AllPaths("svr", "fft", "dac").Count;
		var answer2 = 0;

        return (answer1.ToString(), answer2.ToString());
	}
}

static class Extensions
{
	public static HashSet<string[]> AllPaths(this Dictionary<string, string[]> devices, params string[] nodes)
	{
		var paths = new HashSet<string[]>();

		//var queue = new Queue<List<string>>();
		var stack = new Stack<List<string>>();

		//queue.Enqueue([nodes[0]]);
		stack.Push([nodes[0]]);

		//while (queue.Count > 0)
		while (stack.Count > 0)
		{
			var path = stack.Pop();
			//var path = queue.Dequeue();

			while (true)
			{
				var last = path.Last();

				if (path.Count > 1 && nodes.Length > 2 && path.Contains(nodes[2]) && !path.Contains(nodes[1]))
					break;
	
				if (last == nodes[^1])
				{
					if (nodes.Any(x => !path.Contains(x)))
						break;

					paths.Add([.. path]);
					break;
				}

				// maybe not needed check
				if (!devices.ContainsKey(last))
					break;

				if (devices[last].Length == 1 && !path.Contains(devices[last][0]))
				{
					path.Add(devices[last][0]);
					continue;
				}

				foreach (var next in devices[last].Where(x => !path.Contains(x)))
					//queue.Enqueue([.. path, next]);
					stack.Push([.. path, next]);

				break;
			}
		}

		return paths;
	}
}