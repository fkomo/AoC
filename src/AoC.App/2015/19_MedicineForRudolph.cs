using Ujeby.AoC.Common;

namespace Ujeby.AoC.App._2015_19
{
	[AoCPuzzle(Year = 2015, Day = 19, Answer1 = "518", Answer2 = "200")]
	public class MedicineForRudolph : PuzzleBase
	{
		protected override (string Part1, string Part2) SolvePuzzle(string[] input)
		{
			var replacements = input.Take(Array.IndexOf(input, ""))
				.GroupBy(i => i.Split(' ')[0])
				.ToDictionary(i => i.Key, i => i.Select(x => x.Split(' ')[2]).ToArray());
			var medicine = input[Array.IndexOf(input, "") + 1];

			// part1
			var allMolecules = new HashSet<string>();
			foreach (var r in replacements)
				foreach (var rv in r.Value)
				{
					for (var i = medicine.IndexOf(r.Key, 0); i != -1; i = medicine.IndexOf(r.Key, i + 1))
						allMolecules.Add(Replace(medicine, r.Key, rv, i));
				}
			var answer1 = allMolecules.Count;

			// part2
			var answer2 = 1L;
			var nonEReplacements = replacements.Where(x => x.Key != "e").ToArray();
			while (!replacements["e"].Any(x => x == medicine))
			{
				var replaced = false;
				foreach (var r in nonEReplacements)
				{
					foreach (var rv in r.Value)
					{
						if (!medicine.Contains(rv))
							continue;

						var i = medicine.IndexOf(rv);
						if (i == -1)
							continue;

						medicine = Replace(medicine, rv, r.Key, i);
						replaced = true;
						answer2++;
						break;
					}

					if (replaced)
						break;
				}
			}

			return (answer1.ToString(), answer2.ToString());
		}

		public static string Replace(string s, string old, string _new, int i)
			=> s[0..i] + _new + s[(i + old.Length)..];
	}
}
