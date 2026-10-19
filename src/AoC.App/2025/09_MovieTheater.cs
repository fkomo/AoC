using Ujeby.AoC.Common;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2025_09;

[AoCPuzzle(Year = 2025, Day = 09, Answer1 = "4735222687", Answer2 = "1569262188", Skip = false)]
public class MovieTheater : PuzzleBase
{
    protected override (string Part1, string Part2) SolvePuzzle(string[] input)
    {
        var red = ParseInput(input);

        // part1
        var answer1 = 0L;
        for (var r1 = 0; r1 < red.Length; r1++)
            for (var r2 = r1 + 1; r2 < red.Length; r2++)
            {
                var area = new aab2i([red[r1], red[r2]]).Size.Area();
                if (area > answer1)
                    answer1 = area;
            }

        // part2
        var answer2 = 0L;

        var edges = BuildEdges(red, out v2i[][] hEdges, out v2i[][] vEdges);

        foreach (var rect in EnumRectangles(red)
            .Where(x => CheckRectangle(x, hEdges, vEdges)))
        {
            var area = rect.Size.Area();
            if (area > answer2)
                answer2 = area;
        }

        return (answer1.ToString(), answer2.ToString());
    }

    public static v2i[] ParseInput(string[] input) =>
        input
            .Select(x => new v2i([.. x.Split(',').Select(x => long.Parse(x))]))
            .ToArray();

    public static v2i[][] BuildEdges(v2i[] points, out v2i[][] hEdges, out v2i[][] vEdges)
    {
        var edges = new List<v2i[]>();
        for (var i = 0; i < points.Length - 1; i++)
            edges.Add([points[i], points[i + 1]]);
        edges.Add([points[^1], points[0]]);

        // vertical edges
        vEdges = edges
            .Where(x => x[0].X == x[1].X)
            .Select(x => x.OrderBy(xx => xx.Y).ToArray())
            .OrderBy(x => x[0].X)
            .ToArray();

        // horizontal edges
        hEdges = edges
            .Where(x => x[0].Y == x[1].Y)
            .Select(x => x.OrderBy(xx => xx.X).ToArray())
            .OrderBy(x => x[0].Y)
            .ToArray();

        return edges.ToArray();
    }

    public static IEnumerable<aab2i> EnumRectangles(v2i[] points)
    {
        for (var r1 = 0; r1 < points.Length; r1++)
            for (var r2 = r1 + 2; r2 < points.Length; r2++)
                yield return new aab2i([points[r1], points[r2]]);
    }

    public static bool CheckRectangle(aab2i rect, v2i[][] hEdges, v2i[][] vEdges)
    {
        // skip single lines
        if (rect.Size.X == 1 || rect.Size.Y == 1)
            return false;

        // check if any vertical edge intersects rectangle
        if (vEdges
            .Where(x => rect.Min.X < x[0].X && x[0].X < rect.Max.X)
            .Any(x => x[0].Y < rect.Max.Y && x[1].Y > rect.Min.Y))
            return false;

        // check if any horizontal edge intersects rectangle
        if (hEdges
            .Where(x => rect.Min.Y < x[0].Y && x[0].Y < rect.Max.Y)
            .Any(x => x[0].X < rect.Max.X && x[1].X > rect.Min.X))
            return false;

        return true;
    }
}