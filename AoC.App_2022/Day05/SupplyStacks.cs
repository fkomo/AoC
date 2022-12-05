using Ujeby.AoC.Common;

namespace Ujeby.AoC.App.Day05
{
	internal class SupplyStacks : ProblemBase
	{
		protected override (string, string) SolveProblem(string[] input)
		{
			var numOfStacks = (input[0].Length + 1) / 4;

			var stacksEnd = 0;
			for (; stacksEnd < input.Length; stacksEnd++)
				if (input[stacksEnd].StartsWith(" 1"))
					break;
			var commands = input.Skip(stacksEnd + 2);

			// part1
			var stacks = new List<Stack<char>>();
			for (var s = 0; s < numOfStacks; s++)
				stacks.Add(new Stack<char>());

			var i = stacksEnd;
			while (--i >= 0)
				for (var c = 1; c < input[i].Length; c += 4)
					if (char.IsLetter(input[i][c]))
						stacks[(c - 1) / 4].Push(input[i][c]); 
			
			foreach (var cmd in commands)
			{
				var c = cmd.Replace("move", string.Empty).Replace("from", string.Empty).Replace("to", string.Empty)
					.Split(' ', StringSplitOptions.RemoveEmptyEntries)
					.Select(c => int.Parse(c))
					.ToArray();

				for (var p = 0; p < c[0]; p++)
					stacks[c[2] - 1].Push(stacks[c[1] - 1].Pop());
			}
			var result1 = string.Join("", stacks.Select(s => s.Pop()));

			// part2
			foreach (var stack in stacks)
				stack.Clear();

			i = stacksEnd;
			while (--i >= 0)
				for (var c = 1; c < input[i].Length; c += 4)
					if (char.IsLetter(input[i][c]))
						stacks[(c - 1) / 4].Push(input[i][c]);

			foreach (var cmd in commands)
			{
				var c = cmd.Replace("move", string.Empty).Replace("from", string.Empty).Replace("to", string.Empty)
					.Split(' ', StringSplitOptions.RemoveEmptyEntries)
					.Select(c => int.Parse(c))
					.ToArray();

				var tmpStack = new Stack<char>();
				for (var p = 0; p < c[0]; p++)
				{
					if (stacks[c[1] - 1].Count > 0)
						tmpStack.Push(stacks[c[1] - 1].Pop());
				}

				while (tmpStack.Count > 0)
					stacks[c[2] - 1].Push(tmpStack.Pop());
			}
			var result2 = string.Join("", stacks.Select(s => s.Pop()));

			return (result1, result2);
		}
	}
}
