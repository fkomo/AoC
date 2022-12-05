using System.Xml;
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
				caves[c[0]].Add(c[1]);

				if (!caves.ContainsKey(c[1]))
					caves.Add(c[1], new List<string>());
				caves[c[1]].Add(c[0]);
			}

			// part1
			var paths = new List<string>();
			VisitCave_AllSmallOnce(caves, "start", string.Empty, paths);
			long result1 = paths.Count();

			// part2
			paths = new List<string>();
			VisitCave_OneSmallTwice(caves, "start", string.Empty, paths);
			long result2 = paths.Count();

			return (result1.ToString(), result2.ToString());
		}

		internal void VisitCave_AllSmallOnce(Dictionary<string, List<string>> caves, string toVisit, string path, List<string> paths)
		{
			var visited = path.Split(',', StringSplitOptions.RemoveEmptyEntries);
			foreach (var cave in caves[toVisit])
			{
				if (cave == "start")
					continue;

				if (cave == "end")
				{
					paths.Add(path + ",end");
					continue;
				}

				// small cave
				if (!cave.Any(c => char.IsUpper(c)) && visited.Contains(cave))
					continue;

				// big cave
				VisitCave_AllSmallOnce(caves, cave, path + $",{cave}", paths);
			}	
		}

		internal void VisitCave_OneSmallTwice(Dictionary<string, List<string>> caves, string toVisit, string path, List<string> paths)
		{
			var visited = path.Split(',', StringSplitOptions.RemoveEmptyEntries);
			foreach (var cave in caves[toVisit])
			{
				if (cave == "start")
					continue;

				if (cave == "end")
				{
					paths.Add(path);
					DebugLine("start" + path + ",end");
					continue;
				}

				// small cave
				if (!cave.Any(c => char.IsUpper(c)))
				{
					if (visited.Count(c => c == cave) == 2)
						continue;

					if (visited.Contains(cave) && visited
						.GroupBy(c => c)
						.Where(cave => !cave.Key.Any(c => char.IsUpper(c)))
						.Any(g => g.Count() == 2))
						continue;
				}

				// big cave
				VisitCave_OneSmallTwice(caves, cave, path + "," + cave, paths);
			}
		}
	}
}
