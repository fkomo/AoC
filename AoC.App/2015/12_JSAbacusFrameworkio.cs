using System.Security.Cryptography;
using System.Text;
using Ujeby.AoC.Common;
using Ujeby.Tools.StringExtensions;

namespace Ujeby.AoC.App._2015_12
{
	[AoCPuzzle(Year = 2015, Day = 12, Answer1 = "111754", Answer2 = null)]
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
			// object
			if (json[0] == '{')
			{
				json = json[1..^1];
				for (var i = 0; i < json.Length; i++)
				{
					if (json[i] == ',')
						continue;

					var propertyName = json[(i + 1)..json.IndexOf('\"', i + 1)];

					var sIdx = i + propertyName.Length + 3; // 2x double qotes "" + :
					var eIdx = sIdx;
					if (json[sIdx] == '{' || json[sIdx] == '[')
					{
						eIdx = IndexOfClosingBracket(json, sIdx);
						sum += SumJsonWithoutRed(json[sIdx..eIdx]);
					}
					else
					{
						// string
						if (json[sIdx] == '\"')
						{
							eIdx = json.IndexOf('\"', sIdx + 1);
							var value = json[(sIdx+1)..eIdx];
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
							if (!int.TryParse(n, out int value))
								throw new Exception($"'{json}' -> {sIdx}..{eIdx}={n}");

							sum += value;	
						}
					}
					i = eIdx;
				}
			}
			// array
			else if (json[0] == '[')
			{
				json = json[1..^1];
				for (var i = 0; i < json.Length; i++)
				{
					if (json[i] == ',')
						continue;

					var sIdx = i;
					var eIdx = sIdx;
					if (json[sIdx] == '{' || json[sIdx] == '[')
					{
						eIdx = IndexOfClosingBracket(json, sIdx);
						sum += SumJsonWithoutRed(json[sIdx..eIdx]);
					}
					else
					{
						// string
						if (json[sIdx] == '\"')
						{
							eIdx = json.IndexOf('\"', sIdx + 1);
							var value = json[(sIdx + 1)..eIdx];
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
							if (!int.TryParse(n, out int value))
								throw new Exception($"'{json}' -> {sIdx}..{eIdx}={n}");

							sum += value;
						}
					}
					i = eIdx;
				}

			}

			return sum;
		}

		/// <summary>
		/// returns index of corresponding closing bracket (with nesting in mind)
		/// </summary>
		/// <param name="s"></param>
		/// <param name="openingBracketIndex"></param>
		/// <param name="brackets"></param>
		/// <returns></returns>
		public static int IndexOfClosingBracket(string s, 
			int openingBracketIndex = 0, string openingBrackets = "{([", string closingBrackets = "})]")
		{
			var i = openingBracketIndex;
			var nest = new int[openingBrackets.Length];
			do
			{
				var nOpenId = openingBrackets.IndexOf(s[i]);
				if (nOpenId != -1)
					nest[nOpenId]++;
				else
				{
					var nCloseId = closingBrackets.IndexOf(s[i]);
					if (nCloseId != -1)
						nest[nCloseId]--;
				}

				i++;
			}
			while (nest.Any(n => n > 0));

			return i;
		}
	}
}
