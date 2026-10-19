using Ujeby.AoC.Common;
using Ujeby.Extensions;

namespace Ujeby.AoC.App._2016_21;

[AoCPuzzle(Year = 2016, Day = 21, Answer1 = "gcedfahb", Answer2 = "hegbdcfa", Skip = false)]
public class ScrambledLettersAndHash : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var password = "abcdefgh";
		// sample
		//var password = "abcde";

		// part1
		var answer1 = Scramble(password, input);

		// part2
		var answer2 = Scramble("fbgdceah", input, reverse: true);
		// dgebhfca not right

		return (answer1?.ToString(), answer2?.ToString());
	}

	private static string Scramble(string password, IEnumerable<string> instructions, 
		bool reverse = false)
	{
		var tmp = password.ToArray();

		if (reverse)
			instructions = instructions.Reverse();

		Debug.Line($"{password}");
		foreach (var instr in instructions)
		{
			var i = instr
				.ToNumArray()
				.Select(x => (int)x)
				.ToArray();

			if (instr.StartsWith("swap"))
			{
				if (instr.Contains("letter"))
					i = new int[]
					{
						Array.IndexOf(tmp, instr["swap letter ".Length]),
						Array.IndexOf(tmp, instr[^1])
					};

				(tmp[i[1]], tmp[i[0]]) = (tmp[i[0]], tmp[i[1]]);
			}
			else if (instr.StartsWith("rotate"))
			{
				if (instr.Contains("based"))
				{
					var ci = Array.IndexOf(tmp, instr[^1]);

					if (reverse)
					{
						for (var ci2 = 0; ci2 < tmp.Length; ci2++)
						{
							if (ci2 == ci)
								continue;

							var d = 1 + ci2 + (ci2 >= 4 ? 1 : 0);
							var rDest = (d + ci2) % tmp.Length;
							if (rDest != ci)
								continue;

							tmp = RotateLeft(d, tmp);
							break;
						}
					}
					else
						tmp = RotateRight(1 + ci + (ci >= 4 ? 1 : 0), tmp);
				}
				else
				{
					if (instr.Contains("right"))
						tmp = reverse ? RotateLeft(i[0], tmp) : RotateRight(i[0], tmp);
					else // left
						tmp = reverse ? RotateRight(i[0], tmp) : RotateLeft(i[0], tmp);
				}
			}
			else if (instr.StartsWith("reverse"))
			{
				tmp = tmp[0..System.Math.Max(i[0] - 1, i[0])]
					.Concat(tmp[i[0]..(i[1] + 1)].Reverse())
					.Concat(tmp[(i[1] + 1)..])
					.ToArray();
			}
			else if (instr.StartsWith("move"))
			{
				if (reverse)
					(i[0], i[1]) = (i[1], i[0]);

				var m = tmp[i[0]];

				var dir = 1;
				int x = 0, x2 = 0;
				if (i[0] > i[1])
				{
					x = x2 = tmp.Length - 1;
					dir = -1;
				}

				for (; x >= 0 && x < tmp.Length; x += dir, x2 += dir)
				{
					if (x == i[1])
					{
						tmp[x] = m;
						x2 -= dir;
					}
					else
					{
						if (x == i[0])
							x2 += dir;

						tmp[x] = tmp[x2];
					}
				}
			}
			Debug.Line($"{new string(tmp)}: {instr}");
		}

		Debug.Line();
		return new string(tmp);
	}

	private static char[] RotateRight(int n, char[] password)
	{
		n %= password.Length;
		return password[^n..]
			.Concat(password.Take(password.Length - n))
			.ToArray();
	}

	private static char[] RotateLeft(int n, char[] password)
	{
		n %= password.Length;
		return password[^(password.Length - n)..]
			.Concat(password.Take(n))
			.ToArray();
	}
}