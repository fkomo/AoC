using Ujeby.AoC.Common;

namespace Ujeby.AoC.App.Day06
{
	internal class Lanternfish : ProblemBase
	{
		protected override (string, string) SolveProblem(string[] input)
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
			long result1 = fish.Count;

			// part2

			#region naive approach (sample needs ~25Gb mem and a LOT! of cpu time)

			//long _maxArraySize = 1024 * 1024 * 1024;
			//byte NO_FISH = 0xff;

			//var fishGroup = new List<byte[]>();
			//fishGroup.Add(fish.ToArray());
			//var nextFish = -1;
			//var newGroup = -1;
			//for (var day = 81; day <= 256; day++)
			//{
			//	var lastGroup = fishGroup.Count - 1;
			//	var lastFish = nextFish - 1;
			//	for (var fg = lastGroup; fg >= 0; --fg)
			//	{
			//		var f0 = fishGroup[fg].Length - 1;
			//		if (fg > 0 && fg == lastGroup && lastFish > -1)
			//			f0 = lastFish;
			//		for (var f = f0; f >= 0; --f)
			//		{
			//			if (fishGroup[fg][f] != 0)
			//			{
			//				fishGroup[fg][f]--;
			//				continue;
			//			}
			//			if (nextFish == _maxArraySize || nextFish == -1)
			//			{
			//				var newFish = new byte[_maxArraySize];
			//				System.Runtime.CompilerServices.Unsafe.InitBlock(ref newFish[0], NO_FISH, (uint)newFish.Length);
			//				fishGroup.Add(newFish);
			//				newGroup = fishGroup.Count - 1;
			//				nextFish = 0;
			//			}
			//			fishGroup[fg][f] = 6;
			//			fishGroup[newGroup][nextFish] = 8;
			//			nextFish++;
			//		}
			//	}
			//	DebugLine($"day { day }, { fishGroup.Count } fish groups, { nextFish } fish in last group");
			//}
			//long result2 = fishGroup.First().Length + (fishGroup.Count - 2) * _maxArraySize + nextFish;

			#endregion

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

			long result2 = fishAge.Sum();

			return (result1.ToString(), result2.ToString());
		}
	}
}
