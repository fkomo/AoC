using Ujeby.AoC.Common;
using Ujeby.Tools.StringExtensions;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2016_08;

[AoCPuzzle(Year = 2016, Day = 08, Answer1 = "119", Answer2 = "ZFHFSFOGPO", Skip = false)]
public class TwoFactorAuthentication : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		// part1
		var screenSize = new v2i(50, 6);
		var screen = new HashSet<v2i>();
		foreach (var op in input)
		{
			var param = new v2i(op.ToNumArray());
			if (op.StartsWith("rect"))
			{
				for (var y = 0; y < param.Y; y++)
					for (var x = 0; x < param.X; x++)
						screen.Add(new(x, y));
			}
			else if (op.StartsWith("rotate column"))
			{
				var tmp = new HashSet<v2i>();
				for (var y = screenSize.Y - 1; y >= 0; y--)
				{
					var old = new v2i(param.X, y);
					if (!screen.Contains(old))
						continue;

					screen.Remove(old);
					tmp.Add(new v2i(param.X, (y + param.Y) % screenSize.Y));
				}

				foreach (var p in tmp)
					screen.Add(p);
			}
			else if (op.StartsWith("rotate row"))
			{
				var tmp = new HashSet<v2i>();
				for (var x = screenSize.X - 1; x >= 0; x--)
				{
					var old = new v2i(x, param.X);
					if (!screen.Contains(old))
						continue;

					screen.Remove(old);
					tmp.Add(new v2i((x + param.Y) % screenSize.X, param.X));
				}

				foreach (var p in tmp)
					screen.Add(p);
			}

			//DrawScreen(screen, screenSize);
		}
		var answer1 = screen.Count;

		// part2
		var screen2d = new bool[screenSize.Y, screenSize.X];
		foreach (var p in screen)
			screen2d[p.Y, p.X] = true;

		var answer2 = CharCodes.ToString(screenSize, screen2d);

		return (answer1.ToString(), answer2?.ToString());
	}

	private static void DrawScreen(HashSet<v2i> screen, v2i size)
	{
		Debug.Line();
		for (var y = 0; y < size.Y; y++)
		{
			var line = string.Empty;
			for (var x = 0; x < size.X; x++)
				line += screen.Contains(new v2i(x, y)) ? '#' : '.';
			Debug.Line(line);
		}
	}
}