using System.Numerics;
using Ujeby.AoC.Vis.App.Common;

namespace Ujeby.AoC.Vis.App
{
	internal class Menu : BaseLoop
	{
		private readonly IRunnable[] _options;

		private Vector2 _position;

		public Menu(IRunnable[] options)
		{
			_options = options;
		}

		protected override void Init()
		{
			_position = Program.WindowSize / 2;
		}

		protected override void Update()
		{
			if (Selected != null)
				_endLoop = true;
		}

		public IRunnable Selected { get; private set; }

		private int _selected = -1;

		protected override void Render()
		{
			var items = _options.Select(o => new Text(o.GetType().Name)).ToArray();

			var topLeft = _position;
			for (var i = 0; i < items.Length; i++)
			{
				var itemSize = GetTextSize(new TextLine[] { items[i] }, Program.CurrentFont);

				if (_mouseWindow.X < topLeft.X || _mouseWindow.Y < topLeft.Y ||
					_mouseWindow.X > topLeft.X + itemSize.X || _mouseWindow.Y > topLeft.Y + itemSize.Y)
				{
					topLeft.Y += itemSize.Y;
					continue;
				}

				items[i].Color = new Vector4(1, 0, 0, 1);
				_selected = i;
				break;
			}

			DrawTextLines(_position, items);
		}

		protected override void Destroy()
		{
			
		}

		protected override void LeftMouseUp(Vector2 _mouseGrid)
		{
			if (_selected != -1)
				Selected = _options[_selected];
		}
	}
}
