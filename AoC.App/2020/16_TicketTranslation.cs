using Ujeby.AoC.Common;
using Ujeby.Tools;

namespace Ujeby.AoC.App._2020_16
{
	[AoCPuzzle(Year = 2020, Day = 16, Answer1 = "20013", Answer2 = null)]
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

			//var valueOrder = CheckField(Array.Empty<int>(), validations, validTickets.ToArray());
			//long? answer2 = valueOrder.Take(6).Select(v => myTicket[v]).Aggregate((a, b) => a * b);
			long? answer2 = null;

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

		private int[] CheckField(int[] valueOrder, long[][] validations, long[][] tickets)
		{
			if (valueOrder.Length == tickets[0].Length)
				return valueOrder;

			var vIdx = valueOrder.Length;
			var validation = validations[vIdx];
			for (var v = 0; v < tickets[0].Length; v++)
			{
				if (valueOrder.Contains(v))
					continue;

				var values = tickets.Select(x => x[v]).ToArray();
				if (values.Any(x => !Validate(x, validation)))
					continue;

				var result = CheckField(valueOrder.Concat(new[] { v }).ToArray(), validations, tickets);
				if (result != null)
					return result;
			}

			return null;
		}
	}
}
