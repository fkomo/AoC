using Ujeby.Graphics.Entities;
using Ujeby.Graphics.Sdl;
using Ujeby.Graphics.Sdl.Interfaces;
using Ujeby.Tools;
using Ujeby.Vectors;

namespace Ujeby.AoC.Vis.App.Ui
{
	public class AoCMenu : Sdl2Loop
	{
		private readonly Dictionary<string, IRunnable[]> _items;

		public AoCMenu(v2i windowSize) : base(windowSize)
		{
		}

		public AoCMenu(v2i windowSize, Dictionary<string, IRunnable[]> items) : this(windowSize)
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

		public IRunnable Selected { get; private set; }

		private string _section = null;
		private int _sectionItem = -1;

		protected override void Render()
		{
			//DrawRect((int)_windowSize.X / (_items.Keys.Count + 1), 0, (int)_windowSize.X / (_items.Keys.Count + 1), (int)_windowSize.Y, 
			//	new v4f(0, 1, 0, 0.5f));
			//DrawRect(0, (int)_windowSize.Y / 2, (int)_windowSize.X, (int)_windowSize.Y / 2, 
			//	new v4f(0, 0, 1, 0.5f));

			var spacing = new v2i(0, 4);

			for (var ix = 0; ix < _items.Keys.Count; ix++)
			{
				var sectionTitle = _items.Keys.ElementAt(ix);
				var sectionCenter = new v2i(WindowSize.X / (_items.Keys.Count + 1) * (ix + 1), WindowSize.Y / 2);

				var items = _items[sectionTitle].Select(o => new Text(Strings.SplitCase(o.GetType().Name))).ToArray();
				var itemsSize = Sdl2Wrapper.CurrentFont.GetTextSize(spacing, items);

				var sectionTopLeft = sectionCenter - itemsSize / 2;
				for (var i = 0; i < items.Length; i++)
				{
					var itemSize = Sdl2Wrapper.CurrentFont.GetTextSize(spacing, items[i]);

					if (MouseWindowPosition.X < sectionTopLeft.X || MouseWindowPosition.Y < sectionTopLeft.Y ||
						MouseWindowPosition.X > sectionTopLeft.X + itemSize.X || MouseWindowPosition.Y > sectionTopLeft.Y + itemSize.Y)
					{
						sectionTopLeft.Y += itemSize.Y;
						continue;
					}

					items[i].Color = new v4f(1, 0, 0, 1);

					_section = sectionTitle;
					_sectionItem = i;
					break;
				}

				var title = new Text($"-=#{{ {sectionTitle} }}#=-") { Color = new v4f(0, 1, 0, 1) };
				var titleSize = Sdl2Wrapper.CurrentFont.GetTextSize(spacing, title);
				var titleOffset = new v2i(titleSize.X / 2, titleSize.Y + itemsSize.Y / 2);

				// title
				DrawText(sectionCenter - titleOffset, spacing, title);

				// items
				DrawText(sectionCenter - itemsSize / 2, spacing, items);
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
