using Ujeby.AoC.Common;
using Ujeby.Extensions;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2024_13;

[AoCPuzzle(Year = 2024, Day = 13, Answer1 = "37297", Answer2 = "83197086729371", Skip = false)]
public class ClawContraption : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var machines = input
			.Split(string.Empty)
			.Select(x => new Machine(new v2i(x[0].ToNumArray()), new v2i(x[1].ToNumArray()), new v2i(x[2].ToNumArray())))
			.ToArray();

		// part1
		var answer1 = machines
			.Select(IsWinning)
			.Where(x => x != null)
			.Sum(x => x.Value.X * 3 + x.Value.Y);

		// part2
		var answer2 = machines
			.Select(x => x with { Prize = x.Prize + 10_000_000_000_000 })
			.Select(IsWinning)
			.Where(x => x != null)
			.Sum(x => x.Value.X * 3 + x.Value.Y);

		return (answer1.ToString(), answer2.ToString());
	}

	static v2i? IsWinning(Machine m)
	{
		var b = (m.Prize.Y * m.A.X - m.Prize.X * m.A.Y) / (m.A.X * m.B.Y - m.B.X * m.A.Y);
		var a = (m.Prize.X - b*m.B.X) / m.A.X;

		if (m.A * a + m.B * b != m.Prize)
			return null;

		return new v2i(a, b);
	}

	public record struct Machine(v2i A, v2i B, v2i Prize);
}