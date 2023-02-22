using Ujeby.AoC.Common;

namespace Ujeby.AoC.App._2015_09
{
	[AoCPuzzle(Year = 2015, Day = 09, Answer1 = "251", Answer2 = "898")]
	public class AllInASingleNight : PuzzleBase
	{
		protected override (string Part1, string Part2) SolvePuzzle(string[] input)
		{
			var places = input.SelectMany(i => i[..i.IndexOf(" = ")].Split(" to ")).Distinct().ToArray();

			var dist = new int[places.Length, places.Length];
			foreach (var i in input)
			{
				var s = i.Split(" = ");
				var p = s[0].Split(" to ").ToArray();

				var ip0 = Array.IndexOf(places, p[0]);
				var ip1 = Array.IndexOf(places, p[1]);
				var d = int.Parse(s[1]);

				dist[ip0, ip1] = d;
				dist[ip1, ip0] = d;
			}

			// part1
			var answer1 = int.MaxValue;
			// part2
			var answer2 = int.MinValue;

			var allRoutes = Alg.Combinatorics.Permutations(Enumerable.Range(0, places.Length), places.Length);
			foreach (var r in allRoutes)
			{
				var routeLength = 0;
				var route = r.ToArray();
				for (var i = 1; i < route.Length; i++)
					routeLength += dist[route[i - 1], route[i]];

				if (routeLength < answer1)
					answer1 = routeLength;

				if (routeLength > answer2)
					answer2 = routeLength;
			}

			return (answer1.ToString(), answer2.ToString());
		}
	}
}