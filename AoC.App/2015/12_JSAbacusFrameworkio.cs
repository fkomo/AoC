using Ujeby.AoC.Common;
using Ujeby.Tools.StringExtensions;

namespace Ujeby.AoC.App._2015_12
{
	[AoCPuzzle(Year = 2015, Day = 12, Answer1 = "111754", Answer2 = "65402")]
	public class JSAbacusFrameworkio : PuzzleBase
	{
		protected override (string Part1, string Part2) SolvePuzzle(string[] input)
		{
			// part1
			var answer1 = input.Single().ToNumArray().Sum();

			// part2
			var answer2 = SumJsonWithoutRed(input.Single());

			return (answer1.ToString(), answer2.ToString());
		}

		private static long SumJsonWithoutRed(string json)
		{
			var sum = 0L;
			var isArray = json[0] != '{';

			json = json[1..^1];
			for (var i = 0; i < json.Length; i++)
			{
				if (json[i] == ',')
					continue;

				int sIdx = i, eIdx;
				if (!isArray)
				{
					// object property name
					var propertyName = json[(i + 1)..json.IndexOf('\"', i + 1)];
					sIdx += propertyName.Length + 3; // 2x double qotes "" + :

					if (propertyName == "red" && !isArray)
						return 0;
				}

				// nested object/array				
				if (json[sIdx] == '{' || json[sIdx] == '[')
				{
					eIdx = json.IndexOfClosingBracket(sIdx);
					sum += SumJsonWithoutRed(json[sIdx..(eIdx + 1)]);
				}
				// value
				else
				{
					// string
					if (json[sIdx] == '\"')
					{
						eIdx = json.IndexOf('\"', sIdx + 1);
						var value = json[(sIdx+1)..eIdx];

						if (value == "red" && !isArray)
							return 0;
					}
					// number
					else
					{
						var n = string.Empty;
						if (json[sIdx] == '-')
						{
							n = "-";
							sIdx++;
						}

						for (eIdx = sIdx; eIdx < json.Length && char.IsDigit(json[eIdx]); eIdx++)
							n += json[eIdx];

						sum += int.Parse(n);	
					}
				}
				i = eIdx;
			}

			return sum;
		}
	}
}
