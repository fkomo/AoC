using Ujeby.AoC.Common;
using Ujeby.Tools;
using Ujeby.Vectors;

namespace Ujeby.AoC.App.Year2021.Day22
{
	public class ReactorReboot : PuzzleBase
	{
		protected override (string, string) SolvePuzzle(string[] input)
		{
			// part1
			var target = new AABox3i(new(-50), new(50));
			var cuboids = input.Select(line =>
			{
				var n = line.ToNumArray();
				var area = new AABox3i(new(n[0], n[2], n[4]), new(n[1], n[3], n[5]));
				if (!target.Intersect(area))
					return (TurnOn: false, Area: AABox3i.Empty);

				return (
					TurnOn: line.StartsWith("on"),
					Area: new AABox3i(v3i.Clamp(area.Min, new(-50), new(50)) + 50, v3i.Clamp(area.Max, new(-50), new(50)) + 50));
			})
				.Where(c => c.Area != AABox3i.Empty)
				.ToArray();

			var targetSize = target.Size + 1;
			var result = new bool[targetSize.Volume()];
			var offset = new v2i(targetSize.X, targetSize.X * targetSize.Y);
			foreach (var (turnOn, aab) in cuboids)
				Turn(turnOn, aab, offset, result);
			long? answer1 = result.Count(c => c);

			// part2
			long? answer2 = null;

			return (answer1?.ToString(), answer2?.ToString());
		}

		private static void Turn(bool turnOn, AABox3i aab, v2i offset, bool[] result)
		{
			var c = aab.Min;
			for (c.Z = aab.Min.Z; c.Z <= aab.Max.Z; c.Z++)
				for (c.Y = aab.Min.Y; c.Y <= aab.Max.Y; c.Y++)
					for (c.X = aab.Min.X; c.X <= aab.Max.X; c.X++)
						result[c.Z * offset.Y + c.Y * offset.X + c.X] = turnOn;
		}
	}
}
