using Ujeby.AoC.App.Year2022.Day09;
using Ujeby.AoC.Common;

namespace Ujeby.AoC.App.Year2021.Day15
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

			//Debug.Line();
			//for (var y = 0; y < risk.GetLength(0); y++)
			//{
			//	Debug.Text(null, indent: 6);
			//	for (var x = 0; x < risk.GetLength(0); x++)
			//		Debug.Text(risk[y, x].ToString());
			//	Debug.Line();
			//}
			//Debug.Line();

			// part1
			long? answer1 = LowestRiskPath(risk);

			// part2
			var risk5 = new int[size * 5, size * 5];
			for (var y = 0; y < risk5.GetLength(0); y++)
				for (var x = 0; x < risk5.GetLength(0); x++)
				{
					if (y < size && x < size)
					{
						risk5[y, x] = risk[y, x];
						continue;
					}

					var x0 = x - size;
					if (x0 < 0)
						x0 = x;

					var y0 = y - size;
					if (y0 < 0)
						y0 = y;
					var r = risk5[y0, x0];

					if (x >= size)
						r++;
					if (y >= size)
						r++;

					if (r >= 10)
						r -= 9;

					risk5[y, x] = r;
				}

			//Debug.Line();
			//for (var y = 0; y < risk5.GetLength(0); y++)
			//{
			//	Debug.Text(null, indent: 6);
			//	for (var x = 0; x < risk5.GetLength(0); x++)
			//		Debug.Text(risk5[y, x].ToString());
			//	Debug.Line();
			//}
			//Debug.Line();

			// TODO 2021/15 part2 still wrong (3019) although answer for sample is correct
			long? answer2 = LowestRiskPath(risk5);

			return (answer1?.ToString(), answer2?.ToString());
		}

		private long? LowestRiskPath(int[,] risk)
		{
			var size = risk.GetLength(0);

			var path = new int[size, size];
			for (var y = 0; y < size; y++)
				for (var x = 0; x < size; x++)
					path[y, x] = int.MaxValue;

			path[0, 0] = 0;
			for (var y = 0; y < size; y++)
				for (var x = 0; x < size; x++)
					Visit(risk, path, x, y);

			//Debug.Line();
			//for (var y = 0; y < path.GetLength(0); y++)
			//{
			//	Debug.Text(null, indent: 6);
			//	for (var x = 0; x < path.GetLength(0); x++)
			//		Debug.Text($"{path[y, x],4}");
			//	Debug.Line();
			//}
			//Debug.Line();

			return path[size - 1, size - 1];
		}

		private void Visit(int[,] risk, int[,] path, int x, int y)
		{
			var size = risk.GetLength(0);
			foreach (var dir in Directions.NSWE.Values)
			{
				var x1 = dir[0] + x;
				var y1 = dir[1] + y;
				if (x1 < 0 || y1 < 0 || x1 == size || y1 == size)
					continue;

				var r = path[y, x] + risk[y1, x1];
				if (r < path[y1, x1])
					path[y1, x1] = r;
			}
		}
	}
}
