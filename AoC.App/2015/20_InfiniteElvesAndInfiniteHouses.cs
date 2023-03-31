using Ujeby.AoC.Common;

namespace Ujeby.AoC.App._2015_20
{
	[AoCPuzzle(Year = 2015, Day = 20, Answer1 = "786240", Answer2 = "831600", Skip = true)]
	public class InfiniteElvesAndInfiniteHouses : PuzzleBase
	{
		protected override (string Part1, string Part2) SolvePuzzle(string[] input)
		{
			var desiredPresents = long.Parse(input.Single());

			// part1
			// TODO 2015/20 OPTIMIZE p1 (10min)
			long? answer1 = null;
			var lastHouse = (desiredPresents - 10) / 10;
			answer1 = Parallel.For(2, lastHouse, (h, state) =>
			{
				if (state.ShouldExitCurrentIteration && state.LowestBreakIteration < h)
					return;

				var presents = 10 * (1 + h);
				for (var i = h - 1; i > 1; i--)
				{
					if (h % i != 0)
						continue;

					presents += 10 * i;
					if (presents >= desiredPresents)
						state.Break();
				}
			}).LowestBreakIteration;

			// part2
			long? answer2 = long.MaxValue;
			var houses = new Dictionary<long, long>();
			for (int e = 1, p = 11; e < desiredPresents && e <= answer2; e++, p += 11)
				for (int h = e, i = 1; i <= 50 && h < answer2; i++, h += e)
				{
					if (!houses.ContainsKey(h))
						houses.Add(h, p);
					else
					{
						houses[h] += p;
						if (houses[h] >= desiredPresents && h < answer2.Value)
							answer2 = h;
					}
				}

			return (answer1?.ToString(), answer2?.ToString());
		}
	}
}
