using Ujeby.AoC.Common;
using Ujeby.Tools.StringExtensions;

namespace Ujeby.AoC.App._2016_21;

[AoCPuzzle(Year = 2016, Day = 21, Answer1 = "gcedfahb", Answer2 = null, Skip = false)]
public class ScrambledLettersAndHash : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var password = "abcdefgh".ToArray();
#if _DEBUG_SAMPLE
		password = "abcde".ToArray();
#endif
		// part1
		Debug.Line(new string(password));
		foreach (var instr in input)
		{
			Debug.Line(instr);
			var i = instr
				.ToNumArray()
				.Select(x => (int)x)
				.ToArray();

			if (instr.StartsWith("swap"))
			{
				if (instr.Contains("letter"))
					i = new int[]
					{
						Array.IndexOf(password, instr["swap letter ".Length]),
						Array.IndexOf(password, instr[^1])
					};

				(password[i[1]], password[i[0]]) = (password[i[0]], password[i[1]]);
			}
			else if (instr.StartsWith("rotate"))
			{
				if (instr.Contains("based"))
				{
					var n = Array.IndexOf(password, instr[^1]);
					password = RotateRight(1 + n + (n >= 4 ? 1 : 0), password);
				}
				else
				{
					if (instr.Contains("right"))
						password = RotateRight(i[0], password);
					else // left
						password = RotateLeft(i[0], password);
				}
			}
			else if (instr.StartsWith("reverse"))
			{
				password = password[0..Math.Max(i[0] - 1, i[0])]
					.Concat(password[i[0]..(i[1] + 1)].Reverse())
					.Concat(password[(i[1] + 1)..])
					.ToArray();
			}
			else if (instr.StartsWith("move"))
			{
				var m = password[i[0]];

				var dir = 1;
				int x = 0, x2 = 0;
				if (i[0] > i[1])
				{
					x = x2 = password.Length - 1;
					dir = -1;
				}

				for (; x >= 0 && x < password.Length; x += dir, x2 += dir)
				{
					if (x == i[1])
					{
						password[x] = m;
						x2 -= dir;
					}
					else
					{
						if (x == i[0])
							x2 += dir;

						password[x] = password[x2];
					}
				}
			}
			Debug.Line(new string(password));
			Debug.Line();
		}
		var answer1 = new string(password);

		// part2
		string answer2 = null;

		return (answer1?.ToString(), answer2?.ToString());
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