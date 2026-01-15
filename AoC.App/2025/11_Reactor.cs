using Ujeby.AoC.Common;

namespace Ujeby.AoC.App._2025_11;

[AoCPuzzle(Year = 2025, Day = 11, Answer1 = "668", Answer2 = "294310962265680", Skip = false)]
public class Reactor : PuzzleBase
{
    protected override (string Part1, string Part2) SolvePuzzle(string[] input)
    {
        var devices = input.ToDictionary(x => x[..3], x => x[5..].Split(' '));

        // part1
        var answer1 = devices.AllPaths("you", "out");

        // part2
        long svr_fft = 0;
        long fft_dac = 0;
        long dac_out = 0;

        Parallel.Invoke(
            () => svr_fft = devices
                .CleanDeadEndNodesExcept("svr", "fft")
                .AllPaths("svr", "fft"),

            () => fft_dac = devices
                .CleanDeadEndNodesExcept("fft", "dac")
                .AllPaths("fft", "dac"),

            () => dac_out = devices
                .AllPaths("dac", "out")
        );

        // svr|...|fft|...|dac|...|out
        var answer2 = svr_fft * fft_dac * dac_out;

        return (answer1.ToString(), answer2.ToString());
    }
}

static class Extensions
{
    /// <summary>
    /// remove dead end nodes that are not needed
    /// </summary>
    /// <param name="devices"></param>
    /// <param name="except"></param>
    /// <returns></returns>
    public static Dictionary<string, string[]> CleanDeadEndNodesExcept(this Dictionary<string, string[]> devices, params string[] except)
    {
        while (true)
        {
            var deadEnds = devices
                .SelectMany(x => x.Value)
                .Distinct()
                .Where(x => !devices.ContainsKey(x))
                .Except(except)
                .ToArray();

            if (deadEnds.Length == 0)
                break;

            devices = devices
                .ToDictionary(x => x.Key, x => x.Value.Except(deadEnds).ToArray())
                .Where(x => x.Value.Length > 0)
                .ToDictionary();
        }

        return devices;
    }

    public static long AllPaths(this Dictionary<string, string[]> devices, string from, string to)
    {
        var paths = 0L;

        var stack = new Stack<List<string>>();
        stack.Push([from]);

        while (stack.Count > 0)
        {
            var path = stack.Pop();

            while (true)
            {
                var last = path.Last();
                if (last == to)
                {
                    paths++;
                    break;
                }

                if (!devices.ContainsKey(last))
                    break;

                var next = devices[last];

                if (next.Length == 1 && !path.Contains(next[0]))
                {
                    path.Add(next[0]);
                    continue;
                }

                foreach (var nextNode in next.Where(x => !path.Contains(x)))
                    stack.Push([.. path, nextNode]);

                break;
            }
        }

        return paths;
    }
}