using Ujeby.AoC.Common;
using Ujeby.Extensions;

namespace Ujeby.AoC.App._2018_08;

using LicenseTree = Dictionary<int, LicenseTreeNode>;

record struct LicenseTreeNode(int[] Childs, int[] Meta);

[AoCPuzzle(Year = 2018, Day = 08, Answer1 = "36027", Answer2 = "23960", Skip = false)]
public class MemoryManeuver : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var licenseFile = input.Single().ToNumArray().Select(x => (int)x).ToArray();

		var licenseTree = new LicenseTree();
		licenseTree.ProcessNode(licenseFile, 0);

		// part1
		var answer1 = licenseTree.Sum(x => x.Value.Meta.Sum());

		// part2
		var answer2 = licenseTree.GetNodeValue(0);

		return (answer1.ToString(), answer2.ToString());
	}
}

static class Extensions
{
	public static int ProcessNode(this LicenseTree licenseTree, int[] licenseFile, int nodeId)
	{
		var childs = new List<int>();

		var next = nodeId + 2;
		for (var i = 0; i < licenseFile[nodeId]; i++)
		{
			childs.Add(next);
			next = licenseTree.ProcessNode(licenseFile, next);
		}

		licenseTree.Add(nodeId, new LicenseTreeNode([.. childs], licenseFile[next..(next + licenseFile[nodeId + 1])]));

		return next + licenseFile[nodeId + 1];
	}

	public static long GetNodeValue(this LicenseTree tree, int nodeId)
	{
		if (!tree.TryGetValue(nodeId, out LicenseTreeNode node))
			return 0;

		if (node.Childs.Length == 0)
			return node.Meta.Sum();

		return node.Meta
			.Where(x => x > 0 && x <= node.Childs.Length)
			.Select(x => tree.GetNodeValue(node.Childs[x - 1]))
			.Sum();
	}
}