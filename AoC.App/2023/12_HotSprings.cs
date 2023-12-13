using Ujeby.AoC.Common;
using Ujeby.Tools.StringExtensions;

namespace Ujeby.AoC.App._2023_12;

[AoCPuzzle(Year = 2023, Day = 12, Answer1 = "7084", Answer2 = "8414003326821", Skip = false)]
public class HotSprings : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		// part1
		_cache.Clear();
		long answer1 = 0;
		for (var i = 0; i < input.Length; i++)
		{
			var record = input[i];

			Debug.Line($"* {record}");
			var count = PossibleArrangements(i, record);
			Debug.Line($"  {count}{Environment.NewLine}");
			answer1 += count;
		}

		// part2
		_cache.Clear();
		long answer2 = 0;
		for (var i = 0; i < input.Length; i++)
			answer2 += PossibleArrangementsUnfolded(i, input[i]);

		return (answer1.ToString(), answer2.ToString());
	}

	static long PossibleArrangements(int id, string record)
		=> PossibleArrangements(id, record[..record.IndexOf(' ')], record.ToNumArray());

	static long PossibleArrangementsUnfolded(int id, string record)
		=> PossibleArrangements(id,
			string.Join('?', Enumerable.Repeat(record[..record.IndexOf(' ')], 5)),
			string.Join(',', Enumerable.Repeat(record[(record.IndexOf(' ') + 1)..], 5)).ToNumArray());

	static Dictionary<(int, int, int), long> _cache = new();

	static long PossibleArrangements(int id, string mask, long[] dmgGroups, 
		int maskIdx = 0, int dmgGrpIdx = 0)
	{
		var cacheKey = (id, maskIdx, dmgGrpIdx);
		if (_cache.ContainsKey(cacheKey))
			return _cache[cacheKey];

		if (dmgGrpIdx == dmgGroups.Length)
			return (maskIdx < mask.Length && mask[maskIdx..].Contains('#')) ? 0 : 1;

		var to = mask.Length - (dmgGroups.Skip(dmgGrpIdx).Sum() + (dmgGroups.Length - dmgGrpIdx - 1));
		var dmg = (int)dmgGroups[dmgGrpIdx];

		long count = 0;
		for (var i = maskIdx; i <= to; i++)
		{
			if (maskIdx + dmg > mask.Length)
				continue;

			var free = true;
			for (var x = i; x < i + dmg; x++)
			{
				if (mask[x] == '.')
				{
					free = false;
					break;
				}
			}
			if (!free || (free && i + dmg < mask.Length && mask[i + dmg] == '#'))
			{
				if (mask[i] == '#')
					break;

				continue;
			}

			count += PossibleArrangements(id, mask, dmgGroups, 
				maskIdx: i + dmg + 1, dmgGrpIdx: dmgGrpIdx + 1);

			if (mask[i] == '#')
				break;
		}

		_cache.Add(cacheKey, count);

		return count;
	}
}