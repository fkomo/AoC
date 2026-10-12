using Ujeby.Vectors;

namespace Ujeby.Extensions
{
	public static class GridExtensions
	{
		/// <summary>
		/// fill empty space with specified tile [recursive]
		/// </summary>
		/// <param name="map"></param>
		/// <param name="start"></param>
		/// <param name="fill"></param>
		/// <param name="fillDirections"></param>
		/// <param name="emptyTile"></param>
		public static void FloodFill<T>(this T[][] map, v2i start, T fill, v2i[] fillDirections, params T[] dontFill)
		{
			if (start.X < 0 || start.Y < 0 || start.X == map[0].Length || start.Y == map.Length)
				return;

			if (map[start.Y][start.X].Equals(fill) || dontFill.Contains(map[start.Y][(int)start.X]))
				return;

			map[start.Y][(int)start.X] = fill;
			foreach (var dir in fillDirections)
				map.FloodFill(start + dir, fill, fillDirections, dontFill);
		}

		/// <summary>
		/// fill empty space with specified tile
		/// </summary>
		/// <param name="map"></param>
		/// <param name="start"></param>
		/// <param name="fill"></param>
		/// <param name="fillDirections"></param>
		/// <param name="dontFill"></param>
		public static void FloodFillNonRec<T>(this T[][] map, v2i start, T fill, v2i[] fillDirections, params T[] dontFill)
		{
			var queue = new Queue<v2i>();
			queue.Enqueue(start);
			while (queue.Count > 0)
			{
				var p = queue.Dequeue();

				if (p.X < 0 || p.Y < 0 || p.Y >= map.Length || p.X >= map[p.Y].Length)
					continue;

				if (map[p.Y][p.X].Equals(fill) || dontFill.Contains(map[p.Y][p.X]))
					continue;

				map[p.Y][p.X] = fill;
				foreach (var dir in fillDirections)
					queue.Enqueue(p + dir);
			}
		}

		/// <summary>
		/// returns tiles that are flooded (reachable with given directions) from start
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="map"></param>
		/// <param name="start"></param>
		/// <param name="fillDirections"></param>
		/// <param name="dontFill"></param>
		/// <returns></returns>
		public static Dictionary<v2i, int> FloodFillNonRecWithDistance<T>(this T[][] map, v2i start, v2i[] fillDirections, params T[] dontFill)
		{
			var floodedWithDistance = new Dictionary<v2i, int>();

			var queue = new Queue<(v2i Pos, int Dist)>();
			queue.Enqueue((start, 0));

			while (queue.Count > 0)
			{
				var next = queue.Dequeue();
				var p = next.Pos;

				if (p.X < 0 || p.Y < 0 || p.Y >= map.Length || p.X >= map[p.Y].Length)
					continue;

				if (dontFill.Contains(map[p.Y][p.X]))
					continue;

				if (!floodedWithDistance.TryAdd(p, next.Dist))
					continue;

				foreach (var dir in fillDirections)
					queue.Enqueue((p + dir, next.Dist + 1));
			}

			return floodedWithDistance;
		}

		/// <summary>
		/// compares 2d grids element-by-element
		/// </summary>
		/// <param name="map"></param>
		/// <param name="other"></param>
		/// <returns></returns>
		public static bool EqualTo<T>(this T[][] map, T[][] other)
		{
			if (map.Length != other.Length)
				return false;

			for (var y = 0; y < map.Length; y++)
				for (var x = 0; x < map[y].Length; x++)
					if (!map[y][x].Equals(other[y][x]))
						return false;

			return true;
		}

		/// <summary>
		/// takes smaller grid at specified position
		/// NOTE missing out-of-bound checks
		/// </summary>
		/// <param name="map"></param>
		/// <param name="start"></param>
		/// <param name="size"></param>
		/// <returns></returns>
		public static T[][] TakeSub<T>(this T[][] map, v2i start, v2i size)
			=> map
				.Skip((int)start.Y)
				.Take((int)size.Y)
				.Select(x => x.Skip((int)start.X).Take((int)size.X).ToArray())
				.ToArray();

		/// <summary>
		/// takes smaller grid specified by aa-box
		/// </summary>
		/// <param name="map"></param>
		/// <param name="area"></param>
		/// <returns></returns>
		public static T[][] TakeSub<T>(this T[][] map, aab2i area) => map.TakeSub(area.Min, area.Size);

		/// <summary>
		/// inserts smaller grid at specified position
		/// NOTE missing out-of-bound checks
		/// </summary>
		/// <param name="map"></param>
		/// <param name="start"></param>
		/// <param name="subMap"></param>
		public static void InsertSubAt<T>(this T[][] map, v2i start, T[][] subMap)
		{
			for (var y = 0; y < subMap.Length; y++)
				for (var x = 0; x < subMap[y].Length; x++)
					map[start.Y + y][start.X + x] = subMap[y][x];
		}

		/// <summary>
		/// returns horizontally flipped copy
		/// </summary>
		/// <param name="map"></param>
		/// <returns></returns>
		public static T[][] FlipH<T>(this T[][] map) => [.. map.Select(x => x.Reverse().ToArray())];

		/// <summary>
		/// returns vertically flipped copy
		/// </summary>
		/// <param name="map"></param>
		/// <returns></returns>
		public static T[][] FlipV<T>(this T[][] map)
		{
			var cpy = map.Select(x => x.ToArray()).ToArray();
			var mid = cpy.Length / 2;
			for (var i = 0; i < mid; i++)
				(cpy[i], cpy[cpy.Length - 1 - i]) = (cpy[cpy.Length - 1 - i], cpy[i]);

			return cpy;
		}

		/// <summary>
		/// creates rotated copy (90deg clock-wise)
		/// </summary>
		/// <param name="map"></param>
		/// <returns></returns>
		public static T[][] RotateCW<T>(this T[][] map)
		{
			switch (map.Length)
			{
				case 1: return map.Select(x => x.ToArray()).ToArray();
				case 2: return RotateCW_2x2(map);
				case 3: return RotateCW_3x3(map);
			}

			if (map.Length % 2 == 0)
				throw new NotImplementedException($"{map.Length} % 2 == 0");

			var result = map.Select(x => x.ToArray()).ToArray();
			for (var y = 0; y < result.Length; y++)
				for (var x = 0; x < result.Length; x++)
					result[x][map.Length - y - 1] = map[y][x];

			return result;
		}

		static T[][] RotateCW_3x3<T>(T[][] map)
		{
			if (map.Length != 3)
				throw new ArgumentException(nameof(map.Length));

			var cpy = map.Select(x => x.ToArray()).ToArray();

			cpy[0][0] = map[2][0];
			cpy[0][1] = map[1][0];
			cpy[0][2] = map[0][0];

			cpy[1][0] = map[2][1];
			cpy[1][2] = map[0][1];

			cpy[2][0] = map[2][2];
			cpy[2][1] = map[1][2];
			cpy[2][2] = map[0][2];

			return cpy;
		}

		static T[][] RotateCW_2x2<T>(T[][] map)
		{
			if (map.Length != 2)
				throw new ArgumentException(nameof(map.Length));

			var cpy = map.Select(x => x.ToArray()).ToArray();

			cpy[0][0] = map[1][0];
			cpy[0][1] = map[0][0];
			cpy[1][0] = map[1][1];
			cpy[1][1] = map[0][1];

			return cpy;
		}

		/// <summary>
		/// returns v2i of given item in map
		/// </summary>
		/// <param name="map"></param>
		/// <param name="c"></param>
		/// <param name="result"></param>
		/// <returns></returns>
		public static bool Find<T>(this T[][] map, T c, out v2i result)
		{
			result = v2i.Zero;

			var y = 0;
			for (; y < map.Length; y++)
				if (map[y].Contains(c))
					break;

			if (y == map.Length)
				return false;

			for (var x = 0; x < map[y].Length; x++)
			{
				if (!map[y][x].Equals(c))
					continue;

				result = new v2i(x, y);
				return true;
			}

			return false;
		}

		/// <summary>
		/// enumerates all occurances (positions) of given character c in grid
		/// </summary>
		/// <param name="map"></param>
		/// <param name="c"></param>
		/// <returns></returns>
		public static IEnumerable<v2i> EnumAll<T>(this T[][] map, T c)
			=> new aab2i(v2i.Zero, new v2i(map[0].Length - 1, map.Length - 1)).EnumPoints().Where(x => map[x.Y][x.X].Equals(c));

		public static T Get<T>(this T[][] map, v2i p) => map[p.Y][p.X];

		public static bool TryGet<T>(this T[][] map, v2i p, out T result)
		{
			result = default;

			if (p.Y < 0 || p.X < 0 || p.Y >= map.Length || p.X >= map[p.Y].Length)
				return false;

			result = map[p.Y][p.X];
			return true;
		}

		public static void Set<T>(this T[][] map, v2i p, T c) => map[p.Y][p.X] = c;
		public static void Swap<T>(this T[][] map, v2i p1, v2i p2) => (map[p1.Y][p1.X], map[p2.Y][p2.X]) = (map[p2.Y][p2.X], map[p1.Y][p1.X]);

		public static aab2i ToAAB2i<T>(this T[][] map) => new(v2i.Zero, new v2i(map[0].Length, map.Length) - new v2i(1));

		/// <summary>
		/// creates jagged array with size [0 .. p.Max(Y)][0 .. p.Max(X)]
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="p"></param>
		/// <param name="keySelector"></param>
		/// <param name="valueSelector"></param>
		/// <returns></returns>
		public static T[][] AsJaggedArray<T>(this IEnumerable<v2i> p, Func<v2i, v2i> keySelector, Func<v2i, T> valueSelector) where T : new()
		{
			var gridSize = new v2i(p.Max(x => x.X), p.Max(x => x.Y));

			return p.AsJaggedArray(keySelector, valueSelector, gridSize);
		}

		/// <summary>
		/// creates jagged array with specified size
		/// items out of bounds [0 .. size] are ignored
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="p"></param>
		/// <param name="keySelector"></param>
		/// <param name="valueSelector"></param>
		/// <param name="size"></param>
		/// <returns></returns>
		public static T[][] AsJaggedArray<T>(this IEnumerable<v2i> p, Func<v2i, v2i> keySelector, Func<v2i, T> valueSelector, v2i size) where T : new()
		{
			var grid = new T[size.Y][];
			for (var i = 0; i < grid.Length; i++)
				grid[i] = new T[size.X];

			var bounds = new aab2i(v2i.Zero, size);

			foreach (var item in p)
			{
				if (!bounds.Contains(item))
					continue;

				grid.Set(keySelector(item), valueSelector(item));
			}

			return grid;
		}

		/// <summary>
		/// flatten jagged array to 1d array of T
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="array"></param>
		/// <returns></returns>
		public static T[] Flatten<T>(this T[][] array)
		{
			int totalLength = 0;
			for (var i = 0; i < array.Length; i++)
				totalLength += array[i].Length;

			var result = new T[totalLength];
			var span = result.AsSpan();

			var index = 0;
			for (var i = 0; i < array.Length; i++)
			{
				var row = array[i];
				var length = row.Length;

				row.CopyTo(span.Slice(index, length));
				index += length;
			}

			return result;
		}

		/// <summary>
		/// creates copy of jagged array
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="array"></param>
		/// <returns></returns>
		public static T[][] Copy<T>(this T[][] array)
		{
			var result = new T[array.Length][];

			for (var i = 0; i < array.Length; i++)
			{
				var row = array[i];

				result[i] = new T[row.Length];
				row.CopyTo(result[i], 0);
			}

			return result;
		}
	}
}
