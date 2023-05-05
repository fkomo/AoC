using Ujeby.Graphics;
using Ujeby.Graphics.Entities;
using Ujeby.Graphics.Interfaces;
using Ujeby.Graphics.Sdl;
using Ujeby.Vectors;

namespace Ujeby.AoC.Vis.App.Ui
{
	public class AoCMenu : AoCRunnable
	{
		private readonly Dictionary<string, AoCRunnable[]> _items;

		public AoCMenu(v2i windowSize) : base(windowSize)
		{
		}

		public AoCMenu(v2i windowSize, Dictionary<string, AoCRunnable[]> items) : this(windowSize)
		{
			_items = items;
		}

		protected override void Init()
		{
		}

		protected override void Update()
		{
			if (Selected != null)
				_terminate = true;
		}

		public AoCRunnable Selected { get; private set; }

		public override string Name => "-=#[ Advent Of Code ]#=-";

		private string _section = null;
		private int _sectionItem = -1;

		protected override void Render()
		{
			var cGreen = new v4f(0, .5, 0, 1);
			var cSelectedItem = new v4f(0, .75, 0, 1);

			var title = new Text(Name, Colors.White, cGreen);
			Sdl2Wrapper.DrawText(new(WindowSize.X / 2, WindowSize.Y / 5), new(), new v2i(4, 4), 
				HorizontalTextAlign.Center, VerticalTextAlign.Center, 
				title);

			var spacing = new v2i(0, 4);
			var scale = new v2i(2);

			for (var ix = 0; ix < _items.Keys.Count; ix++)
			{
				var sectionTitleText = _items.Keys.ElementAt(ix);
				var sectionCenter = new v2i(WindowSize.X / (_items.Keys.Count + 1) * (ix + 1), WindowSize.Y / 2);

				var items = _items[sectionTitleText].Select(i => new Text(i.Name)).ToArray();
				var itemsSize = Sdl2Wrapper.CurrentFont.GetTextSize(spacing, scale, items);

				var sectionTopLeft = sectionCenter - itemsSize / 2;
				for (var i = 0; i < items.Length; i++)
				{
					var itemSize = Sdl2Wrapper.CurrentFont.GetTextSize(spacing, scale, items[i]);

					if (MousePosition.X < sectionTopLeft.X || MousePosition.Y < sectionTopLeft.Y ||
						MousePosition.X > sectionTopLeft.X + itemSize.X || MousePosition.Y > sectionTopLeft.Y + itemSize.Y)
					{
						sectionTopLeft.Y += itemSize.Y;
						continue;
					}

					items[i].OutlineColor = cSelectedItem;

					_section = sectionTitleText;
					_sectionItem = i;
					break;
				}

				var sectionTitle = new Text($"-=#{{ {sectionTitleText} }}#=-", cGreen);
				var sectionTitleSize = Sdl2Wrapper.CurrentFont.GetTextSize(spacing, scale, sectionTitle);
				var titleOffset = new v2i(sectionTitleSize.X / 2, sectionTitleSize.Y + itemsSize.Y / 2);

				// title
				Sdl2Wrapper.DrawText(sectionCenter - titleOffset, spacing, scale, sectionTitle);

				// items
				Sdl2Wrapper.DrawText(sectionCenter - itemsSize / 2, spacing, scale, items);
			}
		}

		protected override void Destroy()
		{
		}

		protected override void LeftMouseUp()
		{
			if (_sectionItem != -1)
				Selected = _items[_section][_sectionItem];
		}
	}
}
