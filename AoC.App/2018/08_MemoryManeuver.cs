using Ujeby.AoC.Common;
using Ujeby.Extensions;

namespace Ujeby.AoC.App._2018_08;

record struct Node(int[] Childs, int[] Meta);

[AoCPuzzle(Year = 2018, Day = 08, Answer1 = "36027", Answer2 = null, Skip = false)]
public class MemoryManeuver : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var nums = input.Single().ToNumArray().Select(x => (int)x).ToArray();

		var licenseTree = new Dictionary<int, Node>();
		ProcessNode(nums, 0, licenseTree);

		// part1
		var answer1 = licenseTree.Sum(x => x.Value.Meta.Sum());

		// part2
		string answer2 = null;

		return (answer1.ToString(), answer2?.ToString());
	}

	static int ProcessNode(int[] licenseFile, int nodeId, Dictionary<int, Node> licenseTree)
	{
		var childs = new List<int>();

		var next = nodeId + 2;
		for (var i = 0; i < licenseFile[nodeId]; i++)
		{
			childs.Add(next);
			next = ProcessNode(licenseFile, next, licenseTree);
		}

		licenseTree.Add(nodeId, new Node([.. childs], licenseFile[next..(next + licenseFile[nodeId + 1])]));

		return next + licenseFile[nodeId + 1];
	}
}