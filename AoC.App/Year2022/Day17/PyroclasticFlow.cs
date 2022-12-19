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
				{ '2',  '2', '2' },
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

		public const int _MIN_PATTERN_LENGTH = 1000;

		protected override (string, string) SolveProblem(string[] input)
		{
			// part1
			long? answer1 = FallingRocks(2022, input[0]);

			// part2
			long? answer2 = FallingRocks(1000000000000, input[0]);

			return (answer1.ToString(), answer2?.ToString());
		}

		public static long FallingRocks(long rockCount, string directions)
		{
			var chamber = new List<string>();

			var sb = new StringBuilder();
			var drops = new List<int>();
			var heightInc = new List<int>();

			var i = 0;
			var r = 0;
			for (long rc = 1; rc <= rockCount; rc++)
			{
				var rock = _rocks[r];
				r = (r + 1) % _rocks.GetLength(0);

				var x = 2;
				var y = chamber.Count + 3;

				var heightBefore = chamber.Count;
				while (true)
				{
					int x1;
					if (directions[i] == '<')
						x1 = Math.Max(x - 1, 0);
					else //if (directions[i] == '>')
						x1 = Math.Min(x + 1, 7 - rock.GetLength(1));

					i = (i + 1) % directions.Length;

					if (PlaceRock(rock, chamber, (x1, y)))
						x = x1;

					var y1 = y - 1;
					if (!PlaceRock(rock, chamber, (x, y1)))
						break;

					y = y1;
				}
				drops.Add(chamber.Count + 3 - y);

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

				heightInc.Add(chamber.Count - heightBefore);

				if (drops.Count >= _MIN_PATTERN_LENGTH * 2)
				{
					var patternFound = false;
					(int from, int to) pattern = (0, 0);

					for (var d0 = 0; d0 < drops.Count; d0++)
					{
						for (var d1 = d0 + _MIN_PATTERN_LENGTH; d1 < drops.Count; d1++)
						{
							if (drops[d0] != drops[d1])
								continue;

							var patternLength = d1 - d0;
							if (d1 + patternLength > drops.Count)
								continue;

							patternFound = true;
							for (var d2 = 1; d2 < patternLength; d2++)
							{
								if (drops[d0 + d2] != drops[d1 + d2])
								{
									patternFound = false;
									break;
								}
							}

							if (patternFound)
							{
								pattern.from = d0;
								pattern.to = d1;
								break;
							}
						}

						if (patternFound)
							break;
					}

					if (patternFound)
					{
						var beforePatternHeight = heightInc.Take(pattern.from).Sum();
						Debug.Line($"before-pattern count = {pattern.from}, height = {beforePatternHeight}");
						Debug.Line($"pattern {pattern.from}-{pattern.to}, length = {pattern.to - pattern.from}");

						var patternCount = (rockCount - pattern.from) / (pattern.to - pattern.from);
						var patternHeight = heightInc.Skip(pattern.from).Take(pattern.to - pattern.from).Sum();
						Debug.Line($"pattern height = {patternHeight} * count = {patternCount} => {patternHeight * patternCount}");

						var afterPatternCount = rockCount - patternCount * (pattern.to - pattern.from) - pattern.from;
						var afterPatternHeight = heightInc.Skip(pattern.from).Take((int)afterPatternCount).Sum();
						Debug.Line($"after-pattern count = {afterPatternCount}, height = {afterPatternHeight}");

						return beforePatternHeight + (patternHeight * patternCount) + afterPatternHeight;
					}
				}
			}

			return heightInc.Sum();
		}

		private static bool PlaceRock(char[,] rock, List<string> chamber, (int x, int y) pos)
		{
			if (pos.y < 0)
				return false;

			if (pos.y >= chamber.Count)
				return true;

			for (int ych = pos.y, yr = rock.GetLength(0) - 1; ych < chamber.Count && yr >= 0; yr--, ych++)
			{
				for (var xr = 0; xr < rock.GetLength(1); xr++)
				{
					if (chamber[ych][xr + pos.x] != Empty && rock[yr, xr] != Empty)
						return false;
				}
			}

			return true;
		}
	}
}
