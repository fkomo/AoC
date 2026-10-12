using Ujeby.AoC.Common;
using Ujeby.Extensions;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2018_13;

[AoCPuzzle(Year = 2018, Day = 13, Answer1 = "83,106", Answer2 = "132,26", Skip = false)]
public class MineCartMadness : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		// part1
		ParseInput(input, out Cart[] carts, out char[][] map);

		v2i collision;
		while (MoveCartsAndCheckCollision(carts, map, out collision))
		{ 
			// no collision yet
		}
		var answer1 = $"{collision.X},{collision.Y}";

		// part2
		ParseInput(input, out carts, out _);
		while (carts.Length > 1)
		{
			carts = MoveAndRemoveCollisionCarts(carts, map);
		}
		var answer2 = $"{carts[0].Pos.X},{carts[0].Pos.Y}";

		return (answer1?.ToString(), answer2?.ToString());
	}

	public static void ParseInput(string[] input, out Cart[] carts, out char[][] map)
	{
		map = [.. input.Select(x => x.ToCharArray())];

		carts =
		[
			.. map.EnumAll('>').Select(x => new Cart(v2i.Right, x)),
			.. map.EnumAll('v').Select(x => new Cart(v2i.Up, x)),
			.. map.EnumAll('<').Select(x => new Cart(v2i.Left, x)),
			.. map.EnumAll('^').Select(x => new Cart(v2i.Down, x)),
		];

		// clean map from carts
		foreach (var c in carts)
			map.Set(c.Pos, c.Dir == v2i.Right || c.Dir == v2i.Left ? '-' : '|');
	}

	public static bool MoveCartsAndCheckCollision(Cart[] carts, char[][] map, out v2i collision)
	{
		collision = v2i.Zero;

		foreach (var cart in carts)
		{
			// find next direction
			var tile = map.Get(cart.Pos);
			if (tile == '+')
				cart.Crossroad();

			else if (tile == '\\' || tile == '/')
				cart.Turn(tile);

			cart.Move();

			if (CheckCollision(carts, out Cart[] collisionCarts))
			{
				collision = collisionCarts[0].Pos;
				return false;
			}
		}

		return true;
	}

	public static Cart[] MoveAndRemoveCollisionCarts(Cart[] carts, char[][] map)
	{
		var safeCarts = new List<Cart>();
		var collided = new List<Cart>();

		foreach (var cart in carts)
		{
			if (collided.Contains(cart))
				continue;

			// find next direction
			var tile = map.Get(cart.Pos);
			if (tile == '+')
				cart.Crossroad();

			else if (tile == '\\' || tile == '/')
				cart.Turn(tile);

			cart.Move();

			if (CheckCollision([.. carts.Except(collided)], out Cart[] collision))
			{
				foreach (var c in collision)
				{
					safeCarts.Remove(c);
					collided.Add(c);
				}
			}
			else
				safeCarts.Add(cart);
		}

		return [.. safeCarts];
	}

	static bool CheckCollision(Cart[] carts, out Cart[] collision)
	{
		collision = [];

		// collision check
		foreach (var c in carts)
		{
			var another = carts.Where(x => x != c && x.Pos == c.Pos);
			if (another.Any())
			{
				collision = [c, .. another];
				return true;
			}	
		}

		return false;
	}
}

public class Cart(v2i Dir, v2i Pos, int NextTurn = 0)
{
	public v2i Dir { get; set; } = Dir;
	public v2i Pos { get; set; } = Pos;
	public int NextTurn { get; set; } = NextTurn;

	public void Move()
	{
		Pos += Dir;
	}

	public void Turn(char turn)
	{
		if (turn == '/')
		{
			if (Dir.Y == 0)
				TurnLeft();
			else
				TurnRight();
		}
		else if (turn == '\\')
		{
			if (Dir.Y == 0)
				TurnRight();
			else
				TurnLeft();
		}
	}

	public void TurnRight()
	{
		Dir = Dir.RotateCCW();
	}

	public void TurnLeft()
	{
		Dir = Dir.RotateCW();
	}

	public void Crossroad()
	{
		switch (NextTurn)
		{
			case 0: TurnLeft(); break;
			case 1: break;
			case 2: TurnRight(); break;
		}

		NextTurn = (NextTurn + 1) % 3;
	}
}