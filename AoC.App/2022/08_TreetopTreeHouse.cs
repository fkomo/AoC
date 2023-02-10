using Ujeby.AoC.Common;

namespace Ujeby.AoC.App._2022_08
{
	internal class TreetopTreeHouse : PuzzleBase
	{
		protected override (string, string) SolvePuzzle(string[] input)
		{
			var trees = input.Select(r => r.Select(c => c - '0').ToArray()).ToArray();
			var size = trees.Length;

			// 1 left
			// 2 right
			// 4 top/front
			// 8 bottom/back
			var visibility = input.Select(r => r.Select(c => 0).ToArray()).ToArray();

			// part1
			var highestF = trees.First().ToArray();
			var highestB = trees.Last().ToArray();
			for (var y = 1; y < size - 1; y++)
			{
				var highestL = trees[y].First();
				var highestR = trees[y].Last();

				for (var x = 1; x < size - 1; x++)
				{
					if (trees[y][x] > highestL)
					{
						visibility[y][x] |= 1;
						highestL = trees[y][x];
					}

					if (trees[y][size - 1 - x] > highestR)
					{
						visibility[y][size - 1 - x] |= 2;
						highestR = trees[y][size - 1 - x];
					}

					if (trees[y][x] > highestF[x])
					{
						visibility[y][x] |= 4;
						highestF[x] = trees[y][x];
					}

					if (trees[size - 1 - y][x] > highestB[x])
					{
						visibility[size - 1 - y][x] |= 8;
						highestB[x] = trees[size - 1 - y][x];
					}
				}
			}

			//Debug.Line();
			//Debug.Line("visibility map:", indent: 6);
			//for (var y = 0; y < trees.Length; y++)
			//{
			//	Debug.Text(null, indent: 6);
			//	for (var x = 0; x < trees[y].Length; x++)
			//		Debug.Text($"{visibility[y][x],2}");
			//	Debug.Line();
			//}
			//Debug.Line();

			long? answer1 = 2 * (trees.Length + trees[0].Length - 2) + visibility.Sum(r => r.Count(v => v > 0));

			// part2
			//Debug.Line("scenic score map (inner):", indent: 6);
			long? answer2 = long.MinValue;
			for (var y = 1; y < size - 1; y++)
			{
				//Debug.Text(null, indent: 6);
				for (var x = 1; x < size - 1; x++)
				{
					var i = 0;
					var score = 1;

					for (i = 1; x + i < size - 1 && trees[y][x + i] < trees[y][x]; i++)
					{
					}
					score *= i;

					for (i = 1; y + i < size - 1 && trees[y + i][x] < trees[y][x]; i++)
					{
					}
					score *= i;

					for (i = 1; x - i > 0 && trees[y][x - i] < trees[y][x]; i++)
					{
					}
					score *= i;

					for (i = 1; y - i > 0 && trees[y - i][x] < trees[y][x]; i++)
					{
					}
					score *= i;

					//Debug.Text($"{score,2}");
					if (score > answer2)
						answer2 = score;
				}
				//Debug.Line();
			}
			//Debug.Line();

			return (answer1?.ToString(), answer2?.ToString());
		}
	}
}
