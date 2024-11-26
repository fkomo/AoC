using Ujeby.AoC.Common;

namespace Ujeby.AoC.App._2018_05;

[AoCPuzzle(Year = 2018, Day = 05, Answer1 = "9116", Answer2 = "6890", Skip = false)]
public class AlchemicalReduction : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var polymer = input.Single().ToCharArray();

		// part1
		var answer1 = React(polymer);

		// part2
		polymer = polymer.Where(x => x != 0).ToArray();
		var answer2 = Enumerable.Range('a', 'z' - 'a')
			.Min(x => React(polymer.Where(xx => char.ToLower(xx) != (char)x).ToArray()));

		return (answer1.ToString(), answer2.ToString());
	}

	static int React(char[] polymer)
	{
		var polymerLength = polymer.Length;

		bool change;
		do
		{
			change = false;

			for (var i1 = 0; i1 < polymer.Length; i1++)
			{
				for (; i1 < polymer.Length && polymer[i1] == 0; i1++)
				{
				}

				if (i1 >= polymer.Length)
					break;

				var i2 = i1 + 1;
				for (; i2 < polymer.Length && polymer[i2] == 0; i2++)
				{
				}

				if (i2 >= polymer.Length)
					break;

				if (!CheckReaction(polymer[i1], polymer[i2]))
					continue;

				change = true;

				polymer[i1] = (char)0;
				polymer[i2] = (char)0;

				i1 = i2 - 1;
				polymerLength -= 2;
			}
		}
		while (change);

		return polymerLength;
	}

	static bool CheckReaction(char a, char b) => System.Math.Abs(a - b) == ('a' - 'A');
}