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
		var answer1 = LongestHike(input);

		// part2
		var answer2 = LongestHike(input, ignoreSlopes: true);

		return (answer1.ToString(), answer2.ToString());
	}

	public static long LongestHike(string[] map, 
		bool ignoreSlopes = false, List<v2i[]> allPaths = null)
	{
		var start = new v2i(1, 0);
		var end = new v2i(map.Length - 2, map.Length - 1);

		var longest = long.MinValue;

		var paths = new Queue<(v2i[] Path, v2i NextDir)>();
		paths.Enqueue((new v2i[] { new v2i(1, 0) }, v2i.Up));
		while (paths.Count > 0)
		{
			var pq = paths.Dequeue();
			Debug.Line($"{paths.Count} remaining ...");

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

						if (ignoreSlopes)
						{
							// cant step on same tile twice
							if (steps.Contains(n))
								return false;
						}
						else
						{
							// cant go against slope
							if (_slopes.ContainsKey(t) && d != _slopes[t])
								return false;
						}

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
				if (nextSteps.Length == 1 && last + dir.Value == end)
				{
					steps.Add(last + dir.Value);
					allPaths?.Add(steps.ToArray());

					var totalSteps = steps.Count - 1; // ignore start
					Log.Line($"{totalSteps}");

					if (totalSteps > longest)
						longest = totalSteps;

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

		return longest;
	}
}