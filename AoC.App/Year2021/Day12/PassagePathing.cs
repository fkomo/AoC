using Ujeby.AoC.Common;

namespace Ujeby.AoC.App.Year2021.Day12
{
	internal class PassagePathing : PuzzleBase
	{
		protected override (string, string) SolveProblem(string[] input)
		{
			// Item1 == true - big cave
			var caves = new List<(bool, List<int>)>
			{
				(true, new List<int>()), // start
				(true, new List<int>()), // end
			};
			var caveToId = new Dictionary<string, int>()
			{
				{ "start", 0 },
				{ "end", 1 },
			};

			foreach (var line in input)
			{
				var c = line.Split('-');
				if (!caveToId.TryGetValue(c[0], out int id1))
				{
					id1 = caves.Count;
					caveToId.Add(c[0], id1);
					caves.Add((!c[0].Any(c => char.IsLower(c)), new List<int>()));
				}

				if (!caveToId.TryGetValue(c[1], out int id2))
				{
					id2 = caves.Count;
					caveToId.Add(c[1], id2);
					caves.Add((!c[1].Any(c => char.IsLower(c)), new List<int>()));
				}

				if (id2 != 0)
					caves[id1].Item2.Add(id2);

				if (id1 != 0)
					caves[id2].Item2.Add(id1);
			}

			// part1
			long? answer1 = VisitCave_AllSmallOnce(caves, 0, Array.Empty<int>());

			// part2
			long? answer2 = VisitCave_OneSmallTwice(caves, 0, Array.Empty<int>());

			return (answer1?.ToString(), answer2?.ToString());
		}

		internal int VisitCave_AllSmallOnce(List<(bool, List<int>)> caves, int toVisit, int[] path)
		{
			var result = 0;

			foreach (var cave in caves[toVisit].Item2)
			{
				if (cave == 1)
				{
					// end
					result++;
					continue;
				}

				if (!caves[cave].Item1 && path.Contains(cave))
					// small cave already visited
					continue;

				// big or first time small
				result += VisitCave_AllSmallOnce(caves, cave, path.Concat(new int[] { cave }).ToArray());
			}

			return result;
		}

		internal int VisitCave_OneSmallTwice(List<(bool, List<int>)> caves, int toVisit, int[] path)
		{
			var result = 0;

			foreach (var cave in caves[toVisit].Item2)
			{
				if (cave == 1)
				{
					// end
					result++;
					continue;
				}

				if (!caves[cave].Item1)
				{
					var visits = path.Count(c => c == cave);
					if (visits == 2)
						// small already visited twice
						continue;

					if (visits == 1)
					{
						var smallCaves = path.Where(p => !caves[p].Item1);
						if (smallCaves.Distinct().Count() != smallCaves.Count())
							// small already visited once, but other small visited twice
							continue;
					}
				}

				result += VisitCave_OneSmallTwice(caves, cave, path.Concat(new int[] { cave }).ToArray());
			}

			return result;
		}
	}
}
