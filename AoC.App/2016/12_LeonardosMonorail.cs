using Ujeby.AoC.Common;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2016_12;

[AoCPuzzle(Year = 2016, Day = 12, Answer1 = "318003", Answer2 = "9227657", Skip = false)]
public class LeonardosMonorail : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var instr = input.Select(x => x.Split(' ')).ToArray();

		// part1
		var answer1 = ExecPasswordCheckingLogic(instr, new v4i(0)).X;

		// part2
		var answer2 = ExecPasswordCheckingLogic(instr, new v4i(0, 0, 1, 0)).X;

		return (answer1.ToString(), answer2.ToString());
	}

	private static v4i ExecPasswordCheckingLogic(string[][] instr, v4i reg)
	{
		for (var i = 0; i < instr.Length; i++)
		{
			var ii = instr[i];
			switch (ii[0][0])
			{
				case 'c':
					{
						if (!long.TryParse(ii[1], out long op1))
							op1 = reg[ii[1][0] - 'a'];
						reg[ii[2][0] - 'a'] = op1;
					}
					break;

				case 'i':
					reg[ii[1][0] - 'a']++;
					break;

				case 'd':
					reg[ii[1][0] - 'a']--;
					break;

				case 'j':
					{
						if (!long.TryParse(ii[1], out long op1))
							op1 = reg[ii[1][0] - 'a'];

						if (op1 > 0)
							i += int.Parse(ii[2]) - 1;
					}
					break;
			}
		}

		return reg;
	}
}