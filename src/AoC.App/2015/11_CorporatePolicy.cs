using System.Runtime.Serialization;
using Ujeby.AoC.Common;

namespace Ujeby.AoC.App._2015_11
{
	[AoCPuzzle(Year = 2015, Day = 11, Answer1 = "hxbxxyzz", Answer2 = "hxcaabcc")]
	public class CorporatePolicy : PuzzleBase
	{
		protected override (string Part1, string Part2) SolvePuzzle(string[] input)
		{
			// part1
			var answer1 = NextValidPassword(input.Single());

			// part2
			var answer2 = NextValidPassword(IncreasePassword(answer1));

			return (answer1, answer2);
		}

		private static string NextValidPassword(string oldPassword)
		{
			while (!VerifyPasswordPolicy(oldPassword))
				oldPassword = IncreasePassword(oldPassword);

			return oldPassword;
		}

		private static string IncreasePassword(string oldPassword)
		{
			var chars = oldPassword.ToCharArray();

			if (chars[^1] != 'z')
				chars[^1]++;

			else
			{
				chars[^1] = 'a';
				var wrapAround = 1;
				for (var i = chars.Length - 2; i >= 0 && wrapAround > 0; i--)
				{
					wrapAround--;
					if (chars[i] != 'z')
						chars[i]++;

					else
					{
						chars[i] = 'a';
						wrapAround++;
					}
				}
			}

			return new string(chars);
		}

		private static char[] _notAllowedChars = new char[] { 'i', 'o', 'l' };

		private static bool VerifyPasswordPolicy(string password)
		{
			// Passwords may not contain the letters i, o, or l
			if (_notAllowedChars.Any(c => password.Contains(c)))
				return false;

			// two different, non-overlapping pairs
			char p1Char = '0';
			bool p1 = false, p2 = false;
			for (var i = 0; i < password.Length - 1; i++)
			{
				if (password[i] != password[i + 1])
					continue;

				if (!p1)
				{
					p1 = true;
					p1Char = password[i];
				}
				else if (p1 && password[i] != p1Char)
				{
					p2 = true;
					break;
				}
			}
			if (!p1 || !p2)
				return false;

			// Passwords must include one increasing straight of at least three letters
			for (var i = 0; i < password.Length - 3; i++)
				if (password[i] == password[i + 1] - 1 && password[i + 1] == password[i + 2] - 1)
					return true;

			return false;
		}
	}
}
