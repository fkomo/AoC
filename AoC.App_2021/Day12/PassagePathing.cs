using Ujeby.AoC.Common;

namespace Ujeby.AoC.App.Day12
{
	internal class PassagePathing : ProblemBase
	{
		protected override (string, string) SolveProblem(string[] input)
		{
			var caves = new Dictionary<string, List<string>>();
			foreach (var line in input)
			{
				var c = line.Split('-');
				if (!caves.ContainsKey(c[0]))
					caves.Add(c[0], new List<string>());

				if (c[1] != "start")
					caves[c[0]].Add(c[1]);

				if (!caves.ContainsKey(c[1]))
					caves.Add(c[1], new List<string>());

				if (c[0] != "start")
					caves[c[1]].Add(c[0]);
			}

			// part1
			long? answer1 = VisitCave_AllSmallOnce(caves, "start", string.Empty);

			// part2
			// TODO 2021/11 optimize [<250ms]
			long? answer2 = VisitCave_OneSmallTwice(caves, "start", string.Empty);

			return (answer1.ToString(), answer2.ToString());
		}

		internal int VisitCave_AllSmallOnce(Dictionary<string, List<string>> caves, string toVisit, string path)
		{
			var result = 0;

			foreach (var cave in caves[toVisit])
			{
				if (cave == "end")
				{
					//DebugLine("start" + path + ",end");
					result++;
					continue;
				}

				var visited = path.Split(',');
				if (!cave.Any(c => char.IsUpper(c)) && visited.Contains(cave))
					continue;

				result += VisitCave_AllSmallOnce(caves, cave, path + "," + cave);
			}

			return result;
		}

		internal int VisitCave_OneSmallTwice(Dictionary<string, List<string>> caves, string toVisit, string path)
		{
			var result = 0;

			foreach (var cave in caves[toVisit])
			{
				if (cave == "end")
				{
					//DebugLine("start" + path + ",end");
					result++;
					continue;
				}

				if (!cave.Any(c => char.IsUpper(c)))
				{
					var visited = path.Split(',');
					if (visited.Count(c => c == cave) == 2)
						continue;

					if (visited.Contains(cave) && 
						visited
							.Where(cave => !cave.Any(c => char.IsUpper(c)))
							.GroupBy(c => c)
							.Any(g => g.Count() == 2))
						continue;
				}

				result += VisitCave_OneSmallTwice(caves, cave, path + "," + cave);
			}

			return result;
		}
	}
}
