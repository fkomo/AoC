using Ujeby.AoC.Common;

namespace Ujeby.AoC.App.Day02
{
	internal class Dive : ProblemBase
	{
		protected override (long, long) SolveProblem()
		{
			var input = ReadInputLines();
			DebugLine($"{ input.Length } commands");

			long depth = 0;
			long position = 0;
			foreach (var command in input)
			{
				var commandParts = command.Split(' ');
				var value = int.Parse(commandParts[1]);
				switch (commandParts[0])
				{
					case "forward": position += value; break;
					case "down": depth += value; break;
					case "up": depth -= value; break;
				}
			}

			long result1 = position * depth;

			depth = 0;
			position = 0;
			long aim = 0;
			foreach (var command in input)
			{
				var commandParts = command.Split(' ');
				var value = int.Parse(commandParts[1]);
				switch (commandParts[0])
				{
					case "forward":
						{
							position += value;
							depth += aim * value;
						}
						break;
					case "down": aim += value; break;
					case "up": aim -= value; break;
				}
			}

			long result2 = position * depth;

			return (result1, result2);
		}
	}
}
