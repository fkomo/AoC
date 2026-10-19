using System.Text;
using Ujeby.AoC.Common;

namespace Ujeby.AoC.App._2016_16;

[AoCPuzzle(Year = 2016, Day = 16, Answer1 = "10111110010110110", Answer2 = "01101100001100100", Skip = false)]
public class DragonChecksum : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var diskSize = 272;
		// sample
		//var diskSize = 20;

		// part1
		var answer1 = Checksum(FillDisk(input.Single(), diskSize));

		// part2
		var answer2 = Checksum(FillDisk(input.Single(), 35651584));

		return (answer1?.ToString(), answer2?.ToString());
	}

	private static string DragonCurveStep(string a)
	{
		// reverse copy of a
		var b = a.Reverse().ToArray();

		// turn b
		for (var i = 0; i < b.Length; i++)
			b[i] = (b[i] == '0') ? '1' : '0';

		return a + '0' + new string(b);
	}

	public static string FillDisk(string data, int size)
	{
		while (data.Length < size)
			data = DragonCurveStep(data);
		return data[..size];
	}

	public static string Checksum(string data)
	{
		var sb = new StringBuilder();
		do
		{
			sb.Clear();
			for (var i = 0; i < data.Length; i += 2)
				sb.Append((data[i] != data[i + 1]) ? '0' : '1');
			data = sb.ToString();
		}
		while (data.Length % 2 == 0);

		return data;
	}
}