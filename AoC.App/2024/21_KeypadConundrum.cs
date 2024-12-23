using Ujeby.AoC.Common;
using Ujeby.Extensions;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2024_21;

[AoCPuzzle(Year = 2024, Day = 21, Answer1 = null, Answer2 = null, Skip = false)]
public class KeypadConundrum : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var numKeypad = new Dictionary<char, v2i>()
		{
			{ 'A', v2i.Zero },
			{ '0', new v2i(-1, 0) },
			{ '1', new v2i(-2, -1) },
			{ '2', new v2i(-1, -1) },
			{ '3', new v2i(0, -1) },
			{ '4', new v2i(-2, -2) },
			{ '5', new v2i(-1, -2) },
			{ '6', new v2i(0, -2) },
			{ '7', new v2i(-2, -3) },
			{ '8', new v2i(-1, -3) },
			{ '9', new v2i(0, -3) },
		};

		var dirKeypad = new Dictionary<char, v2i>()
		{
			{ 'A', v2i.Zero },
			{ '^', new v2i(-1, 0) },
			{ '<', new v2i(-2, 1) },
			{ 'v', new v2i(-1, 1) },
			{ '>', new v2i(0, 1) },
		};

		var codes = input.Select(x => x.ToArray()).ToArray();

		// part1
		var answer1 = codes.Sum(x =>
		{
			var seq = GetSequence(GetSequence(GetSequence(x, numKeypad), dirKeypad), dirKeypad);

			Debug.Line($"{new string(x)}: [{seq.Length}] {new string(seq)}");

			return GetComplexity(x, seq);
		});
		// TODO 2024/21 p1
		// 208196 too high

		// part2
		// TODO 2024/21 p2
		string answer2 = null;

		return (answer1.ToString(), answer2?.ToString());
	}

	static char[] GetSequence(char[] code, Dictionary<char, v2i> targetKeypad)
	{
		var current = v2i.Zero;

		var seq = new List<char>();
		for (var i = 0; i < code.Length; i++)
		{
			var next = targetKeypad[code[i]];

			var dir = next - current;
			var horiz = Enumerable.Repeat(dir.X < 0 ? v2i.Left : v2i.Right, (int)System.Math.Abs(dir.X));
			var vert = Enumerable.Repeat(dir.Y < 0 ? v2i.Up : v2i.Down, (int)System.Math.Abs(dir.Y));

			if (horiz.Any() && vert.Any())
			{
				// TODO decision point - one option will be efficient in future sequence
				//vert.Concat(horiz).ToArray();
				//horiz.Concat(vert).ToArray();
			}

			var path = vert.Concat(horiz).ToArray();

			seq.AddRange(StepByStep(path));
			seq.Add('A');

			current = next;
		}

		return [..seq];
	}

	static Dictionary<char, v2i> _dirKeypadBasic = new()
	{
		{ '^', new v2i(0, 1) },
		{ '<', new v2i(-1, 0) },
		{ 'v', new v2i(0, -1) },
		{ '>', new v2i(1, 0) },
	};

	static char[] StepByStep(v2i[] path) => path.Select(x => _dirKeypadBasic.Single(xx => xx.Value == x).Key).ToArray();

	static long GetComplexity(char[] code, char[] sequence) => sequence.Length * new string(code).ToNumArray().Single();
}