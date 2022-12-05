﻿using Ujeby.AoC.Common;

namespace Ujeby.AoC.App.Day10
{
	internal class SyntaxScoring : ProblemBase
	{
		protected override (string, string) SolveProblem(string[] input)
		{
			var tags = new char[][] 
			{ 
				new[] { '(', '[', '{', '<' },
				new[] { ')', ']', '}', '>' },
			};

			var score = new Dictionary<char, long>()
			{
				{ ')', 3 },
				{ ']', 57 },
				{ '}', 1197 },
				{ '>', 25137 },
			};

			// part1
			long result1 = 0;
			for (var i = 0; i < input.Length; i++)
			{
				var open = new List<char>();
				foreach (var c in input[i])
				{
					if (tags[0].Contains(c))
						open.Add(c);
					else
					{
						if (Array.IndexOf(tags[0], open.Last()) != Array.IndexOf(tags[1], c))
						{
							result1 += score[c];
							break;
						}

						open.RemoveAt(open.Count - 1);
					}
				}
			}

			// part2
			var scores = new List<long>();
			for (var i = 0; i < input.Length; i++)
			{
				var corrupted = false;
				var open = new List<char>();
				foreach (var c in input[i])
				{
					if (tags[0].Contains(c))
						open.Add(c);
					else
					{
						if (Array.IndexOf(tags[0], open.Last()) != Array.IndexOf(tags[1], c))
						{
							corrupted = true;
							break;
						}

						open.RemoveAt(open.Count - 1);
					}
				}

				if (corrupted)
					continue;

				long s = 0;
				for (var c = open.Count - 1; c >= 0; c--)
					s = s * 5 + Array.IndexOf(tags[0], open[c]) + 1;
				scores.Add(s);
			}
			long result2 = scores.OrderBy(s => s).ElementAt(scores.Count / 2);

			return (result1.ToString(), result2.ToString());
		}
	}
}
