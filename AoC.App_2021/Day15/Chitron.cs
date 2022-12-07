using Ujeby.AoC.Common;

namespace Ujeby.AoC.App.Day15
{
	internal class Chitron : ProblemBase
	{
		protected override (string, string) SolveProblem(string[] input)
		{
			var size = input.Length;
	
			var risk = new int[size, size];
			for (var y = 0; y < size; y++)
				for (var x = 0; x < size; x++)
					risk[y, x] = input[y][x] - '0';

			// part1
			long? answer1 = LowestRiskPath(risk, size, 1);

			// part2
			long? answer2 = LowestRiskPath(risk, size, 5);

			return (answer1?.ToString(), answer2?.ToString());
		}

		private long? LowestRiskPath(int[,] risk, int size, int scale)
		{
			size *= scale;
			var path = new int[size, size];
			for (var y = 0; y < size; y++)
				for (var x = 0; x < size; x++)
					path[y, x] = int.MaxValue;

			path[0, 0] = 0;
			for (var y = 0; y < size; y++)
				for (var x = 0; x < size; x++)
					Visit(risk, path, x, y);

			return path[size - 1, size - 1];
		}

		private (int, int)[] _dir = new (int, int)[] { ( -1, 0 ), ( 1, 0 ), ( 0, -1 ), ( 0, 1 ) };

		private void Visit(int[,] risk, int[,] path, int x, int y)
		{
			var riskSize = risk.GetLength(0);
			var size = path.GetLength(0);
			foreach (var dir in _dir)
			{
				var x1 = dir.Item1 + x;
				var y1 = dir.Item2 + y;
				if (x1 < 0 || y1 < 0 || x1 == size || y1 == size)
					continue;

				var r = path[y, x] + risk[y1 % riskSize, x1 % riskSize];
				// TODO 2021/15 part2 fix bug with 0
				r += ((y1 / riskSize) + (x1 / riskSize)) % 10;
				if (r < path[y1, x1])
					path[y1, x1] = r;
			}

			//Debug.Line();
			//for (var py = 0; py < size; py++)
			//{
			//	for (var px = 0; px < size; px++)
			//	{
			//		Debug.Text($"{path[py, px],2} ");
			//	}
			//	Debug.Line();
			//}
		}
	}
}
