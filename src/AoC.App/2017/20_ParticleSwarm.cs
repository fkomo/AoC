using Ujeby.AoC.Common;
using Ujeby.Extensions;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2017_20;

[AoCPuzzle(Year = 2017, Day = 20, Answer1 = "161", Answer2 = "438", Skip = false)]
public class ParticleSwarm : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var particles = GetParticles(input);

		// part1
		// min manh. length of acceleration, and then min manh. length of initial velocity
		var answer1 = particles
			.GroupBy(x => x.Acc.ManhLength())
			.OrderBy(x => x.Key)
				.First()
				.OrderBy(x => x.Vel.ManhLength())
					.First().Id;

		// part2
		var magicNumberOfIterations = 100;
		var alive = particles.Select(x => true).ToArray();
		for (var t = 0; t < magicNumberOfIterations; t++)
		{
			// move all particles
			for (var i = 0; i < particles.Length; i++)
			{
				if (!alive[i])
					continue;

				particles[i].Move();
			}

			// find collisions
			var collisions = new HashSet<int>();
			for (var p1 = 0; p1 < particles.Length; p1++)
			{
				if (!alive[p1])
					continue;

				for (var p2 = 0; p2 < p1; p2++)
				{
					if (!alive[p2])
						continue;

					if (particles[p1].Pos == particles[p2].Pos)
					{
						collisions.Add(p1);
						collisions.Add(p2);
					}
				}
			}

			// destroy particles
			foreach (var c in collisions)
				alive[c] = false;
		}

		var answer2 = alive.Count(x => x);

		return (answer1.ToString(), answer2.ToString());
	}

	public class Particle(int id, v3i pos, v3i vel, v3i acc)
	{
		public int Id { get; private set; } = id;
		public v3i Pos { get; private set; } = pos;
		public v3i Vel { get; private set; } = vel;
		public v3i Acc { get; private set; } = acc;

		public void Move()
		{
			Vel += Acc;
			Pos += Vel;
		}
	}

	public static Particle[] GetParticles(string[] input)
	{
		var i = 0;
		return input
			.Select(x =>
			{
				var n = x.ToNumArray();
				return new Particle(i++, new v3i(n.Take(3).ToArray()), new v3i(n.Skip(3).Take(3).ToArray()), new v3i(n.Skip(6).Take(3).ToArray()));
			})
			.ToArray();
	}
}