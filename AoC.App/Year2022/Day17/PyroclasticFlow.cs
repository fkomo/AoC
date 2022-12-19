using System.Text;
using Ujeby.AoC.Common;

namespace Ujeby.AoC.App.Year2022.Day17
{
	public class PyroclasticFlow : ProblemBase
	{
		public static char Empty = '.';
		private static readonly string EmptyLine = new(Enumerable.Repeat(Empty, 7).ToArray());

		private static char[][,] _rocks = new char[][,]
		{
			new char[,]
			{
				{ '1', '1', '1', '1' }
			},
			new char[,]
			{
				{ Empty, '2', Empty },
				{ '2', '2', '2' },
				{ Empty, '2', Empty }
			},
			new char[,]
			{
				{ Empty, Empty, '3' },
				{ Empty, Empty, '3' },
				{ '3', '3', '3' }
			},
			new char[,]
			{
				{ '4' },
				{ '4' },
				{ '4' },
				{ '4' }
			},
			new char[,]
			{
				{ '5', '5' },
				{ '5', '5' }
			},
		};

		protected override (string, string) SolveProblem(string[] input)
		{
			// part1
			long? answer1 = FallingRocks(2022, input[0])
				.Count;

			// part2
			long? answer2 = null;

			return (answer1.ToString(), answer2?.ToString());
		}

		public static List<string> FallingRocks(long rockCount, string directions)
		{
			var chamber = new List<string>();

			var sb = new StringBuilder();

			var i = 0;
			for (int r = 0; r < rockCount; r++)
			{
				var rock = _rocks[r % _rocks.GetLength(0)];

				var x = 2;
				var y = chamber.Count + 3;
				while (true)
				{
					int x1;
					if (directions[i] == '<')
						x1 = Math.Max(x - 1, 0);
					else //if (directions[i] == '>')
						x1 = Math.Min(x + 1, 7 - rock.GetLength(1));

					i = (i + 1) % directions.Length;

					if (PlaceRock(rock, chamber, x1, y))
						x = x1;

					var y1 = y - 1;
					if (!PlaceRock(rock, chamber, x, y1))
						break;

					y = y1;
				}

				// add placed rock to chamber
				if (chamber.Count < y + rock.GetLength(0))
				{
					var toAdd = y + rock.GetLength(0) - chamber.Count;
					for (var ta = 0; ta < toAdd; ta++)
						chamber.Add(EmptyLine);
				}
				for (int ych = y, yr = rock.GetLength(0) - 1; yr >= 0; yr--, ych++)
				{
					sb.Clear();
					sb.Append(chamber[ych]);
					for (var xr = 0; xr < rock.GetLength(1); xr++)
					{
						if (rock[yr, xr] != Empty)
							sb[xr + x] = rock[yr, xr];
					}

					chamber[ych] = sb.ToString();
				}

				//PrintChamber(chamber, r);
			}

			return chamber;
		}

		private static void PrintChamber(List<string> chamber, int rock)
		{
			Debug.Line($"{(rock + 1),4}/2022, height={chamber.Count}");
			for (int l = chamber.Count - 1; l >= 0; l--)
				Debug.Line(chamber[l]);
			Debug.Line();
		}

		private static bool PlaceRock(char[,] rock, List<string> chamber, int x, int y)
		{
			if (y < 0)
				return false;

			if (y >= chamber.Count)
				return true;

			for (int ych = y, yr = rock.GetLength(0) - 1; ych < chamber.Count && yr >= 0; yr--, ych++)
				for (var xr = 0; xr < rock.GetLength(1); xr++)
				{
					if (chamber[ych][xr + x] != Empty && rock[yr, xr] != Empty)
						return false;
				}

			return true;
		}
	}
}
