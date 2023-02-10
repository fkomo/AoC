using Ujeby.AoC.Common;

namespace Ujeby.AoC.App._2022_05
{
	internal class SupplyStacks : PuzzleBase
	{
		protected override (string, string) SolvePuzzle(string[] input)
		{
			var numOfStacks = (input[0].Length + 1) / 4;
			var stacksEnd = Array.IndexOf(input, "") - 1;

			// part1
			var stacks = new List<Stack<char>>();
			for (var s = 0; s < numOfStacks; s++)
				stacks.Add(new Stack<char>());

			var i = stacksEnd;
			while (--i >= 0)
				for (var c = 1; c < input[i].Length; c += 4)
					if (char.IsLetter(input[i][c]))
						stacks[(c - 1) / 4].Push(input[i][c]); 
			
			foreach (var cmd in input.Skip(stacksEnd + 2))
			{
				var c = cmd.Replace("move", string.Empty).Replace("from", string.Empty).Replace("to", string.Empty)
					.Split(' ', StringSplitOptions.RemoveEmptyEntries)
					.Select(c => int.Parse(c))
					.ToArray();

				for (var p = 0; p < c[0]; p++)
					stacks[c[2] - 1].Push(stacks[c[1] - 1].Pop());
			}
			var answer1 = string.Join("", stacks.Select(s => s.Pop()));

			// part2
			foreach (var stack in stacks)
				stack.Clear();

			i = stacksEnd;
			while (--i >= 0)
				for (var c = 1; c < input[i].Length; c += 4)
					if (char.IsLetter(input[i][c]))
						stacks[(c - 1) / 4].Push(input[i][c]);

			foreach (var cmd in input.Skip(stacksEnd + 2))
			{
				var c = cmd.Replace("move", string.Empty).Replace("from", string.Empty).Replace("to", string.Empty)
					.Split(' ', StringSplitOptions.RemoveEmptyEntries)
					.Select(c => int.Parse(c))
					.ToArray();

				var tmpStack = new Stack<char>();
				for (var p = 0; p < c[0]; p++)
					if (stacks[c[1] - 1].Count > 0)
						tmpStack.Push(stacks[c[1] - 1].Pop());

				while (tmpStack.Count > 0)
					stacks[c[2] - 1].Push(tmpStack.Pop());
			}
			var answer2 = string.Join("", stacks.Select(s => s.Pop()));

			return (answer1, answer2);
		}
	}
}
