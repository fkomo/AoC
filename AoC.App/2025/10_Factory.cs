using Ujeby.AoC.Common;

namespace Ujeby.AoC.App._2025_10;

[AoCPuzzle(Year = 2025, Day = 10, Answer1 = "438", Answer2 = null, Skip = false)]
public class Factory : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
        // part1
        var answer1 = input
            .Select(x => x.Split(' ', StringSplitOptions.RemoveEmptyEntries)[..^1])
            .Select(x => (
                Diagram: Math.BaseToDec(x[0].Trim('[', ']'), baseString: ".#"),
                Buttons: x[1..].Select(xx => AsXorPattern([.. xx.Trim(')', '(').Split(',').Select(xxx => int.Parse(xxx))], x[0].Length - 2)).ToArray()))
            .AsParallel()
            .Sum(FewestButtonPressesToDiagram);

        // part2
        var machines = input
            .Select(x => x.Split(' ', StringSplitOptions.RemoveEmptyEntries)[1..])
            .Select(x => (
                Buttons: x[..^1].Select(xx => xx.Trim(')', '(').Split(',').Select(xxx => int.Parse(xxx)).ToArray()).ToList(),
                Joltage: x[^1].Trim('{', '}').Split(',').Select(xx => int.Parse(xx)).ToArray()))
            .ToArray();

        Debug.Line($"{machines.Length} machines with {machines.Sum(x => x.Buttons.Count)} buttons");



        var answer2 = 0L;

		return (answer1.ToString(), answer2.ToString());
	}

    static long AsXorPattern(int[] bits, int patternLength)
    {
		var pattern = 0L;
		foreach (var bit in bits.Select(x => patternLength - 1 - x))
			pattern |= 1L << bit;

		return pattern;
    }

    static long FewestButtonPressesToDiagram((long Diagram, long[] Buttons) machine)
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

            foreach (var transition in machine.Buttons)
			{
				var nextStep = lastStep ^ transition;

                if (nextStep == machine.Diagram)
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