using Ujeby.AoC.Common;

namespace Ujeby.AoC.App._2021_13
{
    internal class TransparentOrigami : PuzzleBase
	{
		protected override (string, string) SolvePuzzle(string[] input)
		{
			var points = new List<int[]>();
			var i = 0;
			for (; input[i].Length != 0; i++)
			{
				var split = input[i].Split(',');
				points.Add(new[] { int.Parse(split[0]), int.Parse(split[1]) });
			}

			// part1
			var instr = input[i + 1];
			var fold = int.Parse(instr["fold along x=".Length..]);
			var axis = (instr["fold along ".Length] == 'x') ? 0 : 1;
			long? answer1 = points.Count(
				p => (p[axis] < fold) || !points.Any(lp => lp[axis] == fold - (p[axis] - fold) && lp[(axis + 1) % 2] == p[(axis + 1) % 2]));

			// part2
			var size = new[] { points.Max(p => p[0]) + 1, points.Max(p => p[1]) + 1 };
			var paper = new bool[size[1], size[0]];
			foreach (var p in points)
				paper[p[1], p[0]] = true;

			for (i++; i < input.Length; i++)
			{
				instr = input[i];
				fold = int.Parse(instr["fold along x=".Length..]);
				axis = (instr["fold along ".Length] == 'x') ? 0 : 1;

				for (var p = points.Count - 1; p >= 0; p--)
				{
					if (points[p][axis] < fold)
						continue;

					var folded = points[p].ToArray();
					folded[axis] = fold - (points[p][axis] - fold);

					paper[points[p][1], points[p][0]] = false;

					if (paper[folded[1], folded[0]])
						points.RemoveAt(p);
					else
					{
						points[p] = folded.ToArray();
						paper[folded[1], folded[0]] = true;
					}
				}
			}
			size = new[] { points.Max(p => p[0]) + 1, points.Max(p => p[1]) + 1 };

			var answer2 = CharCodes.ToString(size[0], size[1], paper);

			return (answer1?.ToString(), answer2?.ToString());
		}
	}
}