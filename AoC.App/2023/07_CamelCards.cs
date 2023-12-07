using Ujeby.AoC.Common;

namespace Ujeby.AoC.App._2023_07;

[AoCPuzzle(Year = 2023, Day = 07, Answer1 = "253313241", Answer2 = "253362743", Skip = false)]
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

	record class CamelHand(string Hand, long Bid, HandType Type);

	class CamelHandComparer : Comparer<CamelHand>
	{
		protected string _cardStrength = "23456789TJQKA";

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
					return _cardStrength.IndexOf(x.Hand[i]) > _cardStrength.IndexOf(y.Hand[i]) ? 1 : -1;

			return 0;
		}
	}

	class CamelHandComparerWithJoker : CamelHandComparer
	{
		public CamelHandComparerWithJoker()
		{
			_cardStrength = "J23456789TQKA";
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
		var answer1 = hands
			.OrderBy(x => x, new CamelHandComparer())
			.Select((x, rank) => (rank + 1) * x.Bid)
			.Sum();

		// part2
		var answer2 = hands
			.Select(x => new CamelHand(x.Hand, x.Bid, GetHandTypeWithJoker(x.Hand)))
			.OrderBy(x => x, new CamelHandComparerWithJoker())
			.Select((x, rank) => (rank + 1) * x.Bid)
			.Sum();

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
		var noJokerType = GetHandType(hand);

		// no joker - no problem
		if (!hand.Contains('J'))
			return noJokerType;

		if (noJokerType == HandType.FiveOfAKind || noJokerType == HandType.FullHouse || noJokerType == HandType.FourOfAKind)
			return HandType.FiveOfAKind;

		if (noJokerType == HandType.HighCard)
			return HandType.OnePair;

		if (noJokerType == HandType.OnePair)
			return HandType.ThreeOfAKind;

		if (noJokerType == HandType.TwoPair && hand.Count(x => x == 'J') == 2)
			return HandType.FourOfAKind;

		if (noJokerType == HandType.TwoPair)
			return HandType.FullHouse;

		if (noJokerType == HandType.FullHouse && hand.Count(x => x == 'J') == 3)
			return HandType.FourOfAKind;

		if (noJokerType == HandType.ThreeOfAKind && hand.Count(x => x == 'J') == 1)
			return HandType.FourOfAKind;

		return HandType.FourOfAKind;
	}
}