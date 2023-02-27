using Ujeby.AoC.Common;

namespace Ujeby.AoC.App._2015_19
{
	[AoCPuzzle(Year = 2015, Day = 19, Answer1 = "518", Answer2 = "200")]
	public class MedicineForRudolph : PuzzleBase
	{
		protected override (string Part1, string Part2) SolvePuzzle(string[] input)
		{
			var replacements = input.Take(Array.IndexOf(input, ""))
				.GroupBy(i => i.Split(' ')[0])
				.ToDictionary(i => i.Key, i => i.Select(x => x.Split(' ')[2]).ToArray());
			var medicine = input[Array.IndexOf(input, "") + 1];

			// part1
			var molecules = new HashSet<string>();
			foreach (var r in replacements)
				foreach (var rv in r.Value)
				{
					for (var i = medicine.IndexOf(r.Key, 0); i != -1; i = medicine.IndexOf(r.Key, i + 1))
						molecules.Add(Replace(medicine, r.Key, rv, i));
				}
			var answer1 = molecules.Count;

			// part2
			var answer2 = 1L;
			while (!replacements["e"].Any(x => x == medicine))
			{
				var replaced = false;
				foreach (var r in replacements.Where(x => x.Key != "e"))
				{
					foreach (var rv in r.Value)
					{
						if (!medicine.Contains(rv))
							continue;

						var i = medicine.IndexOf(rv);
						if (i != -1)
						{
							medicine = Replace(medicine, rv, r.Key, i);
							replaced = true;
							answer2++;
							break;
						}
					}

					if (replaced)
						break;
				}
			}

			return (answer1.ToString(), answer2.ToString());
		}

		public static string Replace(string s, string old, string _new)
			=> Replace(s, old, _new, s.IndexOf(old));

		public static string Replace(string s, string old, string _new, int i)
			=> s[0..i] + _new + s[(i + old.Length)..];

		private static Dictionary<string, long> _cache = new();

		private long CountStepsRec(string current, string end, long steps, Dictionary<string, string[]> replacements)
		{
			if (_cache.TryGetValue(current, out long cached))
				return cached;

			if (current.Length > end.Length)
				return long.MaxValue;

			if (current == end)
				return steps;

			var min = long.MaxValue;

			var possibleSteps = replacements.Keys.Where(k => current.Contains(k)).ToArray();
			foreach (var r in possibleSteps)
			{
				foreach (var rv in replacements[r])
				{
					for (var i = current.IndexOf(r, 0); i != -1; i = current.IndexOf(r, i + 1))
					{
						var cnt = CountStepsRec(Replace(current, r, rv, i), end, steps + 1, replacements);
						if (cnt < min)
							min = cnt;
					}
				}
			}

			_cache.Add(current, min);
			return min;
		}
	}
}
