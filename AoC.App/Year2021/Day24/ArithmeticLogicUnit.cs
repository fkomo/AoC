using Ujeby.AoC.Common;
using Ujeby.Vectors;

namespace Ujeby.AoC.App.Year2021.Day24
{
	public class ArithmeticLogicUnit : PuzzleBase
	{
		/// <summary>
		/// https://www.reddit.com/r/adventofcode/comments/rnejv5/2021_day_24_solutions/hs4g15j/
		/// https://www.ericburden.work/blog/2022/01/05/advent-of-code-2021-day-24/
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		protected override (string, string) SolvePuzzle(string[] input)
		{
			Debug.Line();

			var monad = input.Select(i => i.Split(' ').ToArray()).ToArray();

			var p1 = Enumerable.Range(0, 14).Select(n => int.Parse(monad[4 + n * 18][2])).ToArray();
			var p2 = Enumerable.Range(0, 14).Select(n => int.Parse(monad[5 + n * 18][2])).ToArray();
			var p3 = Enumerable.Range(0, 14).Select(n => int.Parse(monad[15 + n * 18][2])).ToArray();

			// part1
			long? answer1 = RecursiveMonadSearch(true, p1, p2, p3);

			// part2
			long? answer2 = RecursiveMonadSearch(false, p1, p2, p3);

			Debug.Line();

			return (answer1?.ToString(), answer2?.ToString());
		}

		private static long? RecursiveMonadSearch(bool max, int[] p1, int[] p2, int[] p3,
			long monad = 0, int mIdx = 0, long z = 0)
		{
			if (mIdx == 14)
				return ValidCompiled(monad, p1, p2, p3) ? monad : null;

			var n = Enumerable.Range(1, 9);
			if (max)
				n = n.Reverse();

			foreach (var m in n)
			{
				if (p2[mIdx] > 0 || z % 26 == m - p2[mIdx])
				{
					var z1 = AluStep(z, m, mIdx, p1, p2, p3);
					var result = RecursiveMonadSearch(max, p1, p2, p3, 
						monad: monad * 10 + m, mIdx: mIdx + 1, z: z1);

					if (result != null)
						return result;
				}
			}

			return null;
		}

		private static bool ValidCompiled(long input, int[] p1, int[] p2, int[] p3)
			=> ValidCompiled(input.ToString(), p1, p2, p3);

		private static bool ValidCompiled(string input, int[] p1, int[] p2, int[] p3)
		{
			var z = 0L;
			for (var i = 0; i < 14; i++)
				z = AluStep(z, input[i] - '0', i, p1, p2, p3);

			if (z == 0)
				Debug.Line(input);

			return z == 0;
		}

		private static long AluStep(long z, int w, int i, int[] p1, int[] p2, int[] p3)
		{
			var x = (z % 26) + p2[i];
			z /= p1[i];
			if (x != w)
				z = z * 26 + w + p3[i];
			return z;
		}

		private static bool Valid(string input, string[][] program)
		{
			var iIdx = 0;
			var reg = new v4i();
			foreach (var i in program)
			{
				if (i[0] == "inp")
					reg[_regToV4iIdx[i[1]]] = input[iIdx++] - '0';
				else
					reg = _instr[i[0]](i[1], i[2], reg);
			}

			if (reg.Z == 0)
				Debug.Line(input);

			return reg.Z == 0;
		}

		private static Dictionary<string, Func<string, string, v4i, v4i>> _instr = new()
		{
			{ "add", (opL, opR, regIn) =>
				{
					if (!long.TryParse(opR, out long r))
						r = regIn[_regToV4iIdx[opR]];

					var regOut = regIn;
					regOut[_regToV4iIdx[opL]] += r;
					return regOut;
				}
			},
			{ "mul", (opL, opR, regIn) =>
				{
					if (!long.TryParse(opR, out long rightOp))
						rightOp = regIn[_regToV4iIdx[opR]];

					var regOut = regIn;
					regOut[_regToV4iIdx[opL]] *= rightOp;
					return regOut;
				}
			},
			{ "div", (opL, opR, regIn) =>
				{
					if (!long.TryParse(opR, out long rightOp))
						rightOp = regIn[_regToV4iIdx[opR]];

					var regOut = regIn;
					regOut[_regToV4iIdx[opL]] /= rightOp;
					return regOut;
				}
			},
			{ "mod", (opL, opR, regIn) =>
				{
					if (!long.TryParse(opR, out long rightOp))
						rightOp = regIn[_regToV4iIdx[opR]];

					var regOut = regIn;
					regOut[_regToV4iIdx[opL]] %= rightOp;
					return regOut;
				}
			},
			{ "eql", (opL, opR, regIn) =>
				{
					if (!long.TryParse(opR, out long rightOp))
						rightOp = regIn[_regToV4iIdx[opR]];

					var regOut = regIn;
					regOut[_regToV4iIdx[opL]] = (_regToV4iIdx[opL] == rightOp) ? 1 : 0;
					return regOut;
				}
			},
		};

		private static readonly Dictionary<string, int> _regToV4iIdx = new()
		{
			{ "x", 0 },
			{ "y", 1 },
			{ "z", 2 },
			{ "w", 3 },
		};
	}
}
