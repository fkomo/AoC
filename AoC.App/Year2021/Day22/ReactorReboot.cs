using System.Data;
using Ujeby.AoC.Common;
using Ujeby.Tools;
using Ujeby.Vectors;

namespace Ujeby.AoC.App.Year2021.Day22
{
	public class ReactorReboot : PuzzleBase
	{
		public struct Cuboid
		{
			public bool TurnOn;
			public AABox3i Area;
			public long Size() => TurnOn ? (Area.Size + 1).Volume() : -(Area.Size + 1).Volume();

			public Cuboid(bool turnOn, AABox3i area)
			{
				TurnOn = turnOn;
				Area = area;
			}

			public override string ToString() => $"{(TurnOn ? "+" : "")}{Size()} {Area}";
		}

		protected override (string, string) SolvePuzzle(string[] input)
		{
			Debug.Line();

			// part1
			var target = new AABox3i(new(-50), new(50));
			var cuboids50 = input.Select(line =>
				{
					var n = line.ToNumArray();
					var area = new AABox3i(new(n[0], n[2], n[4]), new(n[1], n[3], n[5]));
					if (!target.Intersect(area))
						return new Cuboid { TurnOn = false, Area = AABox3i.Empty };

					return new Cuboid(
						line.StartsWith("on"),
						new AABox3i(v3i.Clamp(area.Min, new(-50), new(50)) + 50, v3i.Clamp(area.Max, new(-50), new(50)) + 50));
				})
				.Where(c => c.Area != AABox3i.Empty)
				.ToArray();

			Debug.Line($"part1: {cuboids50.Length} cuboids in {target}");

			var targetSize = target.Size + 1;
			var result = new bool[targetSize.Volume()];
			var offset = new v2i(targetSize.X, targetSize.X * targetSize.Y);
			foreach (var cuboid in cuboids50)
			{
				var c = cuboid.Area.Min;
				for (c.Z = cuboid.Area.Min.Z; c.Z <= cuboid.Area.Max.Z; c.Z++)
					for (c.Y = cuboid.Area.Min.Y; c.Y <= cuboid.Area.Max.Y; c.Y++)
						for (c.X = cuboid.Area.Min.X; c.X <= cuboid.Area.Max.X; c.X++)
							result[c.Z * offset.Y + c.Y * offset.X + c.X] = cuboid.TurnOn;
			}
			long? answer1 = result.Count(c => c);

			// part2
			var cuboids = input.Select(line =>
			{
				var n = line.ToNumArray();
				return new Cuboid(line.StartsWith("on"), new AABox3i(new(n[0], n[2], n[4]), new(n[1], n[3], n[5])));
			}).ToArray();
			Debug.Line($"part2: {cuboids.Length} cuboids");

			var cList = new List<Cuboid>();
			long? answer2 = 0;
			for (var icA = 0; icA < cuboids.Length; icA++)
			{
				var cA = cuboids[icA];
				for (var icB = cList.Count - 1; icB >= 0; icB--)
				{
					var cB = cList[icB];
					if (!cA.Area.Intersect(cB.Area))
						continue;

					var cAB = AABoxAndAABox(cA.Area, cB.Area);
					if (cA.TurnOn)
					{
						var ccAB = new Cuboid(!cB.TurnOn, cAB);
						cList.Add(ccAB);
						answer2 += ccAB.Size();
					}
					else
					{
						var ccAB = new Cuboid(!cB.TurnOn, cAB);
						cList.Add(ccAB);
						answer2 += ccAB.Size();
					}
				}

				if (cA.TurnOn)
				{
					cList.Add(cA);
					answer2 += cA.Size();
				}
			}

			Debug.Line();

			return (answer1?.ToString(), answer2?.ToString());
		}

		private static AABox3i AABoxAndAABox(AABox3i aab1, AABox3i aab2)
		{
			var x = new long[] { aab1.Min.X, aab1.Max.X, aab2.Min.X, aab2.Max.X }.OrderBy(i => i).ToArray();
			var y = new long[] { aab1.Min.Y, aab1.Max.Y, aab2.Min.Y, aab2.Max.Y }.OrderBy(i => i).ToArray();
			var z = new long[] { aab1.Min.Z, aab1.Max.Z, aab2.Min.Z, aab2.Max.Z }.OrderBy(i => i).ToArray();

			return new AABox3i(new(x[1], y[1], z[1]), new(x[2], y[2], z[2]));
		}
	}
}
