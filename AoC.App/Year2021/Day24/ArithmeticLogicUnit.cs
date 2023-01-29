using System.Collections.Concurrent;
using Ujeby.AoC.Common;
using Ujeby.Vectors;

namespace Ujeby.AoC.App.Year2021.Day24
{
	public class ArithmeticLogicUnit : PuzzleBase
	{
		protected override (string, string) SolvePuzzle(string[] input)
		{
			Debug.Line();

			//var valid = new ConcurrentBag<long>();
			//var monad = input.Select(i => i.Split(' ').ToArray()).ToArray();
			//Parallel.For(0L, 88888888888888L, (i, state) =>
			//{
			//	var modelN = 99999999999999L - i;
			//	var modelNStr = modelN.ToString();
			//	if (!modelNStr.Contains('0') && Valid(modelNStr, monad))
			//	{
			//		Log.Line(modelNStr);
			//		valid.Add(modelN);
			//		state.Break();
			//	}
			//	else if (state.ShouldExitCurrentIteration)
			//		return;
			//});

			// part1
			long? answer1 = null;

			// part2
			long? answer2 = null;

			Debug.Line();

			return (answer1?.ToString(), answer2?.ToString());
		}

		private static bool Valid(string input, string[][] program)
		{
			var iIdx = 0;
			var reg = new v4i();
			foreach (var i in program)
			{
				if (i[0] == "inp")
				{
					//Debug.Line($"input[{iIdx}]: {input[iIdx]}");
					reg[_regToV4iIdx[i[1]]] = input[iIdx++] - '0';
				}
				else
				{
					reg = _instr[i[0]](i[1], i[2], reg);
					//Debug.Line($"{i[0]} {i[1]} {i[2],3} {reg}");
				}
			}

			var result = reg.Z == 0;
			//Debug.Line($"{input}: {reg} {result}");
			return result;
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
