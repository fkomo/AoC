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
		var idBlocks = new List<long>();
		for (var i = 0; i < diskMap.Length; i++)
		{
			if (diskMap[i] == 0)
				continue;

			idBlocks.AddRange(Enumerable.Repeat((i % 2 == 0) ? (long)i / 2 : _empty, diskMap[i]));
		}

		var compactLength = idBlocks.Count;
		var empty = idBlocks.IndexOf(_empty);
		for (var f = idBlocks.Count - 1; f >= 0; f--)
		{
			compactLength--;

			if (idBlocks[f] == _empty)
				continue;

			// swap empty block space with file block
			(idBlocks[empty], idBlocks[f]) = (idBlocks[f], idBlocks[empty]);

			empty = idBlocks.IndexOf(_empty, empty);
			if (empty >= f)
				break;
		}

		var answer1 = Checksum(idBlocks.Take(compactLength));

		// part2
		var position = 0L;
		var blocks = new List<v3i>();
		for (var i = 0; i < diskMap.Length; position += diskMap[i], i++)
		{
			if (diskMap[i] > 0)
				blocks.Add(new v3i(position, diskMap[i], i % 2 == 0 ? i / 2 : _empty));
		}

		for (var i = blocks.Count - 1; i >= 0; i--)
		{
			var block = blocks[i];
			if (block[_id] == _empty)
				continue;

			var e = FindFirstEmptyBlock(blocks, block[_size], i);
			if (e == -1)
				continue;

			// add empty block in place of old file
			blocks.Add(new v3i(block[_pos], block[_size], _empty));
			// move old file block to empty space
			blocks[i] += new v3i(blocks[e][_pos] - block[_pos], 0, 0);
			// cut from old empty block
			blocks[e] += new v3i(block[_size], - block[_size], 0);
		}

		var compact = new List<long>();
		var compactBlocks = blocks.OrderBy(x => x[_pos]).ToArray();
		foreach (var block in compactBlocks)
		{
			var id = block[_id] == _empty ? 0 : block[_id];
			compact.AddRange(Enumerable.Repeat(id, (int)block[_size]));
		}

		var answer2 = Checksum(compact);

		return (answer1.ToString(), answer2.ToString());
	}

	const int _empty = -1;
	const int _pos = 0;
	const int _size = 1;
	const int _id = 2;

	static int FindFirstEmptyBlock(List<v3i> blocks, long size, int maxIndex)
	{
		for (var e = 0; e < maxIndex; e++)
		{
			if (blocks[e][_id] != _empty)
				continue;

			var emptyBlock = blocks[e];

			if (emptyBlock[_size] < size)
				continue;

			return e;
		}

		return -1;
	}

	static long Checksum(IEnumerable<long> blocks) => blocks.Select((x, i) => x * i).Sum();
}