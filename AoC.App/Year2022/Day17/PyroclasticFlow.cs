using Ujeby.AoC.Common;

namespace Ujeby.AoC.App.Year2022.Day17
{
	public class PyroclasticFlow : ProblemBase
	{
		protected override (string, string) SolveProblem(string[] input)
		{
			var rocks = new char[][,]
			{
				new char[,]
				{
					{ '#', '#', '#', '#' }
				},
				new char[,]
				{
					{ '.', '#', '.' },
					{ '#', '#', '#' },
					{ '.', '#', '.' }
				},
				new char[,]
				{
					{ '.', '.', '#' },
					{ '.', '.', '#' },
					{ '#', '#', '#' }
				},
				new char[,]
				{
					{ '#' },
					{ '#' },
					{ '#' },
					{ '#' }
				},
				new char[,]
				{
					{ '#', '#' },
					{ '#', '#' }
				},
			};

			var chamber = new List<char[]>();

			// part1
			int answer1 = 0;
			var i = 0;
			for (var r = 0; r < 2022; r++)
			{
				var rock = rocks[r % rocks.GetLength(0)];

				var x = 2;
				var y = answer1 + 3;
				while (true)
				{
					int x1;
					if (input[0][i++ % input[0].Length] == '<')
						x1 = Math.Max(x - 1, 0);
					else
						x1 = Math.Min(x + 1, 7 - rock.GetLength(1));

					if (PlaceRock(rock, chamber, x1, y))
						x = x1;

					var y1 = y - 1;
					if (!PlaceRock(rock, chamber, x, y1))
						break;

					y = y1;
				}

				if (chamber.Count < y + rock.GetLength(1))
				{
					var toAdd = y + rock.GetLength(0) - chamber.Count;
					for (var ta = 0; ta < toAdd; ta++)
						chamber.Add(new char[] { '.', '.', '.', '.', '.', '.', '.' });
				}

				for (var yr = rock.GetLength(0) - 1; yr >= 0; yr--)
					for (var xr = 0; xr < rock.GetLength(1); xr++)
						chamber[yr + y][xr + x] = rock[yr, xr];

				answer1 = Math.Max(answer1, y + rock.GetLength(0));
				PrintChamber(chamber);
			}

			// part2
			long? answer2 = null;

			return (answer1.ToString(), answer2?.ToString());
		}

		private static void PrintChamber(List<char[]> chamber)
		{
			Debug.Line();
			for (var l = chamber.Count - 1; l >= 0; l--)
				Debug.Line(new string(chamber[l]));
		}

		private bool PlaceRock(char[,] rock, List<char[]> chamber, int x, int y)
		{
			if (y < 0)
				return false;

			for (var yr = 0; yr < rock.GetLength(0); yr++)
				for (var xr = 0; xr < rock.GetLength(1); xr++)
				{
					if (chamber[yr + y][xr + x] == '#' && rock[yr, xr] == '#')
						return false;
				}

			return true;
		}
	}
}
