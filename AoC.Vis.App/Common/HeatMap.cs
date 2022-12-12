using System.Drawing;

namespace Ujeby.AoC.Vis.App.Common
{
	public static class HeatMap
	{
		public readonly static byte Alpha = 0xff;
		private static readonly List<Color> _colors = new();

		static HeatMap()
		{
			initColorsBlocks();
		}

		private static void initColorsBlocks()
		{
			_colors.AddRange(new Color[]{
				//Color.FromArgb(Alpha, 0, 0, 0),

				Color.FromArgb(Alpha, 0x00, 0x00, 0xff),
				Color.FromArgb(Alpha, 0x00, 0xff, 0xff),
				Color.FromArgb(Alpha, 0x00, 0xff, 0x00),

				//Color.FromArgb(Alpha, 0xff, 0xff, 0x00),
				//Color.FromArgb(Alpha, 0xff, 0x00, 0x00),

				//Color.FromArgb(Alpha, 0xff, 0xff, 0xff)
			});
		}

		public static Color GetColorForValue(double val, double maxVal)
		{
			double valPerc = val / maxVal;// value%
			double colorPerc = 1d / (_colors.Count - 1);// % of each block of color. the last is the "100% Color"
			double blockOfColor = valPerc / colorPerc;// the integer part repersents how many block to skip
			int blockIdx = (int)Math.Truncate(blockOfColor);// Idx of 
			double valPercResidual = valPerc - (blockIdx * colorPerc);//remove the part represented of block 
			double percOfColor = valPercResidual / colorPerc;// % of color of this block that will be filled

			Color cTarget = _colors[blockIdx];
			Color cNext = _colors[blockIdx + 1];

			var deltaR = cNext.R - cTarget.R;
			var deltaG = cNext.G - cTarget.G;
			var deltaB = cNext.B - cTarget.B;

			var R = cTarget.R + (deltaR * percOfColor);
			var G = cTarget.G + (deltaG * percOfColor);
			var B = cTarget.B + (deltaB * percOfColor);

			Color c = _colors[0];
			try
			{
				c = Color.FromArgb(Alpha, (byte)R, (byte)G, (byte)B);
			}
			catch
			{
			}

			return c;
		}
	}
}
