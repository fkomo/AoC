using Ujeby.AoC.Common;

namespace Ujeby.AoC.App._2015_17
{
	[AoCPuzzle(Year = 2015, Day = 17, Answer1 = "1638", Answer2 = "17")]
	public class NoSuchThingAsTooMuch : PuzzleBase
	{
		protected override (string Part1, string Part2) SolvePuzzle(string[] input)
		{
			var eggnogSize = 150;
#if _DEBUG_SAMPLE
			eggnogSize = 25;	
#endif
			var containers = input.Select(i => int.Parse(i)).ToArray();

			// part1
			var combinations = new Dictionary<string, int>();
			var usedIdx = Enumerable.Repeat(-1, containers.Length).ToArray();
			FitEggnogRec(eggnogSize, containers, combinations, usedIdx);
			var answer1 = combinations.Count;

			// part2
			var shortest = combinations.Values.Min();
			var answer2 = combinations.Count(c => c.Value == shortest);

			return (answer1.ToString(), answer2.ToString());
		}

		private static readonly Dictionary<string, int> _cache = new();

		private void FitEggnogRec(int eggnogSize, int[] containers, Dictionary<string, int> combinations, int[] buffer,
			int bufferUsage = 0)
		{
			if (bufferUsage == containers.Length)
				return;

			var usedOnly = buffer.Take(bufferUsage).ToArray();

			var id = string.Join('-', usedOnly.OrderBy(i => i));
			if (_cache.ContainsKey(id))
				return;

			var usedSize = usedOnly.Sum(i => containers[i]);
			for (var i = 0; i < containers.Length; i++)
			{
				if (usedOnly.Contains(i))
					continue;

				var newSize = usedSize + containers[i];
				if (newSize > eggnogSize)
					continue;

				buffer[bufferUsage] = i;
				if (newSize == eggnogSize)
				{
					var combination = string.Join('-', buffer.Take(bufferUsage + 1).OrderBy(i => i));
					if (!combinations.ContainsKey(combination))
						combinations.Add(combination, bufferUsage + 1);
				}
				else
					FitEggnogRec(eggnogSize, containers, combinations, buffer, bufferUsage + 1);
			}

			_cache.Add(id, 0);
		}
	}
}
