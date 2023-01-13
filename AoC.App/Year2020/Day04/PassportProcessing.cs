using System.Globalization;
using Ujeby.AoC.Common;

namespace Ujeby.AoC.App.Year2020.Day04
{
	public class PassportProcessing : PuzzleBase
	{
		private static string[] _reqFields = new string[]
		{
			"byr",
			"iyr",
			"eyr",
			"hgt",
			"hcl",
			"ecl",
			"pid",
			//"cid"
		};

		private static string[] _eyeColors = new string[]
		{
			"amb",
			"blu",
			"brn",
			"gry",
			"grn",
			"hzl",
			"oth",
		};

		protected override (string, string) SolvePuzzle(string[] input)
		{
			// part1
			long? answer1 = CountValid(input);

			// part2
			long? answer2 = CountValid(input, verifyFormat: true);

			return (answer1?.ToString(), answer2?.ToString());
		}

		private static long CountValid(string[] input,
			bool verifyFormat = false)
		{
			var result = 0L;

			var fields = new List<string>();
			for (var i = 0; i < input.Length; i++)
			{
				if (input[i] == string.Empty)
				{
					result += VerifyPassport(fields.ToArray(), verifyFormat: verifyFormat) ? 1 : 0;
					fields.Clear();
					continue;
				}

				fields.AddRange(input[i].Split(' '));
			}

			return result + (VerifyPassport(fields.ToArray()) ? 1 : 0);
		}

		private static bool VerifyPassport(string[] fields,
			bool verifyFormat = false)
		{
			if (fields.Length < _reqFields.Length
				|| _reqFields.Any(rf => !fields.Any(f => f.StartsWith($"{rf}:"))))
				return false;

			if (!verifyFormat)
				return true;

			foreach (var field in fields)
			{
				try
				{
					var f = field.Split(':');
					switch (f[0])
					{
						case "byr":
							{
								if (!int.TryParse(f[1], out int year) || year < 1920 || year > 2002)
									return false;
							}
							break;
						case "iyr":
							{
								if (!int.TryParse(f[1], out int year) || year < 2010 || year > 2020)
									return false;
							}
							break;
						case "eyr":
							{
								if (!int.TryParse(f[1], out int year) || year < 2020 || year > 2030)
									return false;
							}
							break;
						case "hgt":
							{
								if (!f[1].EndsWith("cm") && !f[1].EndsWith("in"))
									return false;

								switch (f[1][^2..])
								{
									case "cm":
										{
											if (!int.TryParse(f[1][..^2], out int hgt) 
												|| hgt < 150 || hgt > 193)
												return false;
										}
										break;
									case "in":
										{
											if (!int.TryParse(f[1][..^2], out int hgt) 
												|| hgt < 59 || hgt > 76)
												return false;
										}
										break;

									default:
										return false;
								}
							}
							break;
						case "hcl":
							{
								if (f[1][0] != '#' 
									|| !int.TryParse(f[1][1..], NumberStyles.HexNumber, null, out _))
									return false;
							}
							break;

						case "ecl":
							{
								if (!_eyeColors.Contains(f[1]))
									return false;
							}
							break;

						case "pid":
							{
								if (f[1].Length != 9 || f[1].Any(c => !char.IsDigit(c)))
									return false;
							}
							break;

						case "cid":
							{
								// optional
							}
							break;

						default:
							throw new NotImplementedException(f[0]);
					}
				}
				catch (Exception ex)
				{
					Debug.Line(field);
				}
			}

			return true;
		}
	}
}
