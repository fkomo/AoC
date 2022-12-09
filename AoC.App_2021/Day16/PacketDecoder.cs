using System.Globalization;
using Ujeby.AoC.Common;

namespace Ujeby.AoC.App.Day16
{
	internal class PacketDecoder : ProblemBase
	{
		protected override (string, string) SolveProblem(string[] input)
		{
			var bytes = new byte[input[0].Length / 2];
			for (var i = 0; i < bytes.Length; i++)
				bytes[i] = byte.Parse($"{input[0][i * 2]}{input[0][i * 2 + 1]}", NumberStyles.HexNumber);

			// part1
			long? answer1 = null;

			// part2
			long? answer2 = null;

			return (answer1?.ToString(), answer2?.ToString());
		}
	}
}
