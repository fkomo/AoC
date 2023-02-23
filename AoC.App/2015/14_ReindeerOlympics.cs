using Ujeby.AoC.Common;
using Ujeby.Tools.StringExtensions;

namespace Ujeby.AoC.App._2015_14
{
	[AoCPuzzle(Year = 2015, Day = 14, Answer1 = "2640", Answer2 = "1102")]
	public class ReindeerOlympics : PuzzleBase
	{
		internal class RacingReindeer
		{
			public int Points;
			public long Distance;
			public long Flying;
			public long Resting;

			public override string ToString()
				=> $"{Distance}km with {Points}* Flying={Flying} Resting={Resting}";
		}

		protected override (string Part1, string Part2) SolvePuzzle(string[] input)
		{
			var timeLimit = 2503;
#if _DEBUG_SAMPLE
			timeLimit = 1000;
#endif
			var reindeers = input.ToDictionary(i => i[..i.IndexOf(' ')], i => i.ToNumArray());

			// part1
			var answer1 = reindeers.Values.Max(r => r[0] *
				((timeLimit / (r[1] + r[2]) * r[1]) + Math.Min(r[1], timeLimit - (timeLimit / (r[1] + r[2]) * (r[1] + r[2])))));

			// part2
			var race = reindeers.ToDictionary(i => i.Key, i => new RacingReindeer { Flying = i.Value[1] });
			for (var t = 1; t <= timeLimit; t++)
			{
				foreach (var reindeer in reindeers)
				{
					// flying
					if (race[reindeer.Key].Flying > 0)
					{
						race[reindeer.Key].Flying--;
						race[reindeer.Key].Distance += reindeer.Value[0];

						if (race[reindeer.Key].Flying == 0)
							race[reindeer.Key].Resting = reindeer.Value[2];
					}
					// resting
					else if (race[reindeer.Key].Resting > 0)
					{
						race[reindeer.Key].Resting--;

						if (race[reindeer.Key].Resting == 0)
							race[reindeer.Key].Flying = reindeer.Value[1];
					}
				}

				var firstPosition = race.Max(i => i.Value.Distance);
				foreach (var reindeer in reindeers)
					if (race[reindeer.Key].Distance == firstPosition)
						race[reindeer.Key].Points++;
			}
			var answer2 = race.Max(r => r.Value.Points);

			return (answer1.ToString(), answer2.ToString());
		}
	}
}
