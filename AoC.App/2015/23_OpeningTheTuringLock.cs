using Ujeby.AoC.Common;
using Ujeby.Extensions;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2015_23
{
	[AoCPuzzle(Year = 2015, Day = 23, Answer1 = "255", Answer2 = "334")]
	public class OpeningTheTuringLock : PuzzleBase
	{
		protected override (string Part1, string Part2) SolvePuzzle(string[] input)
		{
			// part1
			var answer1 = Exec(new v2i(), input);

			// part2
			var answer2 = Exec(new v2i(1, 0), input);

			return (answer1.ToString(), answer2.ToString());
		}

		private static long Exec(v2i reg, string[] instr)
		{
			var rn = new Dictionary<char, int>
			{
				{ 'a', 0 },
				{ 'b', 1 },
			};

			var i = 0L;
			while (i >= 0 && i < instr.Length)
			{
				switch (instr[i][..3])
				{
					case "hlf":
						reg[rn[instr[i][4]]] = reg[rn[instr[i][4]]] / 2;
						break;
					case "tpl":
						reg[rn[instr[i][4]]] = reg[rn[instr[i][4]]] * 3;
						break;
					case "inc":
						reg[rn[instr[i][4]]]++;
						break;
					case "jmp":
						i += instr[i].ToNumArray().Single();
						continue;
					case "jie":
						if (reg[rn[instr[i][4]]] % 2 == 0)
						{
							i += instr[i].ToNumArray().Single();
							continue;
						}
						break;
					case "jio":
						if (reg[rn[instr[i][4]]] == 1)
						{
							i += instr[i].ToNumArray().Single();
							continue;
						}
						break;
				}

				i++;
			}

			return reg[rn['b']];
		}
	}
}
