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
			var cubeMap = CreateCubeMap(input);
			var path2 = TravelCube(cubeMap, directions, new(0, 0, 0, 1));
			var p = CubeToMap(path2.Last());
			long? answer2 = 1000 * (p.Y + 1) + 4 * (p.X + 1) + p.Z;

			return (answer1?.ToString(), answer2?.ToString());
		}

		private v3i CubeToMap(v4i facePoint)
		{
			switch (facePoint.W)
			{
#if _DEBUG_SAMPLE
				case 1: return new v3i(facePoint.X + 8, facePoint.Y + 0, facePoint.Z);
				case 2: return new v3i(facePoint.X + 0, facePoint.Y + 4, facePoint.Z);
				case 3: return new v3i(facePoint.X + 4, facePoint.Y + 4, facePoint.Z);
				case 4: return new v3i(facePoint.X + 8, facePoint.Y + 4, facePoint.Z);
				case 5: return new v3i(facePoint.X + 8, facePoint.Y + 8, facePoint.Z);
				case 6: return new v3i(facePoint.X + 12, facePoint.Y + 8, facePoint.Z);
#else
				// TODO 2022/22 part2 cube to 2d map point transformation
#endif

				default:
					break;
			}

			throw new Exception();
		}

		private Dictionary<long, char[,]> CreateCubeMap(string[] input)
		{
			var result = new Dictionary<long, char[,]>();
			char[,] map = null;

#if _DEBUG_SAMPLE
			map = new char[_faceSize, _faceSize];
			for (var y = 0; y < _faceSize; y++)
				for (var x = 0; x < _faceSize; x++)
					map[y, x] = input[y][x + 8];
			result.Add(1, map);

			map = new char[_faceSize, _faceSize];
			for (var y = 0; y < _faceSize; y++)
				for (var x = 0; x < _faceSize; x++)
					map[y, x] = input[y + 4][x];
			result.Add(2, map);

			map = new char[_faceSize, _faceSize];
			for (var y = 0; y < _faceSize; y++)
				for (var x = 0; x < _faceSize; x++)
					map[y, x] = input[y + 4][x + 4];
			result.Add(3, map);

			map = new char[_faceSize, _faceSize];
			for (var y = 0; y < _faceSize; y++)
				for (var x = 0; x < _faceSize; x++)
					map[y, x] = input[y + 4][x + 8];
			result.Add(4, map);

			map = new char[_faceSize, _faceSize];
			for (var y = 0; y < _faceSize; y++)
				for (var x = 0; x < _faceSize; x++)
					map[y, x] = input[y + 8][x + 8];
			result.Add(5, map);

			map = new char[_faceSize, _faceSize];
			for (var y = 0; y < _faceSize; y++)
				for (var x = 0; x < _faceSize; x++)
					map[y, x] = input[y + 8][x + 12];
			result.Add(6, map);
#else
			// TODO 2022/22 part2 create release cube map
#endif

			return result;
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

#if _DEBUG_SAMPLE
		private static readonly int _faceSize = 4;
		private static readonly Dictionary<string, Func<v2i, v4i>> _edge = new()
		{
			{ "40", (position) => { return new v4i(_faceSize - 1 - position.Y, 0, 1, 6); } },
			{ "63", (position) => { return new v4i(_faceSize - 1, _faceSize - 1 - position.X, 2, 4); } },

			{ "10", (position) => { return new v4i(_faceSize - 1, _faceSize - 1 - position.Y, 2, 6); } },
			{ "60", (position) => { return new v4i(_faceSize - 1, _faceSize - 1 - position.Y, 2, 1); } },

			{ "13", (position) => { return new v4i(position.X, 0, 1, 2); } },
			{ "23", (position) => { return new v4i(position.X, 0, 1, 1); } },

			{ "22", (position) => { return new v4i(_faceSize - 1 - position.Y, _faceSize - 1, 3, 6); } },
			{ "61", (position) => { return new v4i(0, _faceSize - 1 - position.X, 0, 2); } },

			{ "33", (position) => { return new v4i(0, position.X, 0, 1); } },
			{ "12", (position) => { return new v4i(position.Y, 0, 1, 3); } },

			{ "31", (position) => { return new v4i(0, _faceSize - 1 - position.X, 0, 5); } },
			{ "52", (position) => { return new v4i(_faceSize - 1 - position.Y, 0, 3, 3); } },

			{ "51", (position) => { return new v4i(_faceSize - 1 - position.X, _faceSize - 1, 3, 2); } },
			{ "21", (position) => { return new v4i(_faceSize - 1 - position.X, _faceSize - 1, 3, 5); } },

			{ "11", (position) => { return new v4i(position.X, 0, 1, 4); } },
			{ "43", (position) => { return new v4i(position.X, _faceSize - 1, 3, 1); } },

			{ "41", (position) => { return new v4i(position.X, 0, 1, 5); } },
			{ "53", (position) => { return new v4i(position.X, _faceSize - 1, 3, 4); } },

			{ "50", (position) => { return new v4i(0, position.Y, 0, 6); } },
			{ "62", (position) => { return new v4i(_faceSize - 1, position.Y, 2, 5); } },

			{ "42", (position) => { return new v4i(_faceSize - 1, position.Y, 2, 3); } },
			{ "30", (position) => { return new v4i(0, position.Y, 0, 4); } },

			{ "32", (position) => { return new v4i(_faceSize - 1, position.Y, 2, 2); } },
			{ "20", (position) => { return new v4i(0, position.Y, 0, 3); } },
		};
#else
		private static int _faceSize = 50;
		private static Dictionary<string, Func<v2i, v4i>> _edge = new()
		{
			// TODO 2022/22 part2 release edge transformations
		};
#endif

		/// <summary>
		/// position [X, Y, Z - facing, W - face]
		/// </summary>
		/// <param name="cubeMap"></param>
		/// <param name="directions"></param>
		/// <param name="position"></param>
		/// <returns></returns>
		public static v4i[] TravelCube(Dictionary<long, char[,]> cubeMap, string[] directions, v4i position)
		{
			var path = new List<v4i>
			{
				position,
			};

			foreach (var step in directions)
			{
				// turn left
				if (step == "L")
				{
					position.Z = (position.Z - 1 + 4) % 4;
					path.Add(position);
				}
				// turn right
				else if (step == "R")
				{
					position.Z = (position.Z + 1) % 4;
					path.Add(position);
				}
				else
				{
					// move
					var distance = int.Parse(step);
					for (var i = 0; i < distance; i++)
					{
						var dir = _facingToDirection[position.Z];
						var map = cubeMap[position.W];

						var newPosition = position + dir;

						if (newPosition.Y < 0 || newPosition.X < 0 ||
							newPosition.X == _faceSize || newPosition.Y == _faceSize)
						{
							// transition to next face
							newPosition = _edge[$"{position.W}{position.Z}"](position.ToV2i());
							map = cubeMap[newPosition.W];
						}

						if (map[newPosition.Y, newPosition.X] == '#')
							break;

						position = newPosition;
						path.Add(position);
					}
				}
			}

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
					position.Z = (position.Z - 1 + 4) % 4;

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
