using Ujeby.AoC.Common;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2023_21;

[AoCPuzzle(Year = 2023, Day = 21, Answer1 = "3751", Answer2 = null, Skip = false)]
public class StepCounter : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var start = GetStart(input);

		// part1
		var plots0 = new HashSet<v2i>() { start };
		var plots1 = new HashSet<v2i>();
		for (var i = 0; i <= 64; i++)
		{
			plots1.Clear();
			foreach (var p0 in plots0)
			{
				foreach (var dir in v2i.UpDownLeftRight)
				{
					var p1 = p0 + dir;
					if (p1.X < 0 || p1.Y < 0 || p1.X >= input.Length || p1.Y >= input.Length)
						continue;

					if (input[p1.Y][(int)p1.X] == '#')
						continue;

					plots1.Add(p1);
				}
			}

			(plots1, plots0) = (plots0, plots1);
		}
		var answer1 = plots1.Count;

		// part2
		long? answer2 = null;

		// TODO 2023/21 p2
		//plots0 = new HashSet<v2i>() { start };
		//for (var i = 0; i <= 1000; i++)
		//{
		//	var tmpPlots = Step(plots0, input);
		//	Debug.Line($"{i+1},{tmpPlots.Count},{tmpPlots.Count - plots0.Count}");

		//	plots0 = tmpPlots;
		//}

		return (answer1.ToString(), answer2.ToString());
	}

	public static v2i GetStart(string[] input)
	{
		for (var y = 0; y < input.Length; y++)
			for (var x = 0; x < input.Length; x++)
				if (input[y][x] == 'S')
					return new v2i(x, y);

		return v2i.Zero;
	}

	public static HashSet<v2i> Step(HashSet<v2i> plots0, string[] input)
	{
		var plots1 = new HashSet<v2i>();

		foreach (var p0 in plots0)
		{
			foreach (var dir in v2i.UpDownLeftRight)
			{
				var p1 = p0 + dir;
				var x = System.Math.Abs(System.Math.Min(p1.X, p1.Y) / input.Length) + 1;
				var pMod = (p0 + dir + (new v2i(input.Length) * x)) % new v2i(input.Length);
				if (input[pMod.Y][(int)pMod.X] == '#')
					continue;

				plots1.Add(p1);
			}
		}

		return plots1;
	}
}