using System.Linq;
using Ujeby.AoC.Common;
using Ujeby.Extensions;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2018_18;

[AoCPuzzle(Year = 2018, Day = 18, Answer1 = "653184", Answer2 = "169106", Skip = false)]
public class SettlersOfTheNorthPole : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var map = CreateMap(input);

		// part1
		var answer1 = GetResourceValueAfter(map.Copy());

		// part2
		var answer2 = GetResourceValueAfter(map.Copy(), minutes: 1_000_000_000);

		return (answer1.ToString(), answer2.ToString());
	}

	public static char[][] CreateMap(string[] input)
	{
		var emptyRow = Enumerable.Repeat('\0', input.Length + 2).ToArray();
		
		return input.Select(x => $"\0{x}\0".ToArray())
			.Prepend(emptyRow)
			.Append(emptyRow)
			.ToArray();
	}

	public static void WaitOneMinute(char[][] sourceMap, char[][] destMap, v2i[] acres)
	{
		foreach (var a in acres)
		{
			var tile = sourceMap.Get(a);
			destMap.Set(a, tile);

			LookAround(sourceMap, a, out int trees, out int open, out int lumberyards);

			if (tile == '.' && trees >= 3)
				destMap.Set(a, '|');

			else if (tile == '|' && lumberyards >= 3)
				destMap.Set(a, '#');

			else if (tile == '#' && (lumberyards < 1 || trees < 1))
				destMap.Set(a, '.');
		}
	}

	static int GetResourceValueAfter(char[][] map, long minutes = 10)
	{
		var acres = new aab2i(new v2i(1), new v2i(map.Length - 2)).EnumPoints().ToArray();

		var hashes = new Dictionary<string, (long Minute, int ResourceValue)>();

		var map2 = map.Copy();
		for (var m = 0; m < minutes; m++)
		{
			WaitOneMinute(map, map2, acres);
			var resourceValue = GetResourceValue(map);

			var hash = Ujeby.Tools.Hashing.FormatHash(System.Security.Cryptography.SHA256.HashData([.. map.Flatten().Select(x => (byte)x)]));
			if (!hashes.TryAdd(hash, (m, resourceValue)))
			{
				var cycleStart = hashes[hash].Minute;
				var cycleEnd = hashes.Last().Value.Minute;

				// hash at minute 604 == 548
				return hashes.Skip((int)(cycleStart + (minutes - cycleStart) % (cycleEnd - cycleStart + 1))).First().Value.ResourceValue;
			}

			(map, map2) = (map2, map);
		}

		return GetResourceValue(map);
	}

	static int GetResourceValue(char[][] map) => map.Sum(x => x.Count(xx => xx == '|')) * map.Sum(x => x.Count(xx => xx == '#'));

	static void LookAround(char[][] map, v2i a, out int trees, out int open, out int lumberyards)
	{
		trees = open = lumberyards = 0;

		foreach (var n in v2i.PlusMinusOne.Select(x => x + a))
		{
			switch (map.Get(n))
			{
				case '.': open++; break;
				case '|': trees++; break;
				case '#': lumberyards++; break;
				default:
					break;
			}
		}
	}
}