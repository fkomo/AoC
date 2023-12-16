using Ujeby.AoC.Common;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2023_16;

[AoCPuzzle(Year = 2023, Day = 16, Answer1 = "7046", Answer2 = "7313", Skip = false)]
public class TheFloorWillBeLava : PuzzleBase
{
	static readonly char[] _mirrorTiles = new char[] { '/', '\\' };
	static readonly char[] _splitterTiles = new char[] { '-', '|' };

	static readonly Dictionary<(char, v2i), v2i> _mirror = new()
	{
		{ ('/', v2i.Left), v2i.Up },
		{ ('/', v2i.Right), v2i.Down },
		{ ('/', v2i.Up), v2i.Left },
		{ ('/', v2i.Down), v2i.Right },

		{ ('\\', v2i.Left), v2i.Down },
		{ ('\\', v2i.Right), v2i.Up },
		{ ('\\', v2i.Up), v2i.Right },
		{ ('\\', v2i.Down), v2i.Left }
	};

	static readonly Dictionary<(char, v2i), v2i[]> _splitter = new()
	{
		{ ('-', v2i.Up ), new v2i[] { v2i.Left, v2i.Right } },
		{ ('-', v2i.Down ), new v2i[] { v2i.Left, v2i.Right } },
		{ ('-', v2i.Left ), new v2i[] { v2i.Left } },
		{ ('-', v2i.Right ), new v2i[] { v2i.Right } },

		{ ('|', v2i.Up ), new v2i[] { v2i.Up } },
		{ ('|', v2i.Down ), new v2i[] { v2i.Down } },
		{ ('|', v2i.Left ), new v2i[] { v2i.Up, v2i.Down } },
		{ ('|', v2i.Right ), new v2i[] { v2i.Up, v2i.Down } },
	};

	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var grid = input.Select(x => x.Select(y => y).ToArray()).ToArray();

		// part1
		var answer1 = Energize(grid, v2i.Zero, v2i.Right);

		// part2
		var answer2 = long.MinValue;
		for (var i = 0; i < grid.Length; i++)
		{
			answer2 = System.Math.Max(answer2, Energize(grid, new v2i(0, i), v2i.Right));
			answer2 = System.Math.Max(answer2, Energize(grid, new v2i(grid.Length - 1, i), v2i.Left));
			answer2 = System.Math.Max(answer2, Energize(grid, new v2i(i, 0), v2i.Up));
			answer2 = System.Math.Max(answer2, Energize(grid, new v2i(i, grid.Length - 1), v2i.Down));
		}

		return (answer1.ToString(), answer2.ToString());
	}

	static long Energize(char[][] grid, v2i origin, v2i dir)
	{
		var energized = new HashSet<v2i>();
		var energizedMirror = new HashSet<(v2i, v2i)>();

		var beams = new Queue<(v2i beamPos, v2i beamDir)>();
		beams.Enqueue((origin, dir));

		while (beams.Count != 0)
		{
			var (beamPos, beamDir) = beams.Dequeue();
			while (beamPos.X >= 0 && beamPos.Y >= 0 && beamPos.X < grid.Length && beamPos.Y < grid.Length)
			{
				energized.Add(beamPos);

				var tile = grid[beamPos.Y][(int)beamPos.X];

				// if this item is already energized with another beam from same direction
				if (tile != '.' && !energizedMirror.Add((beamPos, beamDir)))
					break;

				if (_mirrorTiles.Contains(tile))
					beamDir = _mirror[(tile, beamDir)];
	
				else if (_splitterTiles.Contains(tile))
				{
					var split = _splitter[(tile, beamDir)];
					if (split.Length == 2)
					{
						beams.Enqueue((beamPos + split[1], split[1]));
						beamDir = split[0];
					}
				}

				beamPos += beamDir;
			}
		}

		return energized.Count;
	}
}