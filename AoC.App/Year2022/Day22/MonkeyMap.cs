using System.Data;
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

		protected override (string, string) SolvePuzzle(string[] input)
		{
			var map = CreateMap(input);
			var directions = ReadDirections(input);

#if _DEBUG_SAMPLE
			Debug.Line();
			for (var y = 0; y < map.Length; y++)
				Debug.Line(new string(map[y]));
			Debug.Line();
#endif
			// part1
			var path = Travel(map, directions, new(Array.IndexOf(map[0], '.'), 0, 0));
			long? answer1 = 1000 * (path.Last().Y + 1) + 4 * (path.Last().X + 1) + path.Last().Z;

			// part2
			var cubeMap = CreateCubeMap(input);
			var path2 = TravelCube(cubeMap, directions, new(0, 0, 0, 0));
			var p = CubeToMap(path2.Last());
			long? answer2 = 1000 * (p.Y + 1) + 4 * (p.X + 1) + p.Z;

			return (answer1?.ToString(), answer2?.ToString());
		}

		/// <summary>
		/// transforms point on cube face to point on map
		/// </summary>
		/// <param name="facePoint"></param>
		/// <returns></returns>
		private v3i CubeToMap(v4i facePoint) => facePoint.ToV3i() + _mapFaces[facePoint.W];

		public static Dictionary<long, char[,]> CreateCubeMap(string[] input)
		{
			var result = new Dictionary<long, char[,]>();

			for (var i = 0; i < _mapFaces.Length; i++)
				result.Add(i, CreateCubeFace(_mapFaces[i], input, _faceSize));

			return result;
		}

		private static char[,] CreateCubeFace(v2i topLeft, string[] input, int size)
		{
			var map = new char[size, size];
			for (var y = 0; y < size; y++)
				for (var x = 0; x < size; x++)
					map[y, x] = input[y + (int)topLeft.Y][x + (int)topLeft.X];

			return map;
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

		/// <summary>
		/// position transition over cube edge
		/// [sourceEdge1, destinationEdge](sourceFacePosition, cubeEdgeSize) => destinationFacePosition
		/// </summary>
		private static readonly Func<v2i, int, v2i>[,] _edge = new Func<v2i, int, v2i>[4, 4]
		{
			{ (p, s) => new(s - 1, s - 1 - p.Y), (p, s) => new(p.Y, s - 1), (p, s) => new(0, p.Y), (p, s) => new(s - 1 - p.Y, 0) },
			{ (p, s) => new(s - 1, p.X), (p, s) => new(s - 1 - p.X, s - 1), (p, s) => new(0, s - 1 - p.X), (p, s) => new(p.X, 0) },
			{ (p, s) => new(s - 1, p.Y), (p, s) => new(s - 1 - p.Y, s - 1), (p, s) => new(0, s - 1 - p.Y), (p, s) => new(p.Y, 0) },
			{ (p, s) => new(s - 1, s - 1 - p.X), (p, s) => new(p.X, s - 1), (p, s) => new(0, p.X), (p, s) => new(s - 1 - p.X, 0) },
		};

#if _DEBUG_SAMPLE
		private static readonly int _faceSize = 4;

		private static readonly v2i[] _mapFaces = new v2i[]
		{
			new (8, 0), new (0, 4), new(4, 4), new(8, 4), new (8, 8), new (12, 8)
		};

		/// <summary>
		/// cube face folding
		/// [sourceFace, sourceFaceEdge] => (destinationFace, destinationFaceEdge)
		/// </summary>
		private static readonly v2i[,] _folding = new v2i[6, 4]
		{
			{ new(5, 0), new(3, 3), new(2, 3), new(1, 3) },
			{ new(2, 2), new(4, 1), new(5, 1), new(0, 3) },
			{ new(3, 2), new(4, 2), new(1, 0), new(0, 2) },
			{ new(5, 3), new(4, 3), new(2, 0), new(0, 1) },
			{ new(5, 2), new(1, 1), new(2, 1), new(3, 1) },
			{ new(0, 0), new(1, 2), new(4, 0), new(3, 0) }
		};
#else
		private static int _faceSize = 50;

		private static readonly v2i[] _mapFaces = new v2i[]
		{
			new (50, 0), new (100, 0), new(50, 50), new(0, 100), new (50, 100), new (0, 150)
		};

		/// <summary>
		/// cube face folding
		/// [sourceFace, sourceFaceEdge] => (destinationFace, destinationFaceEdge)
		/// </summary>
		private static readonly v2i[,] _folding = new v2i[6, 4]
		{
			{ new(1, 2), new(2, 3), new(3, 2), new(5, 2) },
			{ new(4, 0), new(2, 0), new(0, 0), new(5, 1) },
			{ new(1, 1), new(4, 3), new(3, 3), new(0, 1) },
			{ new(4, 2), new(5, 3), new(0, 2), new(2, 2) },
			{ new(1, 0), new(5, 0), new(3, 0), new(2, 1) },
			{ new(4, 1), new(1, 3), new(0, 3), new(3, 1) }
		};
#endif

		/// <summary>
		/// position [X(x-pos), Y(y-pos), Z(direction/facing), W(face)]
		/// </summary>
		/// <param name="cubeMap"></param>
		/// <param name="directions"></param>
		/// <param name="position"></param>
		/// <returns></returns>
		public static v4i[] TravelCube(Dictionary<long, char[,]> cubeMap, string[] directions, v4i position)
		{
			var faceSize = cubeMap.First().Value.GetLength(0);

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
							newPosition.X == faceSize || newPosition.Y == faceSize)
						{
							// transition to next face
							var newFace = _folding[position.W, position.Z];
							newPosition = new v4i(_edge[position.Z, newFace.Y](position.ToV2i(), faceSize), (newFace.Y + 2) % 4, newFace.X);

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
