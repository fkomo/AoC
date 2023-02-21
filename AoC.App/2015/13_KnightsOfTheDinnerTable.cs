using Ujeby.AoC.Common;

namespace Ujeby.AoC.App._2015_13
{
	[AoCPuzzle(Year = 2015, Day = 13, Answer1 = "709", Answer2 = "668")]
	public class KnightsOfTheDinnerTable : PuzzleBase
	{
		public record struct Neighbour(string Name, int Happiness);

		protected override (string Part1, string Part2) SolvePuzzle(string[] input)
		{
			var guests = input.GroupBy(i => i.Split(' ').First())
				.ToDictionary(i => i.Key, i => i.Select(n =>
				{
					var s = n.Split(' ');
					return new Neighbour(s.Last()[..^1], int.Parse(s[3]) * (s[2] == "gain" ? 1 : -1));
				}).ToArray());

			// part1
			var seats = Alg.Combinatorics.Permutations(guests.Keys, guests.Count);
			var answer1 = seats.Max(s => TableHappiness(s.ToArray(), guests));

			// part2
			seats = Alg.Combinatorics.Permutations(new string[] { "@me" }.Concat(guests.Keys).ToArray(), guests.Count + 1);
			var answer2 = seats.Max(s => TableHappiness(s.ToArray(), guests));

			return (answer1.ToString(), answer2.ToString());
		}

		private static int TableHappiness(string[] seats, Dictionary<string, Neighbour[]> guests)
		{
			var happiness = 0;

			var first = seats.First();
			var last = seats.Last();
			if (first != "@me" && last != "@me")
				happiness += guests[first].Single(n => n.Name == last).Happiness;

			for (var i = 0; i < seats.Length; i++)
			{
				var current = seats[i];
				if (current == "@me")
					continue;

				var next = seats[(i + 1) % seats.Length];
				if (next != "@me")
					happiness += guests[current].Single(n => n.Name == next).Happiness;

				if (i > 0)
				{
					var prev = seats[i - 1];
					if (prev != "@me")
						happiness += guests[current].Single(n => n.Name == prev).Happiness;
				}
			}

			return happiness;
		}
	}
}
