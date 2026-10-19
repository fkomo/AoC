using Ujeby.AoC.Common;
using Ujeby.Extensions;

namespace Ujeby.AoC.App._2020_16
{
	[AoCPuzzle(Year = 2020, Day = 16, Answer1 = "20013", Answer2 = "5977293343129")]
	public class TicketTranslation : PuzzleBase
	{
		protected override (string, string) SolvePuzzle(string[] input)
		{
			var validations = input.Take(Array.IndexOf(input, ""))
				.Select(i => i.ToNumArray())
				.ToArray();

			var tickets = input.Skip(Array.LastIndexOf(input, "") + 2)
				.Select(i => i.ToNumArray());

			// part1
			long? answer1 = tickets.Sum(t => { ValidTicket(t, validations, out long invalidValue); return invalidValue; });

			// part2
			var myTicket = input.Skip(Array.IndexOf(input, "your ticket:") + 1).First().Split(',').Select(i => long.Parse(i)).ToArray();

			var validTickets = tickets.Where(t => ValidTicket(t, validations, out _)).ToList();
			validTickets.Add(myTicket);

			// dont need whole tickets any more, just columns of ticket values
			var columns = Enumerable.Range(0, validations.Length)
				.Select(iCol => validTickets.Select(t => t[iCol]).ToArray()).ToArray();

			// precompute all possible validations for each column
			var columnValidations = new int[columns.Length][];
			for (var iCol = 0; iCol < columns.Length; iCol++)
			{
				var matchingValidations = new List<int>();
				for (var iVal = 0; iVal < validations.Length; iVal++)
					if (columns[iCol].All(v => Validate(v, validations[iVal])))
						matchingValidations.Add(iVal);

				columnValidations[iCol] = matchingValidations.ToArray();
			}

			var order = Enumerable.Range(0, columns.Length).Select(x => -1).ToArray();
			order = FindValueOrderRecursive(0, order, columnValidations.ToArray());

			long? answer2 = Enumerable.Range(0, System.Math.Clamp(6, 0, validations.Length)).Select(x => Array.IndexOf(order, x))
				.Select(v => myTicket[v]).Aggregate((a, b) => a * b);

			return (answer1?.ToString(), answer2?.ToString());
		}

		static bool Validate(long value, long[] validation)
			=> (validation[0] <= value && value <= validation[1]) || (validation[2] <= value && value <= validation[3]);

		static bool ValidValue(long value, long[][] validations)
			=> validations.Any(v => Validate(value, v));

		static bool ValidTicket(long[] ticket, long[][] validations, out long error)
		{
			error = 0;
			foreach (var value in ticket)
			{
				if (!ValidValue(value, validations))
				{
					error = value;
					return false;
				}
			}

			return true;
		}

		private int[] FindValueOrderRecursive(int i, int[] order, int[][] columnValidations)
		{
			if (i == columnValidations.Length)
				return order;

			for (var iColVal = 0; iColVal < columnValidations[i].Length; iColVal++)
			{
				var iVal = columnValidations[i][iColVal];
				if (order.Contains(iVal))
					continue;

				order[i] = iVal;
				var result = FindValueOrderRecursive(i + 1, order.ToArray(), columnValidations);
				if (result != null)
					return result;
			}

			return null;
		}
	}
}
