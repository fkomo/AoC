using Ujeby.AoC.Common;
using Ujeby.Extensions;

namespace Ujeby.AoC.App._2024_25;

[AoCPuzzle(Year = 2024, Day = 25, Answer1 = "2824", Answer2 = "*", Skip = false)]
public class CodeChronicle : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var schematics = input.Split(string.Empty)
			.Select(x =>
				{
					var isLock = x[0].All(x => x == '#') && x[^1].All(x => x == '.');

					var scheme = x.Skip(1).Take(x.Length - 2)
						.Select(x => x.ToArray()).ToArray()
						.RotateCW()
						.Select(x => x.Count(c => c == '#'))
						.ToArray();

					return (IsLock: isLock, Scheme: PackScheme(scheme, isLock));
				})
			.ToArray();

		// part1
		var locks = schematics.Where(x => x.IsLock).Select(x => x.Scheme).Distinct().ToArray();
		var keys = schematics.Where(x => !x.IsLock).Select(x => x.Scheme).Distinct().ToArray();

		Debug.Line($"{schematics.Length} schematics");
		Debug.Line($"{locks.Length} distinct locks");
		Debug.Line($"{keys.Length} distinct keys");

		var answer1 = 0L;
		for (var l = 0; l < locks.Length; l++)
			for (var k = 0; k < keys.Length; k++)
			{
				if ((locks[l] & keys[k]) != 0) 
					continue;

				answer1++;
			}

		// part2
		var answer2 = "*";

		return (answer1.ToString(), answer2.ToString());
	}

	static uint PackScheme(int[] rawScheme, bool isLock)
	{
		var result = uint.MinValue;

		for (var i = 0; i < rawScheme.Length; i++)
		{
			var offset = isLock ? i * 5 : ((i + 1) * 5 - 1);
			for (var b = 0; b < rawScheme[i]; b++)
			{
				result |= (uint)System.Math.Pow(2, offset);
				offset = isLock ? offset + 1 : offset - 1;
			}
		}

		//Debug.Line($"{string.Join(',', rawScheme)} {Ujeby.Math.DecToBase(result).PadLeft(25, '0')}");
		return result;
	}
}