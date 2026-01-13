using Ujeby.AoC.Common;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2025_09;

[AoCPuzzle(Year = 2025, Day = 09, Answer1 = "4735222687", Answer2 = "1569262188", Skip = false)]
public class MovieTheater : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var red = input.Select(x => new v2i([.. x.Split(',').Select(x => long.Parse(x))])).ToArray();

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

        // build edges
        var edges = new List<v2i[]>();
		for (var i = 0; i < red.Length - 1; i++)
			edges.Add([red[i], red[i + 1]]);
        edges.Add([red[^1], red[0]]);

		// vertical edges
        var vEdges = edges
			.Where(x => x[0].X == x[1].X)
			.Select(x => x.OrderBy(xx => xx.Y).ToArray())
			.OrderBy(x => x[0].X)
			.ToArray();

        // horizontal edges
        var hEdges = edges
			.Where(x => x[0].Y == x[1].Y)
            .Select(x => x.OrderBy(xx => xx.X).ToArray())
            .OrderBy(x => x[0].Y)
			.ToArray();

        var answer2 = 0L;

        for (var r1 = 0; r1 < red.Length; r1++)
            for (var r2 = r1 + 2; r2 < red.Length; r2++)
			{
				var rect = new aab2i([red[r1], red[r2]]);

				// skip single lines
				if (rect.Size.X == 1 || rect.Size.Y == 1)
					continue;

                // check if any vertical edge intersects rectangle
                if (vEdges
					.Where(x => rect.Min.X < x[0].X && x[0].X < rect.Max.X)
					.Any(x => x[0].Y < rect.Max.Y && x[1].Y > rect.Min.Y))
					continue;

                // check if any horizontal edge intersects rectangle
                if (hEdges
                    .Where(x => rect.Min.Y < x[0].Y && x[0].Y < rect.Max.Y)
                    .Any(x => x[0].X < rect.Max.X && x[1].X > rect.Min.X))
                    continue;

                var area = rect.Size.Area();
                if (area > answer2)
                    answer2 = area;
            }

        return (answer1.ToString(), answer2.ToString());
	}
}