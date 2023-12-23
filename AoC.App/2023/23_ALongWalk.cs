using Ujeby.AoC.Common;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2023_23;

[AoCPuzzle(Year = 2023, Day = 23, Answer1 = "2106", Answer2 = null, Skip = false)]
public class ALongWalk : PuzzleBase
{
	readonly static Dictionary<char, v2i> _slopes = new()
	{
		{ '>', v2i.Right },
		{ 'v', v2i.Up },
	};

	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		Debug.Line($"map size: {input.Length}x{input.Length}");

		// part1
		var allPaths = AllPaths(input);
		Debug.Line($"{allPaths.Length} possible paths");
		var answer1 = allPaths.Max(x => x.Length) - 1;

		// part2
		string answer2 = null;

		return (answer1.ToString(), answer2?.ToString());
	}

	public static v2i[][] AllPaths(string[] map)
	{
		var start = new v2i(1, 0);
		var end = new v2i(map.Length - 2, map.Length - 1);

		var allPaths = new List<v2i[]>();

		var paths = new Queue<(v2i[] Path, v2i NextDir)>();
		paths.Enqueue((new v2i[] { new v2i(1, 0) }, v2i.Up));
		while (paths.Count > 0)
		{
			var pq = paths.Dequeue();
			var steps = new List<v2i>(pq.Path);
			v2i? dir = pq.NextDir;

			while (dir.HasValue)
			{
				// add step
				var last = steps.Last() + dir.Value;
				steps.Add(last);

				// next steps
				var nextSteps = v2i.UpDownLeftRight
					.Where(d =>
					{
						var n = last + d;
						var t = map[n.Y][(int)n.X];

						// forrest
						if (t == '#')
							return false;

						// cant go against slope
						if (_slopes.ContainsKey(t) && d != _slopes[t])
							return false;

						// cant go back
						if (d * -1 == dir.Value)
							return false;

						// clear path ahead
						return true;
					})
					.ToArray();

				// nowhere to go
				if (nextSteps.Length == 0)
					break;

				dir = nextSteps[0];
				if (nextSteps.Length == 1 && last + dir == end)
				{
					steps.Add(last + dir.Value);
					allPaths.Add(steps.ToArray());

					var totalSteps = steps.Count - 1; // ignore start
					Debug.Line($"end after {totalSteps}");
					break;
				}
				else
				{
					// split path
					foreach (var s in nextSteps.Skip(1))
						paths.Enqueue((steps.ToArray(), s));
				}
			}
		}

		return allPaths.ToArray();
	}
}