using Ujeby.AoC.Common;

namespace Ujeby.AoC.App._2015_07
{
	[AoCPuzzle(Year = 2015, Day = 07, Answer1 = "956", Answer2 = "40149")]
	public class SomeAssemblyRequired : PuzzleBase
	{
		public class Gate
		{
			public long? Result { get; private set; } = null;

			private readonly string[] In;
			private readonly string Out;
			private readonly string Op;

			public Gate(string[] _in, string _out, string op)
			{
				In = _in;
				Out = _out;
				Op = op;
			}

			public override string ToString()
				=> $"{string.Join(",", In)} {Op} = {Out}";

			public long Eval(Dictionary<string, Gate> gates)
			{
				//Debug.Line(ToString());

				if (Result.HasValue)
					return Result.Value;

				switch (Op)
				{
					case "AND":
						{
							if (!long.TryParse(In[0], out long left))
								left = gates[In[0]].Eval(gates);

							if (!long.TryParse(In[1], out long right))
								right = gates[In[1]].Eval(gates);

							Result = left & right;
						}
						break;

					case "OR":
						{
							if (!long.TryParse(In[0], out long left))
								left = gates[In[0]].Eval(gates);

							if (!long.TryParse(In[1], out long right))
								right = gates[In[1]].Eval(gates);

							Result = left | right;
						}
						break;

					case "NOT":
						{
							if (!long.TryParse(In.Single(), out long right))
								right = gates[In.Single()].Eval(gates);

							Result = ~right;
						}
						break;

					case "LSHIFT":
						{
							if (!long.TryParse(In[0], out long left))
								left = gates[In[0]].Eval(gates);

							if (!long.TryParse(In[1], out long right))
								right = gates[In[1]].Eval(gates);

							Result = left << (int)right;
						}
						break;

					case "RSHIFT":
						{
							if (!long.TryParse(In[0], out long left))
								left = gates[In[0]].Eval(gates);

							if (!long.TryParse(In[1], out long right))
								right = gates[In[1]].Eval(gates);

							Result = left >> (int)right;
						}
						break;

					case null:
						{
							Result = long.TryParse(In.Single(), out long value) ? value : gates[In.Single()].Eval(gates);
						}
						break;

					default:
						throw new NotImplementedException(ToString());
				};

				return Result.Value;
			}

			public void Reset()
			{
				Result = null;
			}
		}

		protected override (string Part1, string Part2) SolvePuzzle(string[] input)
		{
			var gates = input.ToDictionary(i => i.Split(" -> ").Last(), i => 
			{
				var s = i.Split(" -> ");
				if (i.Contains("AND"))
					return new Gate(s[0].Replace(" AND", string.Empty).Split(' '), s[1], "AND");

				else if (i.Contains("OR"))
					return new Gate(s[0].Replace(" OR", string.Empty).Split(' '), s[1], "OR");

				else if (i.Contains("SHIFT"))
				{
					var shitSplit = s[0].Split(" ");
					return new Gate(new string[] { shitSplit[0], shitSplit[2] }, s[1], shitSplit[1]);
				}
				else if (i.Contains("NOT"))
					return new Gate(new string[] { s[0][("NOT ".Length)..] }, s[1], "NOT");

				else
					return new Gate(new string[] { s[0] }, s[1], null);
			});

			// part1
			var answer1 = gates["a"].Eval(gates);

			// part2
			foreach (var gate in gates.Values)
				gate.Reset();
			gates["b"] = new Gate(new string[] { answer1.ToString() }, "b", null);
			var answer2 = gates["a"].Eval(gates);

			return (answer1.ToString(), answer2.ToString());
		}
	}
}
