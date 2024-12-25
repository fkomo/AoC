using Ujeby.AoC.Common;
using Ujeby.Extensions;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2024_21;

[AoCPuzzle(Year = 2024, Day = 21, Answer1 = null, Answer2 = null, Skip = false)]
public class KeypadConundrum : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var codes = input.Select(x => x.ToArray()).ToArray();

		// part1
		var answer1 = codes.Sum(x =>
		{
			var seq = GetMinSequence(x);

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

	static readonly Dictionary<string, char[]> _cache = [];

	static char[] GetMinSequence(char[] code)
	{
		_cache.Clear();

		char[] best = null;

		var queue = new Queue<SeqState>();
		queue.Enqueue(new(code));

		while (queue.Count > 0)
		{
			var state = queue.Dequeue();

			if (state.Stage == 0)
			{
				var seq = GetSequence(state, _numKeypad, queue);
				if (seq == null)
					continue;

				state = new(seq, Stage: 1);
			}

			if (state.Stage == 1)
			{
				var seq = GetSequence(state, _dirKeypad, queue);
				if (seq == null)
					continue;

				state = new(seq, Stage: 2);
			}

			if (state.Stage == 2)
			{
				var seq = GetSequence(state, _dirKeypad, queue);
				if (seq == null)
					continue;

				if (best == null || best.Length > seq.Length)
					best = [.. seq];
			}
		}

		return best;
	}

	static char[] GetSequence(SeqState state, Dictionary<char, v2i> keypad, Queue<SeqState> queue)
	{
		if (_cache.TryGetValue(state.ToString(), out char[] result))
			return result;

		var current = v2i.Zero;
		var seq = state.Sequence?.ToList() ?? [];

		if (state.NextPath != null)
		{
			seq.AddRange(StepByStep(state.NextPath));
			seq.Add('A');

			current = keypad[state.Input[state.InputIdx]];
		}
		
		for (var i = state.InputIdx + 1; i < state.Input.Length; i++)
		{
			var next = keypad[state.Input[i]];

			var dir = next - current;
			if (dir != v2i.Zero)
			{
				var h = Enumerable.Repeat(dir.X < 0 ? v2i.Left : v2i.Right, (int)System.Math.Abs(dir.X));
				var v = Enumerable.Repeat(dir.Y < 0 ? v2i.Up : v2i.Down, (int)System.Math.Abs(dir.Y));

				var path = h.Concat(v).ToArray();
				if (path.Length > 1)
				{
					var pathsTaken = new HashSet<string>();
					var allPaths = Ujeby.Alg.Combinatorics.Permutations(Enumerable.Range(0, path.Length), path.Length);
					foreach (var pIds in allPaths)
					{
						var p = pIds.Select(x => path[x]).ToArray();

						if (!pathsTaken.Add(string.Join('#', p.Select(x => $"{x.X},{x.Y}"))))
							continue;

						// check if path is legal
						if (IlegalPath(p, current))
							continue;

						queue.Enqueue(new(state.Input, InputIdx: i, Stage: state.Stage, Sequence: [.. seq], NextPath: p, CurrentPos: current));
					}

					return null;
				}

				seq.AddRange(StepByStep(path));
			}

			seq.Add('A');
			current = next;
		}

		result = [.. seq];

		_cache.Add(state.ToString(), result);

		return result;
	}

	static bool IlegalPath(v2i[] path, v2i current)
	{
		var illegal = new v2i(-2, 0);
		for (var i = 0; i < path.Length; i++)
		{
			current += path[i];
			if (current == illegal)
				return true;
		}

		return false;
	}

	record struct SeqState(char[] Input, int InputIdx = -1, int Stage = 0, char[] Sequence = null, v2i[] NextPath = null, v2i? CurrentPos = null)
	{
		public override readonly string ToString() =>
			$"{Stage}#{InputIdx}";
	}

	static Dictionary<char, v2i> _numKeypad = new()
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

	static Dictionary<char, v2i> _dirKeypad = new()
	{
		{ 'A', v2i.Zero },
		{ '^', new v2i(-1, 0) },
		{ '<', new v2i(-2, 1) },
		{ 'v', new v2i(-1, 1) },
		{ '>', new v2i(0, 1) },
	};

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