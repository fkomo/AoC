using System.Collections.Generic;

namespace Ujeby.AoC.App.Year2022.Day09
{
	public static class Directions
	{
		/// <summary>
		/// 4 x,y directions (90deg)
		/// </summary>
		public readonly static Dictionary<char, int[]> NSWE = new()
		{
			{ 'W', new[] { -1, 0 } },
			{ 'E', new[] { 1, 0 } },
			{ 'S', new[] { 0, -1 } },
			{ 'N', new[] { 0, 1 } },

			//{ 'E', new[] { 1, 0 } },
			//{ 'S', new[] { 0, -1 } },
			//{ 'N', new[] { 0, 1 } },
			//{ 'W', new[] { -1, 0 } },
		};

		/// <summary>
		/// 8 x,y directions (45deg)
		/// </summary>
		public readonly static Dictionary<string, int[]> All = new()
		{
			{ "N", new[] { 0, 1 } },
			{ "S", new[] { 0, -1 } },
			{ "W", new[] { -1, 0 } },
			{ "E", new[] { 1, 0 } },
			{ "NW", new[] { -1, 1 } },
			{ "NE", new[] { 1, 1 } },
			{ "SW", new[] { -1, -1 } },
			{ "SE", new[] { 1, -1 } },
		};

		/// <summary>
		/// 
		/// </summary>
		public readonly static Dictionary<char, char> OppositeNSWE = new()
		{
			{ 'W', 'E'},
			{ 'E', 'W' },
			{ 'S', 'N' },
			{ 'N', 'S' },
		};
	}
}