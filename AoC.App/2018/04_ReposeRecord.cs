using Ujeby.AoC.Common;

namespace Ujeby.AoC.App._2018_04;

[AoCPuzzle(Year = 2018, Day = 04, Answer1 = "118599", Answer2 = null, Skip = false)]
public class ReposeRecord : PuzzleBase
{
	const string _timestampFormat = "yyyy-MM-dd HH:mm";
	const int _from = 0;
	const int _to = 1;

	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var log = input
			.ToDictionary(
				x => DateTime.ParseExact(x[..$"[{_timestampFormat}]".Length], $"[{_timestampFormat}]", null),
				x => x.Split(' ')[3])
			.OrderBy(x => x.Key);

		// part1
		string currentGuard = null;
		DateTime from = DateTime.MinValue, to = DateTime.MinValue;

		var guards = new Dictionary<string, List<DateTime[]>>();
		foreach (var entry in log)
		{
			var value = entry.Value;

			if (value[0] == '#')
				currentGuard = entry.Value;

			else if (value[0] == 'a')
				from = entry.Key;

			else if (value[0] == 'u')
			{
				to = entry.Key;
				if (!guards.TryGetValue(currentGuard, out List<DateTime[]> schedule))
					guards[currentGuard] = [];

				guards[currentGuard].Add([from, to]);
				from = DateTime.MinValue;
				to = DateTime.MinValue;
			}
		}

		var guardWithMostSleep = int.Parse(guards.OrderByDescending(x => x.Value.Sum(xx => (xx[_to] - xx[_from]).Minutes)).First().Key[1..]);

		var minDict = new Dictionary<int, int>();
		foreach (var sleep in guards[$"#{guardWithMostSleep}"])
			for (var m = sleep[_from].Minute; m < sleep[_to].Minute; m++)
			{
				if (!minDict.ContainsKey(m))
					minDict[m] = 0;

				minDict[m]++;
			}
		var answer1 = guardWithMostSleep * minDict.MaxBy(x => x.Value).Key;

		// part2
		string answer2 = null;

		return (answer1.ToString(), answer2?.ToString());
	}
}