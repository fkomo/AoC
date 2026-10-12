using Ujeby.AoC.Common;

namespace Ujeby.AoC.App._2018_04;

[AoCPuzzle(Year = 2018, Day = 04, Answer1 = "118599", Answer2 = "33949", Skip = false)]
public class ReposeRecord : PuzzleBase
{
	const string _timestampFormat = "yyyy-MM-dd HH:mm";
	const int _from = 0;
	const int _to = 1;

	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var guards = ProcessSleepLog(input);

		// part1
		var guardWithMostSleep = int.Parse(guards.OrderByDescending(x => x.Value.Sum(xx => (xx[_to] - xx[_from]).Minutes)).First().Key[1..]);
		var answer1 = guardWithMostSleep * MostSleptMinute(guards[$"#{guardWithMostSleep}"], out _);

		// part2
		string mostFreqGuard = null;
		var mostFreqMin = -1;
		var mostFreqCnt = int.MinValue;
		foreach (var guard in guards)
		{
			var min = MostSleptMinute(guard.Value, out int cnt);
			if (cnt > mostFreqCnt)
			{
				mostFreqCnt = cnt;
				mostFreqMin = min;
				mostFreqGuard = guard.Key;
			}
		}

		var answer2 = mostFreqMin * int.Parse(mostFreqGuard[1..]);

		return (answer1.ToString(), answer2.ToString());
	}

	static Dictionary<string, List<DateTime[]>> ProcessSleepLog(string[] input)
	{
		var log = input
			.ToDictionary(
				x => DateTime.ParseExact(x[..$"[{_timestampFormat}]".Length], $"[{_timestampFormat}]", null),
				x => x.Split(' ')[3])
			.OrderBy(x => x.Key);

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

		return guards;
	}

	static int MostSleptMinute(List<DateTime[]> sleeps, out int count)
	{
		var minDict = new Dictionary<int, int>();
		foreach (var sleep in sleeps)
			for (var m = sleep[_from].Minute; m < sleep[_to].Minute; m++)
			{
				if (!minDict.ContainsKey(m))
					minDict[m] = 0;

				minDict[m]++;
			}

		var max = minDict.MaxBy(x => x.Value);

		count = max.Value;
		return max.Key;
	}
}