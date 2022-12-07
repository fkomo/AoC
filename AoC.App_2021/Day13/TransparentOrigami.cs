using Ujeby.AoC.Common;

namespace Ujeby.AoC.App.Day13
{
	internal class TransparentOrigami : ProblemBase
	{
		protected override (string, string) SolveProblem(string[] input)
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
			var answer2 = GetCodeFromPaper(size, paper);

			return (answer1?.ToString(), answer2?.ToString());
		}

		private string GetCodeFromPaper(int[] size, bool[,] paper)
		{
			var result = string.Empty;
			for (var p = 0; p < size[0]; p += 5)
				foreach (var c in _codes)
				{
					var found = true;
					for (var y = 0; y < c.Value.GetLength(0) && found; y++)
						for (var x = 0; x < c.Value.GetLength(1) && found; x++)
						{
							if ((paper[y, p + x] && c.Value[y, x] == 0) ||
								(!paper[y, p + x] && c.Value[y, x] == 1))
								found = false;
						}

					if (found)
					{
						result += c.Key;
						break;
					}
				}

			if (result.Length != 8)
				DrawPaper(size[0], size[1], paper);

			return result;
		}

		private void DrawPaper(int width, int height, bool[,] paper)
		{
			Debug.Line();
			for (var y = 0; y < height; y++)
			{
				Debug.Text(string.Empty, indent: 6);
				for (var x = 0; x < width; x++)
					Debug.Text(paper[y, x] ? "█" : " ");

				Debug.Line();
			}
			Debug.Line();
		}

		private Dictionary<char, int[,]> _codes = new Dictionary<char, int[,]>
		{
			{ 'P', new int[,]
				{
					{ 1, 1, 1, 0 },
					{ 1, 0, 0, 1 },
					{ 1, 0, 0, 1 },
					{ 1, 1, 1, 0 },
					{ 1, 0, 0, 0 },
					{ 1, 0, 0, 0 }
				}
			},
			{ 'G', new int[,]
				{
					{ 0, 1, 1, 0 },
					{ 1, 0, 0, 1 },
					{ 1, 0, 0, 0 },
					{ 1, 0, 1, 1 },
					{ 1, 0, 0, 1 },
					{ 0, 1, 1, 1 }
				}
			},
			{ 'H', new int[,]
				{
					{ 1, 0, 0, 1 },
					{ 1, 0, 0, 1 },
					{ 1, 1, 1, 1 },
					{ 1, 0, 0, 1 },
					{ 1, 0, 0, 1 },
					{ 1, 0, 0, 1 }
				}
			},
			{ 'R', new int[,]
				{
					{ 1, 1, 1, 0 },
					{ 1, 0, 0, 1 },
					{ 1, 0, 0, 1 },
					{ 1, 1, 1, 0 },
					{ 1, 0, 1, 0 },
					{ 1, 0, 0, 1 }
				}
			},
			{ 'K', new int[,]
				{
					{ 1, 0, 0, 1 },
					{ 1, 0, 1, 0 },
					{ 1, 1, 0, 0 },
					{ 1, 0, 1, 0 },
					{ 1, 0, 1, 0 },
					{ 1, 0, 0, 1 }
				}
			},
			{ 'L', new int[,]
				{
					{ 1, 0, 0, 0 },
					{ 1, 0, 0, 0 },
					{ 1, 0, 0, 0 },
					{ 1, 0, 0, 0 },
					{ 1, 0, 0, 0 },
					{ 1, 1, 1, 1 }
				}
			},
		};
	}
}