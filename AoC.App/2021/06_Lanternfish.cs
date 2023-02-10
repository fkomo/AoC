using Ujeby.AoC.Common;

namespace Ujeby.AoC.App._2021_06
{
	internal class Lanternfish : PuzzleBase
	{
		protected override (string, string) SolvePuzzle(string[] input)
		{
			var inputB = input.First().Split(',').Select(s => byte.Parse(s)).ToArray();

			// part1
			var fish = new List<byte>(inputB);
			for (var day = 1; day <= 80; day++)
			{
				for (int f = fish.Count - 1; f >= 0; f--)
				{
					if (fish[f] == 0)
					{
						fish.Add(8);
						fish[f] = 6;
					}
					else
						fish[f]--;
				}
			}
			long answer1 = fish.Count;

			// part2
			var fishAge = new long[9];
			foreach (var f in fish)
				fishAge[f]++;

			for (var day = 81; day <= 256; day++)
			{
				var newFish = fishAge[0];

				for (var i = 0; i < fishAge.Length - 1; i++)
					fishAge[i] = fishAge[i + 1];

				fishAge[6] += newFish;
				fishAge[8] = newFish;
			}

			long answer2 = fishAge.Sum();

			return (answer1.ToString(), answer2.ToString());
		}
	}
}
