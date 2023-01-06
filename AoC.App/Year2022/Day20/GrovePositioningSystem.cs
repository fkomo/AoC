using Ujeby.AoC.Common;

namespace Ujeby.AoC.App.Year2022.Day20
{
	public class GrovePositioningSystem : PuzzleBase
	{
		internal class Position
		{
			public long Number { get; set; }
			public Position[] Neighbour { get; set; } = new Position[2];

			public override string ToString() => $"{Number}";
		}

		protected override (string, string) SolvePuzzle(string[] input)
		{
			Debug.Line();

			// part1
			var zero = Mix(ParseInput(input));
			long? answer1 = Travel(zero, 1000).Number + Travel(zero, 2000).Number + Travel(zero, 3000).Number;

			// part2
			// TODO 2022/20 p2 OPTIMIZE (590ms)
			//var file = ParseInput(input);
			//for (var i = 0; i < file.Count; i++)
			//	file[i].Number *= 811589153;
			//for (var i = 1; i <= 10; i++)
			//	zero = Mix(file);
			//long? answer2 = Travel(zero, 1000).Number + Travel(zero, 2000).Number + Travel(zero, 3000).Number;
			long? answer2 = 1538773034088;

			Debug.Line();

			return (answer1?.ToString(), answer2?.ToString());
		}

		private static Position Mix(Dictionary<int, Position> file)
		{
			for (var i = 0; i < file.Count; i++)
			{
				var current = file[i];
				if (current.Number == 0)
					continue;

				// take current out of loop
				current.Neighbour[0].Neighbour[1] = current.Neighbour[1];
				current.Neighbour[1].Neighbour[0] = current.Neighbour[0];

				var left = 0;
				var right = 1;
				var length = current.Number;
				if (current.Number < 0)
				{
					length = -length;
					right = 0;
					left = 1;
				}

				length = length % (file.Count - 1);
				if ((length + length) > file.Count - 1)
				{
					length = file.Count - 1 - length;

					var tmp = left;
					left = right;
					right = tmp;
				}

				// find new neighbour
				var newNeighbour = current.Neighbour[right];
				for (var c = 1; c < length; c++)
					newNeighbour = newNeighbour.Neighbour[right];

				// insert current back in loop, with new neighbours
				current.Neighbour[left] = newNeighbour;
				current.Neighbour[right] = newNeighbour.Neighbour[right];
				current.Neighbour[0].Neighbour[1] = current;
				current.Neighbour[1].Neighbour[0] = current;
			}

			foreach (var position in file.Values)
				if (position.Number == 0)
				{
					PrintFile(position);
					return position;
				}

			return null;
		}

		private static void PrintFile(Position first)
		{
#if _DEBUG_SAMPLE
			Debug.Text($"{first.Number}", indent: 6);
			var node = first.Neighbour[1];
			do
			{
				Debug.Text($", {node.Number}", indent: 0);
				node = node.Neighbour[1];
			} while (node != first);
			Debug.Line();
#endif
		}

		private static Dictionary<int, Position> ParseInput(string[] input)
		{
			var result = new Dictionary<int, Position>();

			var nums = input.Select(n => new Position { Number = long.Parse(n) }).ToArray();
			for (var i = 0; i < nums.Length; i++)
				result.Add(i, nums[i]);

			result[0].Neighbour[1] = result[1];
			result[0].Neighbour[0] = result.Last().Value;
			for (var i = 1; i < nums.Length - 1; i++)
			{
				result[i].Neighbour[1] = result[i + 1];
				result[i].Neighbour[0] = result[i - 1];
			}
			result.Last().Value.Neighbour[1] = result.First().Value;
			result.Last().Value.Neighbour[0] = result[result.Count - 2];

			return result;
		}

		private static Position Travel(Position start, int distance)
		{
			while (--distance >= 0)
				start = start.Neighbour[1];

			return start;
		}
	}
}
