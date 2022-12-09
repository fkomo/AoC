using System.Diagnostics.CodeAnalysis;
using Ujeby.AoC.Common;

namespace Ujeby.AoC.App.Year2022.Day09
{
	internal class RopeBridge : ProblemBase
	{
		protected override (string, string) SolveProblem(string[] input)
		{
			// part1
			var h = new int[] { 0, 0 };
			var t = new int[] { 0, 0 };
			var dist = new int[] { 0, 0 };
			var visited = new List<int[]>();
			visited.Add(t.ToArray());

			foreach (var mov in input)
			{
				var dir = _dir[mov[0]];
				var len = mov[2] - '0';

				for (var i = 0; i < len; i++)
				{
					h[0] += dir[0];
					h[1] += dir[1];

					dist[0] = h[0] - t[0];
					dist[1] = h[1] - t[1];

					if (Math.Abs(dist[0]) <= 1 && Math.Abs(dist[1]) <= 1)
						continue;

					if (dist[0] == 0)
						t[1] += dir[1];
					else if (dist[1] == 0)
						t[0] += dir[0];
					else
					{
						t[0] += dist[0] / Math.Abs(dist[0]);
						t[1] += dist[1] / Math.Abs(dist[1]);
					}

					if (!visited.Any(v => v[0] == t[0] && v[1] == t[1]))
						visited.Add(t.ToArray());
				}
			}
			long? answer1 = visited.Count;

			// part2
			long? answer2 = null;

			return (answer1?.ToString(), answer2?.ToString());
		}

		internal class Int2EqComparer : IEqualityComparer<int[]>
		{
			public bool Equals(int[] x, int[] y)
			{
				if (x.Length != y.Length)
					return false;

				for (var i = 0; i < x.Length; i++)
					if (x[i] != y[i])
						return false;

				return true;
			}

			public int GetHashCode([DisallowNull] int[] obj) => obj.GetHashCode();
		}

		private Dictionary<char, int[]> _dir = new()
		{
			{ 'L', new[] { -1, 0 } }, { 'R', new[] { 1, 0 } }, { 'D', new[] { 0, -1 } }, { 'U', new[] { 0, 1 } },
		};
	}
}
