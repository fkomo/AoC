using Ujeby.AoC.Common;
using Ujeby.Tools.ArrayExtensions;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2023_14;

[AoCPuzzle(Year = 2023, Day = 14, Answer1 = "109098", Answer2 = "100064", Skip = false)]
public class ParabolicReflectorDish : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var dish = input.Select(x => x.Select(y => y).ToArray()).ToArray();

#if DEBUG
		foreach (var line in dish)
			Debug.Line(new string(line));
		Debug.Line();
#endif
		// part1
		var answer1 = NorthLoad(dish);

		// part2
		var invalidRock = new v2i(-1);
		var rocks = input
			.SelectMany((row, y) => row.Select((col, x) => (Pos: new v2i(x, y), Shape: col)))
			.Where(x => x.Shape != '.')
			.ToArray();
		Debug.Line($"{rocks.Length} rocks [#,O]");

		var loads = new List<long>();
		var sw = System.Diagnostics.Stopwatch.StartNew();
		while (sw.ElapsedMilliseconds < 200) // 200ms should make enough cycles to find repeating pattern
		{
			// tilt north
			var stack = Enumerable.Repeat((long)-1, dish.Length).ToArray();
			rocks = rocks.OrderBy(x => x.Pos.Y).ToArray();
			for (var i = 0; i < rocks.Length; i++)
			{
				var rock = rocks[i];
				if (rocks[i].Shape == '#')
					stack[rock.Pos.X] = rock.Pos.Y;
				else if (rocks[i].Shape == 'O')
					rocks[i].Pos.Y = ++stack[rock.Pos.X];
			}

			// tilt west
			stack = Enumerable.Repeat((long)-1, dish.Length).ToArray();
			rocks = rocks.OrderBy(x => x.Pos.X).ToArray();
			for (var i = 0; i < rocks.Length; i++)
			{
				var rock = rocks[i];
				if (rocks[i].Shape == '#')
					stack[rock.Pos.Y] = rock.Pos.X;
				else if (rocks[i].Shape == 'O')
					rocks[i].Pos.X = ++stack[rock.Pos.Y];
			}

			// tilt south
			stack = Enumerable.Repeat((long)dish.Length, dish.Length).ToArray();
			rocks = rocks.OrderByDescending(x => x.Pos.Y).ToArray();
			for (var i = 0; i < rocks.Length; i++)
			{
				var rock = rocks[i];
				if (rocks[i].Shape == '#')
					stack[rock.Pos.X] = rock.Pos.Y;
				else if (rocks[i].Shape == 'O')
					rocks[i].Pos.Y = --stack[rock.Pos.X];
			}

			// tilt east
			stack = Enumerable.Repeat((long)dish.Length, dish.Length).ToArray();
			rocks = rocks.OrderByDescending(x => x.Pos.X).ToArray();
			for (var i = 0; i < rocks.Length; i++)
			{
				var rock = rocks[i];
				if (rocks[i].Shape == '#')
					stack[rock.Pos.Y] = rock.Pos.X;
				else if (rocks[i].Shape == 'O')
					rocks[i].Pos.X = --stack[rock.Pos.Y];
			}

			loads.Add(NorthLoad(rocks, input.Length));
		}
		sw.Stop();

		long answer2 = 0;
		var loadsArray = loads.ToArray();
		if (loadsArray.FindRepeatingPattern(out int patternStart, out int patternLength))
		{
			var loop = loadsArray
				.Skip(patternStart).Take(patternLength)
				.ToArray();

			answer2 = loop[(1000000000 - patternStart) % patternLength - 1];
		}

		return (answer1.ToString(), answer2.ToString());
	}

	static long NorthLoad((v2i Pos, char Shape)[] rocks, int dishSize)
		=> rocks
			.Where(x => x.Shape == 'O')
			.Sum(x => dishSize - x.Pos.Y);

	static long NorthLoad(char[][] dish)
	{
		long load = 0;
		var northStack = Enumerable.Repeat(-1, dish.Length).ToArray();
		for (var i = 0; i < dish.Length; i++)
		{
			for (var x = 0; x < dish[i].Length; x++)
			{
				if (dish[i][x] == '#')
					northStack[x] = i;
				else if (dish[i][x] == 'O')
				{
					northStack[x]++;
					load += dish.Length - northStack[x];
				}
			}
		}

		return load;
	}
}