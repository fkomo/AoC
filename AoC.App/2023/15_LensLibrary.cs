using System.Reflection.Emit;
using Ujeby.AoC.Common;

namespace Ujeby.AoC.App._2023_15;

[AoCPuzzle(Year = 2023, Day = 15, Answer1 = "506869", Answer2 = "271384", Skip = false)]
public class LensLibrary : PuzzleBase
{
	class Lens
	{
		public string Label { get; set; }
		public int FocalLength { get; set; }
		public override string ToString() => $"[{Label} {FocalLength}]";
	}

	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		// part1
		var answer1 = input
			.Single()
			.Split(',')
			.Sum(x => HASH(x));

		// part2
		var boxes = new Dictionary<byte, List<Lens>>();
		foreach (var step in input.Single().Split(','))
		{
			var label = step.Contains('-') ? step[..^1] : step[..^2];
			var boxId = HASH(label);
			if (!boxes.ContainsKey(boxId))
				boxes.Add(boxId, new List<Lens>());

			if (step.Contains('='))
			{
				if (!boxes[boxId].Any(x => x.Label == label))
					boxes[boxId].Add(new Lens { Label = label, FocalLength = step.Last() - '0' });
				else
				{
					for (var i = 0; i < boxes[boxId].Count; i++)
						if (boxes[boxId][i].Label == label)
						{
							boxes[boxId][i].FocalLength = step.Last() - '0';
							break;
						}
				}
			}
			else if (step.Contains('-'))
			{
				for (var i = 0; i < boxes[boxId].Count; i++)
					if (boxes[boxId][i].Label == label)
					{
						boxes[boxId].RemoveAt(i);
						break;
					}
			}
		}

#if DEBUG
		foreach (var box in boxes.Where(b => b.Value.Count > 0))
			Debug.Line($"{box.Key,3} {string.Join(" ", box.Value)}");
#endif
		var answer2 = boxes
			.Where(b => b.Value.Any())
			.Select(b => b.Value.Select((l, i) => (b.Key + 1) * (i + 1) * l.FocalLength).Sum())
			.Sum();

		return (answer1.ToString(), answer2.ToString());
	}

	static byte HASH(string s)
	{
		var hash = 0;
		foreach (var c in s)
		{
			hash += c;
			hash *= 17;
			hash %= 256;
		}

		return (byte)hash;
	}
}