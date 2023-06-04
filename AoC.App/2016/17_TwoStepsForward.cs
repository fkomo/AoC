using Ujeby.AoC.Common;
using Ujeby.Tools;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2016_17;

[AoCPuzzle(Year = 2016, Day = 17, Answer1 = "RDRDUDLRDR", Answer2 = "386", Skip = false)]
public class TwoStepsForward : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var passcode = input.Single();

		// part1
		var answer1 = FindPath(new v2i(0, 0), passcode);

		// part2
		var answer2 = FindPath(new v2i(0, 0), passcode, min: false).Length;

		return (answer1.ToString(), answer2.ToString());
	}

	/// <summary>
	/// up, down, left, right
	/// </summary>
	private static readonly char[] _pathDir = new char[] { 'U', 'D', 'L', 'R' };
	private static readonly char[] _open = new char[] { 'b', 'c', 'd', 'e', 'f' };

	private static v4i DoorsOpen(string hash)
		=> new(hash.Take(4).Select(x => (long)(_open.Contains(x) ? 1 : 0)).ToArray());

	private static string FindPath(v2i position, string passcode, 
		bool min = true, string path = null, string bestPath = null)
	{
		if (position == new v2i(3, 3))
			return path;

		var hash = Hashing.HashMd5(passcode + path);
		var doors = DoorsOpen(hash);

		for (var i = 0; i < v2i.UpDownLeftRight.Length; i++)
		{
			if (min && bestPath != null && bestPath?.Length == path?.Length - 1)
				break;

			// closed door
			if (doors[i] == 0)
				continue;

			var nextPos = position + v2i.UpDownLeftRight[i] * new v2i(1, -1); // inv up/down

			// wall
			if (nextPos.X < 0 || nextPos.Y < 0 || nextPos.X > 3 || nextPos.Y > 3)
				continue;

			var p = FindPath(nextPos, passcode,
				path: path + _pathDir[i],
				bestPath: bestPath,
				min: min);

			if (p == null)
				continue;
			
			if (bestPath == null)
				bestPath = p;

			else if (min && p.Length < bestPath.Length || !min && p.Length > bestPath.Length)
				bestPath = p;
		}

		return bestPath;
	}
}