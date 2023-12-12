using Ujeby.AoC.Common;
using Ujeby.Tools.StringExtensions;

namespace Ujeby.AoC.App._2023_12;

[AoCPuzzle(Year = 2023, Day = 12, Answer1 = "7084", Answer2 = null, Skip = false)]
public class HotSprings : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var records = input
			.Select(x => (Mask: x[..x.IndexOf(' ')], DmgGroups: x.ToNumArray()))
			.ToArray();

		// part1
		long answer1 = 0;
		foreach (var (Mask, DmgGroups) in records)
		{
			Debug.Line($"{Mask} {string.Join(',', DmgGroups)}");
			var count = PossibleArrangements(Mask, DmgGroups);
			Debug.Line($"{count}{Environment.NewLine}");
			answer1 += count;
		}

		// part2
		string answer2 = null;

		return (answer1.ToString(), answer2?.ToString());
	}

	static long PossibleArrangements(string mask, long[] dmgGroups, 
		string tmp = "", int maskIdx = 0, int dmgGrpIdx = 0)
	{
		if (dmgGrpIdx == dmgGroups.Length)
		{
			Debug.Line($"{tmp.PadRight(mask.Length, '.')}");
			if (maskIdx < mask.Length && mask[maskIdx..].Contains('#'))
				return 0;

			// final check
			for (var i = 0; i < mask.Length; i++)
				if (mask[i] == '#' && tmp[i] != '#')
					return 0;

			return 1;
		}

		var to = mask.Length - (dmgGroups.Skip(dmgGrpIdx).Sum() + (dmgGroups.Length - dmgGrpIdx - 1));
		var dmg = (int)dmgGroups[dmgGrpIdx];

		long count = 0;
		for (var i = maskIdx; i <= to; i++)
		{
			if (maskIdx + dmg > mask.Length)
				continue;

			var valid = true;
			for (var x = i; x < i + dmg; x++)
			{
				if (mask[x] == '.')
				{
					valid = false;
					break;
				}
			}
			if (!valid || (valid && i + dmg < mask.Length && mask[i + dmg] == '#'))
				continue;

			var _tmp = tmp + new string(Enumerable.Repeat('.', i - maskIdx).ToArray()) + new string(Enumerable.Repeat('#', dmg).ToArray());
			if (i + dmg < mask.Length)
				_tmp += '.';

			count += PossibleArrangements(mask, dmgGroups, 
				tmp: _tmp,
				maskIdx: i + dmg + 1, dmgGrpIdx: dmgGrpIdx + 1);

			if (mask[i] == '#')
				break;
		}

		return count;
	}
}