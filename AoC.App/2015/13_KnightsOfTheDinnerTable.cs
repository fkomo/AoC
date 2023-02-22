using Ujeby.AoC.Common;
using Ujeby.Tools.StringExtensions;

namespace Ujeby.AoC.App._2015_13
{
	[AoCPuzzle(Year = 2015, Day = 13, Answer1 = "709", Answer2 = "668")]
	public class KnightsOfTheDinnerTable : PuzzleBase
	{
		public record struct Neighbour(string Name, int Happiness);

		protected override (string Part1, string Part2) SolvePuzzle(string[] input)
		{
			var guests = input.GroupBy(i => i.Split(' ').First())
				.ToDictionary(
					i => i.Key, 
					i => i.ToDictionary(
						y => y[(y.LastIndexOf(' ') + 1)..^1], 
						y => y.ToNumArray()[0] * (y.Contains("gain") ? 1 : -1)));

			// part1
			var seats = Alg.Combinatorics.Permutations(guests.Keys, guests.Count);
			var answer1 = seats.Max(s => TableHappiness(s.ToArray(), guests));

			// part2
			seats = Alg.Combinatorics.Permutations(new string[] { "@me" }.Concat(guests.Keys).ToArray(), guests.Count + 1);
			var answer2 = seats.Max(s => TableHappiness(s.ToArray(), guests));

			return (answer1.ToString(), answer2.ToString());
		}

		private static long TableHappiness(string[] seats, Dictionary<string, Dictionary<string, long>> happMap)
		{
			var happiness = 0L;
			for (var i = 0; i < seats.Length; i++)
			{
				var current = seats[i];
				if (current == "@me")
					continue;

				var next = seats[(i + 1) % seats.Length];
				if (next != "@me")
					happiness += happMap[current][next];

				var prev = seats[(i - 1 + seats.Length) % seats.Length];
				if (prev != "@me")
					happiness += happMap[current][prev];
			}

			return happiness;
		}
	}
}
