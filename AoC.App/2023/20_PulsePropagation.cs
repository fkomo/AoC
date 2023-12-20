using Ujeby.AoC.Common;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2023_20;

[AoCPuzzle(Year = 2023, Day = 20, Answer1 = "825896364", Answer2 = null, Skip = false)]
public class PulsePropagation : PuzzleBase
{
	const string _broadcaster = "broadcaster";
	const int _low = 0;
	const int _high = 1;

	record class Pulse(string From, string To, bool High)
	{
		public override string ToString()
			=> $"{From} -{(High ? "high" : "low")}> {To}";
	}

	class Module
	{
		public string Name { get; private set; }
		public string[] Output { get; set; }

		public Module(string name, string[] output)
		{
			Name = name;
			Output = output.ToArray();
		}

		public virtual Pulse[] Process(Dictionary<string, Module> modules, bool pulse, string from)
			=> Array.Empty<Pulse>();

		public static Module Create(string s)
		{
			var name = s[1..s.IndexOf(' ')];
			var output = s.Split(" -> ").Last().Split(", ");
			return s[0] switch
			{
				'b' => new Broadcaster(output),
				'%' => new FlipFlop(name, output),
				'&' => new Conjuncton(name, output),

				_ => throw new NotImplementedException(s),
			};
		}
	}

	class FlipFlop : Module
	{
		public bool State { get; set; }

		public FlipFlop(string name, string[] output) : base(name, output)
		{
			State = false;
		}

		public override Pulse[] Process(Dictionary<string, Module> modules, bool pulse, string from)
		{
			if (pulse)
				return base.Process(modules, pulse, from);

			State = !State;
			return Output.Select(x => new Pulse(Name, x, State)).ToArray();
		}
	}

	class Conjuncton : Module
	{
		public Dictionary<string, bool> Input { get; set; }

		public Conjuncton(string name, string[] output) : base(name, output)
		{
		}

		public override Pulse[] Process(Dictionary<string, Module> modules, bool pulse, string from)
		{
			if (Input.ContainsKey(from))
				Input[from] = pulse;
			else
				Input.Add(from, pulse);

			var output = Input.All(x => x.Value);
			return Output.Select(x => new Pulse(Name, x, !output)).ToArray();
		}

		public void SetInputs(string[] inputs)
		{
			Input = inputs.ToDictionary(x => x, x => false);
		}
	}

	class Broadcaster : FlipFlop
	{
		public Broadcaster(string[] output) : base(_broadcaster, output)
		{
		}

		public override Pulse[] Process(Dictionary<string, Module> modules, bool pulse, string from)
		{
			State = pulse;
			return Output.Select(x => new Pulse(Name, x, State)).ToArray();
		}
	}

	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		// part1
		var modules = CreateModules(input);

		var pulses = new v2i(0);
		var pulseQueue = new Queue<Pulse>();
		for (var i = 0; i < 1000; i++)
		{
			pulseQueue.Enqueue(new Pulse("button", _broadcaster, false));
			while (pulseQueue.Count > 0)
			{
				var pulse = pulseQueue.Dequeue();
#if DEBUG
				//Debug.Line(pulse.ToString());
#endif
				if (pulse.High)
					pulses[_high]++;
				else
					pulses[_low]++;

				// output fix
				if (!modules.ContainsKey(pulse.To))
					continue;

				var newImp = modules[pulse.To].Process(modules, pulse.High, pulse.From);
				foreach (var ii in newImp)
					pulseQueue.Enqueue(ii);
			}
		}
		Debug.Line($"pulses: {pulses}");
		var answer1 = pulses.Area();

		// part2
		modules = CreateModules(input);

		long? answer2 = null;
		// TODO 2023/20 p2

		return (answer1.ToString(), answer2?.ToString());
	}

	static long FewestNumOfPressesToRx(Dictionary<string, Module> modules)
	{
		var buttonPresses = 0;

		var lowRx = false;
		var pulseQueue = new Queue<Pulse>();
		while (!lowRx)
		{
			pulseQueue.Enqueue(new Pulse("button", _broadcaster, false));
			buttonPresses++;

			while (pulseQueue.Count > 0)
			{
				var pulse = pulseQueue.Dequeue();

				// output fix
				if (pulse.To == "rx")
				{
					if (!pulse.High)
					{
						lowRx = true;
						break;
					}

					continue;
				}

				var newImp = modules[pulse.To].Process(modules, pulse.High, pulse.From);
				foreach (var ii in newImp)
					pulseQueue.Enqueue(ii);
			}
		}

		return buttonPresses;
	}

	static Dictionary<string, Module> CreateModules(string[] input)
	{
		var modules = input.ToDictionary(
			x => x[0] == 'b' ? _broadcaster : x[1..x.IndexOf(' ')],
			x => Module.Create(x));
		Debug.Line($"{modules.Count} modules");

		FixConunctionInput(modules);

		return modules;
	}

	static void FixConunctionInput(Dictionary<string, Module> modules)
	{
		foreach (var con in modules.Where(x => x.Value is Conjuncton))
		{
			var inputs = modules
				.Where(x => x.Value.Output.Contains(con.Key))
				.Select(x => x.Key)
				.ToArray();

			(con.Value as Conjuncton).SetInputs(inputs);
			Debug.Line($"conj {con.Key}: {inputs.Length} inputs");
		}
	}
}