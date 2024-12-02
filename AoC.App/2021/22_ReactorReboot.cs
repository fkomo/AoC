using System.Data;
using Ujeby.AoC.Common;
using Ujeby.Extensions;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2021_22
{
	[AoCPuzzle(Year = 2021, Day = 22, Answer1 = "591365", Answer2 = "1211172281877240")]
	public class ReactorReboot : PuzzleBase
	{
		internal record struct Cuboid(bool State, aab3i AABox)
		{
			public long Size() => State ? (AABox.Size + 1).Volume() : -(AABox.Size + 1).Volume();
			public override string ToString() => $"{(State ? "+" : "")}{Size()} {AABox}";
		}

		protected override (string, string) SolvePuzzle(string[] input)
		{
			// part1
			var target = new aab3i(new(-50), new(50));
			var cuboids50 = input.Select(line =>
				{
					var n = line.ToNumArray();
					var area = new aab3i(new(n[0], n[2], n[4]), new(n[1], n[3], n[5]));
					if (!target.Intersect(area))
						return new Cuboid { State = false, AABox = aab3i.Empty };

					return new Cuboid(
						line.StartsWith("on"),
						new aab3i(v3i.Clamp(area.Min, new(-50), new(50)) + 50, v3i.Clamp(area.Max, new(-50), new(50)) + 50));
				})
				.Where(c => c.AABox != aab3i.Empty)
				.ToArray();
			long? answer1 = RebootFinite(target.Size + 1, cuboids50);

			// part2
			var cuboids = input.Select(line =>
			{
				var n = line.ToNumArray();
				return new Cuboid(line.StartsWith("on"), new aab3i(new(n[0], n[2], n[4]), new(n[1], n[3], n[5])));
			}).ToArray();
			long? answer2 = RebootInfinite(cuboids);

			return (answer1?.ToString(), answer2?.ToString());
		}

		private static long RebootFinite(v3i reactorSize, Cuboid[] cuboids)
		{
			var offset = new v2i(reactorSize.X, reactorSize.X * reactorSize.Y);

			var result = new bool[reactorSize.Volume()];
			foreach (var cuboid in cuboids)
			{
				var c = cuboid.AABox.Min;
				for (c.Z = cuboid.AABox.Min.Z; c.Z <= cuboid.AABox.Max.Z; c.Z++)
					for (c.Y = cuboid.AABox.Min.Y; c.Y <= cuboid.AABox.Max.Y; c.Y++)
						for (c.X = cuboid.AABox.Min.X; c.X <= cuboid.AABox.Max.X; c.X++)
							result[c.Z * offset.Y + c.Y * offset.X + c.X] = cuboid.State;
			}

			return result.LongCount(c => c);
		}

		private static long RebootInfinite(Cuboid[] cuboids)
		{
			var result = 0L;

			var tmp = new List<Cuboid>();
			for (var icA = 0; icA < cuboids.Length; icA++)
			{
				var cA = cuboids[icA];
				for (var icB = tmp.Count - 1; icB >= 0; icB--)
				{
					var cB = tmp[icB];
					if (!cA.AABox.Overlaps(cB.AABox, out aab3i cAB))
						continue;

					if (cA.State)
					{
						var ccAB = new Cuboid(!cB.State, cAB);
						tmp.Add(ccAB);
						result += ccAB.Size();
					}
					else
					{
						var ccAB = new Cuboid(!cB.State, cAB);
						tmp.Add(ccAB);
						result += ccAB.Size();
					}
				}

				if (cA.State)
				{
					tmp.Add(cA);
					result += cA.Size();
				}
			}

			return result;
		}
	}
}
