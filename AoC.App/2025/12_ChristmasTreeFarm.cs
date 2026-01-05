using Ujeby.AoC.Common;
using Ujeby.Extensions;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2025_12;

[AoCPuzzle(Year = 2025, Day = 12, Answer1 = "448", Answer2 = "*", Skip = false)]
public class ChristmasTreeFarm : PuzzleBase
{
    protected override (string Part1, string Part2) SolvePuzzle(string[] input)
    {
        var presents = input
            .Split(string.Empty)[..^1]
            .Select(x => x[1..])
            .Select(x => x.Select(xx => xx.ToArray()).ToArray())
            .ToArray();

        var presentAreas = presents
            .Select(x => x.Sum(xx => xx.Count(xxx => xxx == '#')))
            .ToArray();

        var regions = input
            .Split(string.Empty)[^1]
            .Select(x => x.Split(':'))
            .Select(x => (
                size: new v2i(x[0].ToNumArray()),
                presCount: x[1].ToNumArray()))
            .ToArray();

        Debug.Line($"{regions.Length} regions");

        // part1
        var answer1 = 0L;
        foreach (var (size, presCount) in regions)
        {
            // area big enough to fit presents with no need to overlap
            if (size.X / 3 * size.Y / 3 >= presCount.Sum())
            {
                answer1++;
                continue;
            }

            // area too small to fit presents even if perfectly overlapped
            else if (size.Area() < presCount.Select((x, i) => x * presentAreas[i]).Sum())
                continue;

            else
                throw new NotImplementedException();
        }

        // part2
        string answer2 = "*";

        return (answer1.ToString(), answer2.ToString());
    }
}