using Ujeby.AoC.Common;

namespace Ujeby.AoC.App._2023_07;

[AoCPuzzle(Year = 2023, Day = 07, Answer1 = "253313241", Answer2 = null, Skip = false)]
public class CamelCards : PuzzleBase
{
	enum HandType : int
	{
		HighCard = 0,
		OnePair,
		TwoPair,
		ThreeOfAKind,
		FullHouse,
		FourOfAKind,
		FiveOfAKind
	}

	record class CamelHand(string Hand, long Bid, HandType Type)
	{
		public override string ToString() => $"{Hand} {Type,-15} {Bid}";
	}

	class CamelHandComparer : Comparer<CamelHand>
	{
		protected char[] _cardStrength;

		public CamelHandComparer() : base()
		{
			_cardStrength = new char[]
			{
				'2',
				'3',
				'4',
				'5',
				'6',
				'7',
				'8',
				'9',
				'T',
				'J',
				'Q',
				'K',
				'A',
			};
		}

		public override int Compare(CamelHand x, CamelHand y)
		{
			if (x.Type > y.Type)
				return 1;

			if (x.Type < y.Type)
				return -1;

			if (x.Hand == y.Hand)
				return 0;

			for (var i = 0; i < x.Hand.Length; i++)
				if (x.Hand[i] != y.Hand[i])
					return Array.IndexOf(_cardStrength, x.Hand[i]) > Array.IndexOf(_cardStrength, y.Hand[i]) ? 1 : -1;

			return 0;
		}
	}

	class CamelHandComparerWithJoker : CamelHandComparer
	{
		public CamelHandComparerWithJoker() : base()
		{
			_cardStrength = new char[]
			{
				'J',
				'2',
				'3',
				'4',
				'5',
				'6',
				'7',
				'8',
				'9',
				'T',
				'Q',
				'K',
				'A',
			};
		}
	}

	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var hands = input.Select(x =>
		{
			var split = x.Split(' ');
			return new CamelHand(split[0], long.Parse(split[1]), GetHandType(split[0]));
		});

		// part1
		long answer1 = 0;
		var camelHandComparer = new CamelHandComparer();
		var oHands = hands
			.OrderBy(x => x, camelHandComparer)
			.ToArray();

		for (var i = 0; i < oHands.Length; i++)
		{
			var rank = i + 1;
			answer1 += rank * oHands[i].Bid;
			//Debug.Line($"{rank,5}: {oHands[i],-30} winning += {rank * oHands[i].Bid}");
		}

		// part2
		long answer2 = 0;
		var camelHandComparerWithJoker = new CamelHandComparerWithJoker();
		oHands = hands
			.Select(x => new CamelHand(x.Hand, x.Bid, GetHandTypeWithJoker(x.Hand)))
			.OrderBy(x => x, camelHandComparerWithJoker)
			.ToArray();

		for (var i = 0; i < oHands.Length; i++)
		{
			var rank = i + 1;
			answer2 += rank * oHands[i].Bid;
			Debug.Line($"{rank,5}: {oHands[i],-30} winning += {rank * oHands[i].Bid}");
		}

		return (answer1.ToString(), answer2.ToString());
	}

	static HandType GetHandType(string hand)
	{
		var group = hand.GroupBy(x => x).ToArray();

		if (group.Length == 1)
			return HandType.FiveOfAKind;
		else if (group.Length == 2 && group.Any(x => x.Count() == 4))
			return HandType.FourOfAKind;
		else if (group.Length == 2 && group.Any(x => x.Count() == 3))
			return HandType.FullHouse;
		else if (group.Length == 3 && group.Any(x => x.Count() == 3))
			return HandType.ThreeOfAKind;
		else if (group.Length == 3 && group.Any(x => x.Count() == 2))
			return HandType.TwoPair;
		else if (group.Length == 4)
			return HandType.OnePair;

		return HandType.HighCard;
	}

	static HandType GetHandTypeWithJoker(string hand)
	{
		var group = hand.GroupBy(x => x).ToArray();

		if (group.Length == 1)
			return HandType.FiveOfAKind;
		else if (group.Length == 2 && group.Any(x => x.Count() == 4))
			return HandType.FourOfAKind;
		else if (group.Length == 2 && group.Any(x => x.Count() == 3))
			return HandType.FullHouse;
		else if (group.Length == 3 && group.Any(x => x.Count() == 3))
			return HandType.ThreeOfAKind;
		else if (group.Length == 3 && group.Any(x => x.Count() == 2))
			return HandType.TwoPair;
		else if (group.Length == 4)
			return HandType.OnePair;

		return HandType.HighCard;
	}
}

