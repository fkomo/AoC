using Ujeby.AoC.Common;
using Ujeby.Tools.StringExtensions;

namespace Ujeby.AoC.App._2016_04
{
	[AoCPuzzle(Year = 2016, Day = 04, Answer1 = "361724", Answer2 = "482")]
	public class SecurityThroughObscurity : PuzzleBase
	{
		public record struct Room(string Enc, int SectorId, string CheckSum)
		{
			internal class RoomIdComparer : IComparer<KeyValuePair<char, int>>
			{
				public int Compare(KeyValuePair<char, int> x, KeyValuePair<char, int> y)
				{
					if (x.Value > y.Value)
						return 1;

					if (x.Value < y.Value)
						return -1;

					if (x.Key < y.Key)
						return 1;

					if (x.Key > y.Key)
						return -1;

					return 0;
				}
			}

			public string Decrypt()
			{
				var dec = Enc.ToArray();

				for (var c = 0; c < dec.Length; c++)
				{
					if (dec[c] == '-')
						dec[c] = ' ';
					else
						for (var i = 0; i < SectorId; i++)
							if (++dec[c] > 'z')
								dec[c] = 'a';
				}

				return new string(dec);
			}

			public bool IsReal()
				=> CheckSum == new string(
					Enc.LettersOnly()
						.GroupBy(c => c)
						.ToDictionary(x => x.Key, x => x.Count())
						.OrderByDescending(x => x, new RoomIdComparer())
						.Take(5)
						.Select(x => x.Key)
						.ToArray()
					);
		}

		protected override (string Part1, string Part2) SolvePuzzle(string[] input)
		{
			var rooms = input.Select(x =>
				new Room(x[..x.LastIndexOf('-')], int.Parse(x[(x.LastIndexOf('-') + 1)..x.IndexOf('[')]), x[(x.IndexOf('[') + 1)..^1]))
				.ToArray();

			// part1
			var validRooms = rooms.Where(r => r.IsReal()).ToArray();
			var answer1 = validRooms.Sum(r => r.SectorId);

			// part2
			var answer2 = validRooms.Single(r => r.Decrypt().Contains("northpole")).SectorId;

			return (answer1.ToString(), answer2.ToString());
		}
	}
}
