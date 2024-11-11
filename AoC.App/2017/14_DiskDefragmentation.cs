using Ujeby.AoC.Common;
using Ujeby.Vectors;
using Ujeby.Grid.CharMapExtensions;

namespace Ujeby.AoC.App._2017_14;

[AoCPuzzle(Year = 2017, Day = 14, Answer1 = "8216", Answer2 = "1139", Skip = false)]
public class DiskDefragmentation : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var mem = CreateMemMap(input);

		// part1
		var answer1 = mem.Sum(x => x.Count(x2 => x2 == '#'));

		// part2
		var regions = 0;
		for (var y = 0; y < mem.Length; y++)
		{
			for (var x = 0; x < mem.Length; x++)
			{
				if (mem[y][x] == '#')
				{
					mem.FloodFill(new v2i(x, y), 'x', v2i.DownUpLeftRight, '.');
					regions++;
				}
			}
		}

		var answer2 = regions;

		return (answer1.ToString(), answer2.ToString());
	}

	public static char[][] CreateMemMap(string[] input) => Enumerable.Range(0, 128)
		.Select(x => _2017_10.KnotHash.Compute($"{input[0]}-{x}"))
		.Select(HexStringToMap)
		.ToArray();

	static char[] HexStringToMap(string s) => s
		.SelectMany(x => Math.DecToBase(HexCharIndex(x)).PadLeft(4, '0').Take(4).Select(b => b == '1' ? '#' : '.'))
		.ToArray();

	static int HexCharIndex(char c) => "0123456789abcdef".IndexOf(char.ToLower(c));
}