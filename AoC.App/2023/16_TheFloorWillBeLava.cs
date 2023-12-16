using Ujeby.AoC.Common;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2023_16;

[AoCPuzzle(Year = 2023, Day = 16, Answer1 = "7046", Answer2 = null, Skip = false)]
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
		var answer1 = Energize(grid);

		// part2
		string answer2 = null;

		return (answer1.ToString(), answer2?.ToString());
	}

	static long Energize(char[][] grid)
	{
		var energized = new HashSet<v2i>();
		var energizedFrom = new HashSet<(v2i Position, v2i Direction)>();

		var beams = new Queue<(v2i Position, v2i Dir)>();
		beams.Enqueue((v2i.Zero, v2i.Right));

		while (beams.Count != 0)
		{
			var beam = beams.Dequeue();
			while (beam.Position.X >= 0 && beam.Position.Y >= 0 && beam.Position.X < grid.Length && beam.Position.Y < grid.Length)
			{
				energized.Add(beam.Position);

				var tile = grid[beam.Position.Y][(int)beam.Position.X];

				// if this item is already energized with another beam from same direction
				if (tile != '.' && !energizedFrom.Add((beam.Position, beam.Dir)))
					break;

				if (_mirrorTiles.Contains(tile))
					beam.Dir = _mirror[(tile, beam.Dir)];
	
				else if (_splitterTiles.Contains(tile))
				{
					var split = _splitter[(tile, beam.Dir)];
					if (split.Length == 2)
					{
						beams.Enqueue((beam.Position + split[1], split[1]));
						beam.Dir = split[0];
					}
				}

				beam.Position += beam.Dir;
			}
		}

		return energized.Count;
	}
}