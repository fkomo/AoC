using Ujeby.AoC.Common;

namespace Ujeby.AoC.App.Day09
{
	internal class RopeBridge : ProblemBase
	{
		protected override (string, string) SolveProblem(string[] input)
		{
			// part1
			var h = new int[] { 0, 0 };
			var t = new int[] { 0, 0 };
			var visited = new List<int[]>();
			visited.Add(t);

			foreach (var mov in input)
			{
				var dir = _dir[mov[0]];
				var len = mov[2] - '0';

				for (var i = 0; i < len; i++)
				{
					h[0] += dir[0];
					h[1] += dir[1];

					if (Math.Abs(t[0] - h[0]) <= 1 && Math.Abs(t[1] - h[1]) <= 1)
						continue;





					if (t[0] == h[0])

					visited.Add(t);
				}
			}
			long? answer1 = null;

			// part2
			long? answer2 = null;

			return (answer1?.ToString(), answer2?.ToString());
		}

		private Dictionary<char, int[]> _dir = new()
		{
			{ 'L', new[] { -1, 0 } },
			{ 'R', new[] { 1, 0 } },
			{ 'D', new[] { 0, -1 } },
			{ 'U', new[] { 0, 1 } },
		};
	}
}
