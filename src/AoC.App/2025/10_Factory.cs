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
                Buttons: x[1..]
                    .Select(xx => AsXorPattern([.. xx.Trim(')', '(').Split(',').Select(xxx => int.Parse(xxx))], x[0].Length - 2))
                    .ToArray()))
            .AsParallel()
            .Sum(FewestButtonPressesToDiagram);

        // part2
        //var tmp = FewestButtonPressesToJoltage(
        //    input
        //        .Select(x => x.Split(' ', StringSplitOptions.RemoveEmptyEntries)[1..])
        //        .Select(x => (
        //            Buttons: x[..^1]
        //                .Select(xx => xx.Trim(')', '(').Split(',').Select(xxx => int.Parse(xxx)).ToArray())
        //                .OrderByDescending(x => x.Length)
        //                .ToArray(),
        //            Joltage: x[^1]
        //                .Trim('{', '}')
        //                .Split(',')
        //                .Select(xx => int.Parse(xx))
        //                .ToArray()))
        //        .ToArray()
        //        [1]);

        var machines = input
            .Select(x => x.Split(' ', StringSplitOptions.RemoveEmptyEntries)[1..])
            .Select(x => (
                Buttons: x[..^1]
                    .Select(xx => xx.Trim(')', '(').Split(',').Select(xxx => int.Parse(xxx)).ToArray())
                    .OrderByDescending(x => x.Length)
                    .ToArray(),
                Joltage: x[^1]
                    .Trim('{', '}')
                    .Split(',')
                    .Select(xx => int.Parse(xx))
                    .ToArray()));

        //Debug.Line($"buttons.count [{machines.Min(x => x.Buttons.Length)}..{machines.Max(x => x.Buttons.Length)}]~{machines.Average(x => x.Buttons.Length)}");
        //Debug.Line($"joltage.length [{machines.Min(x => x.Joltage.Length)}..{machines.Max(x => x.Joltage.Length)}]~{machines.Average(x => x.Joltage.Length)}");
        //Debug.Line($"joltage.values [{machines.SelectMany(x => x.Joltage).Min()}..{machines.SelectMany(x => x.Joltage).Max()}]~{machines.SelectMany(x => x.Joltage).Average()}");

        var answer2 = machines
            //.AsParallel()
            .Sum(FewestButtonPressesToJoltage);

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

        var minCount = long.MaxValue;

        while (queue.Count > 0)
        {
            var currentPath = queue.Dequeue();

            if (currentPath.Count >= minCount)
                continue;

            var lastStep = currentPath.Last();

            foreach (var transition in machine.Buttons)
            {
                var nextStep = lastStep ^ transition;

                if (nextStep == machine.Diagram)
                {
                    minCount = System.Math.Min(minCount, currentPath.Count);
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

        return minCount;
    }

    static long FewestButtonPressesToJoltage((int[][] Buttons, int[] Joltage) machine)
    {
        var buttons = Enumerable.Repeat(0, machine.Buttons.Length).ToArray();

        var buttonUsage = Enumerable.Range(0, machine.Joltage.Length).ToDictionary(x => x, x => machine.Buttons.Select((b, i) => (b, i)).Where(b => b.b.Contains(x)).ToArray()).ToArray();

        foreach (var counter in buttonUsage.Where(x => x.Value.Length == 1))
        {
            var button = counter.Value.Single();
            var btnPress = button.b.Min(x => machine.Joltage[x]);

            buttons[button.i] = btnPress;
        }

        Debug.Line($"{{{string.Join(',', machine.Joltage)}}}: [{string.Join(',', buttons.Select(x => (x == 0 ? "?" : x.ToString())))}]");

        return 0L;
    }

    static int[] PressBtn(int[] currentJoltage, int[] button, int count = 1)
    {
        var newJoltage = currentJoltage.ToArray();

        foreach (var cnt in button)
            newJoltage[cnt] -= count;

        return newJoltage;
    }
}
