using Ujeby.Graphics.Entities;
using Ujeby.Graphics.Sdl;
using Ujeby.Graphics.Sdl.Interfaces;
using Ujeby.Vectors;

namespace Ujeby.AoC.Vis.App.Common
{
	public class Menu : Sdl2Loop
	{
		private readonly IRunnable[] _options;

		private v2i _position;

		public Menu(v2i windowSize) : base(windowSize)
		{
			_position = windowSize / 2;
		}

		public Menu(v2i windowSize, IRunnable[] options) : this(windowSize)
		{
			_options = options;

		}

		protected override void Init()
		{
		}

		protected override void Update()
		{
			if (Selected != null)
				_terminate = true;
		}

		public IRunnable Selected { get; private set; }

		private int _selected = -1;

		protected override void Render()
		{
			var items = _options.Select(o => new Text(o.GetType().Name)).ToArray();

			var topLeft = _position;
			for (var i = 0; i < items.Length; i++)
			{
				var itemSize = GetTextSize(Sdl2Wrapper.CurrentFont, items[i]);

				if (MouseWindowPosition.X < topLeft.X || MouseWindowPosition.Y < topLeft.Y ||
					MouseWindowPosition.X > topLeft.X + itemSize.X || MouseWindowPosition.Y > topLeft.Y + itemSize.Y)
				{
					topLeft.Y += itemSize.Y;
					continue;
				}

				items[i].Color = new v4f(1, 0, 0, 1);
				_selected = i;
				break;
			}

			DrawText(_position, items);
		}

		protected override void Destroy()
		{

		}

		protected override void LeftMouseUp(v2i _mouseGrid)
		{
			if (_selected != -1)
				Selected = _options[_selected];
		}
	}
}
