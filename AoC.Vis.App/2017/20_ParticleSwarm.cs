using Ujeby.AoC.Common;
using Ujeby.AoC.Vis.App.Common;
using Ujeby.AoC.Vis.App.Ui;
using Ujeby.Graphics;
using Ujeby.Graphics.Entities;
using Ujeby.Graphics.Sdl;
using Ujeby.Vectors;

namespace Ujeby.AoC.Vis.App
{
	internal class ParticleSwarm : AoCRunnable
	{
		long _t = 0;
		AoC.App._2017_20.ParticleSwarm.Particle[] _particles;

		long[] _min;
		long[] _pt;

		public override string Name => $"#20 {nameof(ParticleSwarm)}";

		public ParticleSwarm(v2i windowSize) : base(windowSize)
		{
			Sdl2Wrapper.ShowCursor(false);
		}

		protected override void Init()
		{
			var input = InputProvider.Read(AppSettings.InputDirectory, 2017, 20);
			_particles = Ujeby.AoC.App._2017_20.ParticleSwarm.GetParticles(input);

			_min = _particles.Select(x => long.MaxValue).ToArray();
			_pt = _particles.Select(x => x.p0.ManhLength()).ToArray();
		}

		protected override void Update()
		{
			_t++;
			for (var i = 0; i < _particles.Length; i++)
			{
				var p = _particles[i];

				_pt[i] = (p.p0 + p.v0 * _t + p.a * (_t * (_t + 1) / 2)).ManhLength();
				_min[i] = System.Math.Min(_pt[i], _min[i]);
			}
		}

		protected override void Render()
		{
			Grid.Draw();

			var fill = Colors.White;
			fill.W = .1;

			var min = Colors.Green;
			min.W = .5;

			var border = Colors.DarkGray;
			fill.W = .1;

			var start = new v2i(0, 0);
			for (var i = 0; i < _particles.Length; i++)
			{
				//Grid.DrawRect(start + v2i.Right * i, new v2i(1, -_pt[i]), border: border, fill: fill);
				Grid.DrawRect(start + v2i.Right * i, new v2i(1, -_min[i]), border: border, fill: min);
			}

			Grid.DrawMouseCursor(style: GridCursorStyles.SimpleFill);

			Sdl2Wrapper.DrawText(new v2i(32, 32), null,
				new Text($"t: {_t}")
				);

			base.Render();
		}

		protected override void LeftMouseUp()
		{

		}

		protected override void Destroy()
		{
			Sdl2Wrapper.ShowCursor();
		}
	}
}
