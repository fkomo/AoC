using Ujeby.AoC.Common;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2024_09;

[AoCPuzzle(Year = 2024, Day = 09, Answer1 = "6334655979668", Answer2 = "6349492251099", Skip = false)]
public class DiskFragmenter : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var diskMap = input.Single().Select(x => (byte)(x - '0')).ToArray();

		// part1
		var blockIds = new List<long>();
		for (var i = 0; i < diskMap.Length; i++)
		{
			if (diskMap[i] == 0)
				continue;

			blockIds.AddRange(Enumerable.Repeat((i % 2 == 0) ? (long)i / 2 : _emptyId, diskMap[i]));
		}

		var compactLength = blockIds.Count;
		var empty = blockIds.IndexOf(_emptyId);
		for (var i = blockIds.Count - 1; i >= 0; i--)
		{
			compactLength--;

			if (blockIds[i] == _emptyId)
				continue;

			// swap empty block space with file block
			(blockIds[empty], blockIds[i]) = (blockIds[i], blockIds[empty]);

			empty = blockIds.IndexOf(_emptyId, empty);
			if (empty >= i)
				break;
		}

		var answer1 = Checksum(blockIds.Take(compactLength));

		// part2
		var position = 0L;
		var blocks = new List<v3i>();
		var emptyBlocks = new List<v2i>();
		for (var i = 0; i < diskMap.Length; position += diskMap[i], i++)
		{
			if (diskMap[i] == 0)
				continue;

			if (i % 2 == 0)
				blocks.Add(new v3i(position, diskMap[i], i / 2));
			else
				emptyBlocks.Add(new v2i(position, diskMap[i]));
		}

		for (var i = blocks.Count - 1; i >= 0; i--)
		{
			var block = blocks[i];

			var e = IndexOfFirstEmpty(emptyBlocks, block[_size], block[_pos]);
			if (e == -1)
				continue;

			// add empty block in place of old file
			emptyBlocks.Add(new v2i(block[_pos], block[_size]));
			// move old file block to empty space
			blocks[i] += new v3i(emptyBlocks[e][_pos] - block[_pos], 0, 0);
			// cut from old empty block
			emptyBlocks[e] += new v2i(block[_size], - block[_size]);
		}

		var compact = blocks
			.Concat(emptyBlocks.Select(x => new v3i(x, 0)))
			.OrderBy(x => x[_pos])
			.SelectMany(x => Enumerable.Repeat(x[_id], (int)x[_size]))
			.ToArray();

		var answer2 = Checksum(compact);

		return (answer1.ToString(), answer2.ToString());
	}

	const int _emptyId = -1;
	const int _pos = 0;
	const int _size = 1;
	const int _id = 2;

	static int IndexOfFirstEmpty(List<v2i> emptyBlocks, long size, long fileBlockPosition)
	{
		for (var e = 0; e < emptyBlocks.Count; e++)
		{
			var emptyBlock = emptyBlocks[e];
			if (emptyBlock[_size] < size)
				continue;

			return emptyBlock[_pos] < fileBlockPosition ? e : -1;
		}

		return -1;
	}

	static long Checksum(IEnumerable<long> blocks) => blocks.Select((x, i) => x * i).Sum();
}