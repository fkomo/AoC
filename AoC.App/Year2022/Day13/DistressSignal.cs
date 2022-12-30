using Ujeby.AoC.Common;

namespace Ujeby.AoC.App.Year2022.Day13
{
	public class DistressSignal : PuzzleBase
	{
		protected override (string, string) SolvePuzzle(string[] input)
		{
			var packetCmp = new PacketComparer();

			// part1
			long? answer1 = 0;
			for (var i = 0; i < input.Length; i += 3)
			{
				var result = packetCmp.Compare(input[i], input[i + 1]);
				if (result == -1)
					answer1 += (i + 3) / 3;
			}

			// part2
			var orderedPackets = input.Where(l => l.Length > 0).Concat(new[] { "[[2]]", "[[6]]" })
				.OrderBy(p => p, packetCmp)
				.ToArray();
			long? answer2 = (Array.IndexOf(orderedPackets, "[[2]]") + 1) * (Array.IndexOf(orderedPackets, "[[6]]") + 1);

			return (answer1?.ToString(), answer2?.ToString());
		}

		internal class PacketComparer : IComparer<string>
		{
			public int Compare(string x, string y) => CompareArray(x, y);

			private int CompareArray(string array1, string array2)
			{
				if (array1 == "[]" && array2.Length > 2)
					return -1;
				else if (array2 == "[]" && array1.Length > 2)
					return 1;
				else if (array1 == "[]" && array2 == "[]")
					return 0;

				var p1Values = SplitArray(array1[1..^1]);
				var p2Values = SplitArray(array2[1..^1]);

				if (p1Values.Length == 1 && p2Values.Length == 1 &&
					int.TryParse(p1Values.Single(), out int int1) && int.TryParse(p2Values.Single(), out int int2))
				{
					if (int1 > int2)
						return 1;

					else if (int1 < int2)
						return -1;

					return 0;
				}

				for (var i = 0; i < p1Values.Length; i++)
				{
					var arr1 = p1Values[i];
					if (arr1.Length == 0 || arr1[0] != '[')
						arr1 = $"[{arr1}]";

					if (p2Values.Length == i)
						return 1;

					var arr2 = p2Values[i];
					if (arr2.Length == 0 || arr2[0] != '[')
						arr2 = $"[{arr2}]";

					var result = CompareArray(arr1, arr2);

					if (result != 0)
						return result;
				}

				if (p1Values.Length < p2Values.Length)
					return -1;

				return 0;
			}

			private static string[] SplitArray(string raw)
			{
				if (raw.All(c => c == ',' || char.IsDigit(c)))
					return raw.Split(',');

				var result = new List<string>();

				string item = null;
				for (var i = 0; i < raw.Length; i++)
				{
					if (raw[i] == '[')
					{
						var c = 1;
						var i2 = i + 1;
						while (c > 0)
						{
							if (raw[i2] == '[')
								c++;
							else if (raw[i2] == ']')
								c--;

							i2++;
						}

						result.Add(raw.Substring(i, i2 - i));
						i = i2 - 1;
					}
					else
					{
						if (raw[i] == ',')
						{
							if (item != null)
								result.Add(item);

							item = null;
						}
						else
							item += raw[i];
					}
				}

				if (item != null)
					result.Add(item);

				return result.ToArray();
			}
		}
	}
}
