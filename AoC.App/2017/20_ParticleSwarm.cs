using Ujeby.AoC.Common;
using Ujeby.Tools.StringExtensions;
using Ujeby.Vectors;

namespace Ujeby.AoC.App._2017_20;

[AoCPuzzle(Year = 2017, Day = 20, Answer1 = "161", Answer2 = null, Skip = false)]
public class ParticleSwarm : PuzzleBase
{
	protected override (string Part1, string Part2) SolvePuzzle(string[] input)
	{
		var particles = GetParticles(input);
		//Debug.Line($"{particles.Length} particles");

		//var t = 0;
		//var p = particles[0];
		//var pt = p.p0 + p.v0 * t + p.a * (t * (t + 1) / 2);
		//var d = pt.ManhLength();

		// part1
		// min manh. length of acceleration, and then min manh. length of initial velocity
		var answer1 = particles
			.GroupBy(x => x.a.ManhLength())
			.OrderBy(x => x.Key)
				.First()
				.OrderBy(x => x.v0.ManhLength())
					.First().i;

		// part2
		string answer2 = null;

		return (answer1.ToString(), answer2?.ToString());
	}

	public record class Particle(int i, v3i p0, v3i v0, v3i a);

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