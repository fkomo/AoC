using Ujeby.AoC.Common;

namespace Ujeby.AoC.App._2025_10;

[AoCPuzzle(Year = 2025, Day = 10, Answer1 = "438", Answer2 = null, Skip = false)]
public class Factory : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var machines = input
			.Select(x => x.Split(' ', StringSplitOptions.RemoveEmptyEntries))
			.Select(x => (
                Final: Math.BaseToDec(x[0].Trim('[', ']'), baseString: ".#"),
                Transitions: x[1..^1].Select(xx => AsXorPattern([.. xx.Trim(')', '(').Split(',').Select(xxx => int.Parse(xxx))], x[0].Length - 2)).ToArray(),
				Joltage: x[^1].Trim('{', '}').Split(',').Select(xx => int.Parse(xx)).ToArray()))
			.ToArray();

        // part1
        var answer1 = machines.AsParallel().Sum(MinButtonPresses);

		// part2
		string answer2 = null;

		return (answer1.ToString(), answer2?.ToString());
	}

    static long AsXorPattern(int[] bits, int patternLength)
    {
		var pattern = 0L;
		foreach (var bit in bits.Select(x => patternLength - 1 - x))
			pattern |= 1L << bit;

		return pattern;
    }

    static long MinButtonPresses((long Final, long[] Transitions, int[] Joltage) machine)
    {
		var queue = new Queue<HashSet<long>>();
		queue.Enqueue([0L]);

		var minSteps = long.MaxValue;

		while (queue.Count > 0)
		{
			var currentPath = queue.Dequeue();

			if (currentPath.Count >= minSteps)
				continue;

            var lastStep = currentPath.Last();

            foreach (var transition in machine.Transitions)
			{
				var nextStep = lastStep ^ transition;

                if (nextStep == machine.Final)
                {
                    minSteps = System.Math.Min(minSteps, currentPath.Count);
                    continue;
                }

                if (currentPath.Contains(nextStep))
					continue;

				if (queue.Any(x => x.Contains(nextStep)))
					continue;

                var nextPath = currentPath.ToHashSet();
				nextPath.Add(nextStep);

				queue.Enqueue(nextPath);
            }
        }

        return minSteps;
    }
}