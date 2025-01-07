using Ujeby.AoC.Common;
using Ujeby.Extensions;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2024_18;

[AoCPuzzle(Year = 2024, Day = 18, Answer1 = "364", Answer2 = "52,28", Skip = false)]
public class RAMRun : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var bytes = input.Select(x => new v2i(x.ToNumArray())).ToArray();
		var size = new v2i(bytes.Length > 1000 ? 71 : 7);

		var start = v2i.Zero;
		var end = size - 1;
		var memSpace = new aab2i(start, end);

		// part1
		var answer1 = PathSteps(bytes.Take(1024), start, end, memSpace);

		// part2
		var byteCount = 0;
		var bytesToCheck = bytes.Length;
		while (bytesToCheck != 0)
		{
			var steps = PathSteps(bytes.Take(byteCount), start, end, memSpace);

			bytesToCheck /= 2;
			byteCount += steps > 0 ? bytesToCheck : -bytesToCheck;
		}
		var answer2 = input[byteCount];

		return (answer1.ToString(), answer2?.ToString());
	}

	static long PathSteps(IEnumerable<v2i> bytes, v2i start, v2i end, aab2i memorySpace)
	{
		var empty = memorySpace.EnumPoints().Except(bytes).ToArray();

		var neighbours = Enumerable.Range(0, empty.Length)
			.ToDictionary(
				x => x,
				x => v2i.UpDownLeftRight
					.Select(xx => Array.IndexOf(empty, empty[x] + xx))
					.Where(xx => xx != -1)
					.ToArray());

		int[] Neighbours(int node) => neighbours[node];

		 // step count is one less than path length
		return Ujeby.Alg.Graph.BreadthFirstSearch(Array.IndexOf(empty, start), Array.IndexOf(empty, end), Neighbours).Length - 1;
	}
}