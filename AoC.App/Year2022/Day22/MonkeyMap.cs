using Ujeby.AoC.Common;
using Ujeby.Vectors;

namespace Ujeby.AoC.App.Year2022.Day22
{
	public class MonkeyMap : PuzzleBase
	{
		private static Dictionary<long, v2i> _facingToDirection = new()
		{
			{ 0, v2i.Right },
			{ 1, v2i.Up },
			{ 2, v2i.Left },
			{ 3, v2i.Down }
		};

		protected override (string, string) SolveProblem(string[] input)
		{
			var map = CreateMap(input);
			var directions = ReadDirections(input);

			Debug.Line();
			for (var y = 0; y < map.Length; y++)
				Debug.Line(new string(map[y]));
			Debug.Line();

			// part1
			var path = Travel(map, directions, new(Array.IndexOf(map[0], '.'), 0, 0));
			long? answer1 = 1000 * (path.Last().Y + 1) + 4 * (path.Last().X + 1) + path.Last().Z;

			// part2
			path = TravelCube(map, directions, new(Array.IndexOf(map[0], '.'), 0, 0));
			long? answer2 = 1000 * (path.Last().Y + 1) + 4 * (path.Last().X + 1) + path.Last().Z;

			return (answer1?.ToString(), answer2?.ToString());
		}

		public static string[] ReadDirections(string[] input)
		{
			return input[Array.IndexOf(input, "") + 1]
				.Replace("R", " R ")
				.Replace("L", " L ")
				.Split(" ", StringSplitOptions.RemoveEmptyEntries);
		}

		public static char[][] CreateMap(string[] input)
		{
			var mapHeight = Array.IndexOf(input, "");
			var mapWidth = input.Take(mapHeight).Max(l => l.Length);

			var map = input.Take(mapHeight).Select(l => l.ToArray()).ToArray();

			for (var y = 0; y < mapHeight; y++)
				if (map[y].Length < mapWidth)
					map[y] = map[y].Concat(Enumerable.Repeat(' ', mapWidth - map[y].Length).ToArray()).ToArray();

			return map;
		}

		private static v3i[] TravelCube(char[][] map, string[] directions, v3i position)
		{
			var path = new List<v3i>
			{
				position
			};




			path.Add(position);
			return path.ToArray();
		}


		public static v3i[] Travel(char[][] map, string[] directions, v3i position)
		{
			var path = new List<v3i>
			{
				position
			};

			var mapWidth = map.First().Length;
			var mapHeight = map.Length;

			foreach (var step in directions)
			{
				// turn left
				if (step == "L")
				{
					position.Z = (position.Z - 1 + 4) % 4;
				}
				// turn right
				else if (step == "R")
					position.Z = (position.Z + 1) % 4;

				else
				{
					// move
					var distance = int.Parse(step);
					var dir = _facingToDirection[position.Z];
					
					for (var i = 0; i < distance; i++)
					{
						var newPosition = position + dir;

						if (newPosition.Y < 0 || newPosition.X < 0 ||
							newPosition.X == mapWidth || newPosition.Y == mapHeight ||
							map[newPosition.Y][newPosition.X] == ' ')
						{
							var backPosition = new v3i(0, newPosition.Y, newPosition.Z);
							switch (position.Z)
							{
								case 0: backPosition = new v3i(0, newPosition.Y, newPosition.Z); break;
								case 1: backPosition = new v3i(newPosition.X, 0, newPosition.Z); break;
								case 2: backPosition = new v3i(mapWidth - 1, newPosition.Y, newPosition.Z); break;
								case 3: backPosition = new v3i(newPosition.X, mapHeight - 1, newPosition.Z); break;
							}

							while (true)
							{
								if (map[backPosition.Y][backPosition.X] == '.')
								{
									position = backPosition;
									path.Add(position);
									break;
								}

								if (map[backPosition.Y][backPosition.X] == '#')
								{
									i = distance;
									break;
								}

								backPosition += dir;
							}
						}

						else if (map[newPosition.Y][newPosition.X] == '.')
						{
							position = newPosition;
							path.Add(position);
						}
						else if (map[newPosition.Y][newPosition.X] == '#')
							break;
					}
				}
			}

			path.Add(position);
			return path.ToArray();
		}
	}
}
