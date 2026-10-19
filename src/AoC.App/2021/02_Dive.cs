using Ujeby.AoC.Common;

namespace Ujeby.AoC.App._2021_02
{
	[AoCPuzzle(Year = 2021, Day = 02, Answer1 = "1868935", Answer2 = "1965970888")]
	public class Dive : PuzzleBase
    {
        protected override (string, string) SolvePuzzle(string[] input)
        {
            // part1
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

            long answer1 = position * depth;

            // part2
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

            long answer2 = position * depth;

            return (answer1.ToString(), answer2.ToString());
        }
    }
}
