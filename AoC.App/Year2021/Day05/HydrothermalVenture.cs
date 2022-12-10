﻿using Ujeby.AoC.Common;

namespace Ujeby.AoC.App.Year2021.Day05
{
	internal class HydrothermalVenture : ProblemBase
	{
		protected override (string, string) SolveProblem(string[] input)
		{
			var size = new Point();
			var lines = input
				.Select(l =>
				{
					var split = l.Split(" -> ");
					var split1 = split[0].Split(',');
					var split2 = split[1].Split(',');

					var point = (
						new Point(int.Parse(split1[0]), int.Parse(split1[1])),
						new Point(int.Parse(split2[0]), int.Parse(split2[1])));

					if (point.Item1.X > size.X)
						size.X = point.Item1.X;
					if (point.Item2.X > size.X)
						size.X = point.Item2.X;

					if (point.Item1.Y > size.Y)
						size.Y = point.Item1.Y;
					if (point.Item2.Y > size.Y)
						size.Y = point.Item2.Y;

					return point;
				}).ToArray();
			size.X += 1;
			size.Y += 1;

			var map = new int[size.X * size.Y];

			// part1
			foreach (var line in lines)
			{
				// vertical
				if (line.Item1.X == line.Item2.X)
				{
					var length = Math.Abs(line.Item1.Y - line.Item2.Y) + 1;

					var y = line.Item1.Y;
					var dy = (line.Item1.Y < line.Item2.Y) ? 1 : -1;
					for (var i = 0; i < length; i++, y += dy)
						map[y * size.X + line.Item1.X]++;
				}
				// horizontal
				else if (line.Item1.Y == line.Item2.Y)
				{
					var length = Math.Abs(line.Item1.X - line.Item2.X) + 1;

					var x = line.Item1.X;
					var dx = (line.Item1.X < line.Item2.X) ? 1 : -1;
					for (var i = 0; i < length; i++, x += dx)
						map[line.Item1.Y * size.X + x]++;
				}
			}
			long answer1 = map.Count(p => p > 1);
			
			// part2
			foreach (var line in lines)
			{
				// diagonal
				if (Math.Abs(line.Item1.X - line.Item2.X) == Math.Abs(line.Item1.Y - line.Item2.Y))
				{
					var length = Math.Abs(line.Item1.X - line.Item2.X) + 1;

					var dx = (line.Item1.X < line.Item2.X) ? 1 : -1;
					var dy = (line.Item1.Y < line.Item2.Y) ? 1 : -1;

					var x = line.Item1.X;
					var y = line.Item1.Y;
					for (var i = 0; i < length; i++, x += dx, y += dy)
						map[y * size.X + x]++;
				}
			}
			long answer2 = map.Count(p => p > 1);

			return (answer1.ToString(), answer2.ToString());
		}
	}
}