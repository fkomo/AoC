using System.Text;
using Ujeby.AoC.Common;

namespace Ujeby.AoC.App.Year2021.Day13
{
	public class CharCodes
    {
        public static string ToString(int width, int height, bool[,] charMap)
        {
            var result = string.Empty;
            for (var p = 0; p < width; p += 5)
                foreach (var c in _map)
                {
                    var found = true;
                    for (var y = 0; y < c.Value.GetLength(0) && found; y++)
                        for (var x = 0; x < c.Value.GetLength(1) && found; x++)
                        {
                            if (charMap[y, p + x] && c.Value[y, x] == 0 ||
                                !charMap[y, p + x] && c.Value[y, x] == 1)
                                found = false;
                        }

                    if (found)
                    {
                        result += c.Key;
                        break;
                    }
                }

#if _DEBUG
            DrawString(width, height, charMap);
#endif
            return result;
        }

        public static void DrawString(int width, int height, bool[,] paper)
        {
			var line = new StringBuilder();

			Log.Line();
            for (var y = 0; y < height; y++)
            {
				line.Clear();
				for (var x = 0; x < width; x++)
					line.Append(paper[y, x] ? "█" : " ");

                Log.Line(line.ToString());
            }
            Log.Line();
        }

		/// <summary>
		/// A B C D E F G H i* J K L m* n* O P q* R s* t* U v* x* y* Z
		/// * missing
		/// </summary>
        public static Dictionary<char, int[,]> _map = new Dictionary<char, int[,]>
        {
			{ 'A', new int[,]
				{
					{ 0, 1, 1, 0 },
					{ 1, 0, 0, 1 },
					{ 1, 1, 1, 1 },
					{ 1, 0, 0, 1 },
					{ 1, 0, 0, 1 },
					{ 1, 0, 0, 1 }
				}
			},
			{ 'B', new int[,]
				{
					{ 1, 1, 1, 0 },
					{ 1, 0, 0, 1 },
					{ 1, 1, 1, 0 },
					{ 1, 0, 0, 1 },
					{ 1, 0, 0, 1 },
					{ 1, 1, 1, 0 }
				}
			},
			{ 'C', new int[,]
				{
					{ 0, 1, 1, 0 },
					{ 1, 0, 0, 1 },
					{ 1, 0, 0, 0 },
					{ 1, 0, 0, 0 },
					{ 1, 0, 0, 1 },
					{ 0, 1, 1, 0 }
				}
			},
			{ 'D', new int[,]
				{
					{ 1, 1, 1, 0 },
					{ 1, 0, 0, 1 },
					{ 1, 0, 0, 1 },
					{ 1, 0, 0, 1 },
					{ 1, 0, 0, 1 },
					{ 1, 1, 1, 0 }
				}
			},
			{ 'E', new int[,]
				{
					{ 1, 1, 1, 1 },
					{ 1, 0, 0, 0 },
					{ 1, 1, 1, 0 },
					{ 1, 0, 0, 0 },
					{ 1, 0, 0, 0 },
					{ 1, 1, 1, 1 }
				}
			},
            { 'F', new int[,]
				{
					{ 1, 1, 1, 1 },
					{ 1, 0, 0, 0 },
					{ 1, 1, 1, 0 },
					{ 1, 0, 0, 0 },
					{ 1, 0, 0, 0 },
					{ 1, 0, 0, 0 }
				}
			},
			{ 'G', new int[,]
                {
                    { 0, 1, 1, 0 },
                    { 1, 0, 0, 1 },
                    { 1, 0, 0, 0 },
                    { 1, 0, 1, 1 },
                    { 1, 0, 0, 1 },
                    { 0, 1, 1, 1 }
                }
            },
            { 'H', new int[,]
                {
                    { 1, 0, 0, 1 },
                    { 1, 0, 0, 1 },
                    { 1, 1, 1, 1 },
                    { 1, 0, 0, 1 },
                    { 1, 0, 0, 1 },
                    { 1, 0, 0, 1 }
                }
            },
			{ 'J', new int[,]
				{
					{ 0, 0, 1, 1 },
					{ 0, 0, 0, 1 },
					{ 0, 0, 0, 1 },
					{ 0, 0, 0, 1 },
					{ 1, 0, 0, 1 },
					{ 0, 1, 1, 0 }
				}
			},
            { 'K', new int[,]
                {
                    { 1, 0, 0, 1 },
                    { 1, 0, 1, 0 },
                    { 1, 1, 0, 0 },
                    { 1, 0, 1, 0 },
                    { 1, 0, 1, 0 },
                    { 1, 0, 0, 1 }
                }
            },
            { 'L', new int[,]
                {
                    { 1, 0, 0, 0 },
                    { 1, 0, 0, 0 },
                    { 1, 0, 0, 0 },
                    { 1, 0, 0, 0 },
                    { 1, 0, 0, 0 },
                    { 1, 1, 1, 1 }
                }
            },
			{ 'O', new int[,]
				{
					{ 0, 1, 1, 0 },
					{ 1, 0, 0, 1 },
					{ 1, 0, 0, 1 },
					{ 1, 0, 0, 1 },
					{ 1, 0, 0, 1 },
					{ 0, 1, 1, 0 }
				}
			},
			{ 'P', new int[,]
                {
                    { 1, 1, 1, 0 },
                    { 1, 0, 0, 1 },
                    { 1, 0, 0, 1 },
                    { 1, 1, 1, 0 },
                    { 1, 0, 0, 0 },
                    { 1, 0, 0, 0 }
                }
            },
            { 'R', new int[,]
                {
                    { 1, 1, 1, 0 },
                    { 1, 0, 0, 1 },
                    { 1, 0, 0, 1 },
                    { 1, 1, 1, 0 },
                    { 1, 0, 1, 0 },
                    { 1, 0, 0, 1 }
                }
            },
			{ 'U', new int[,]
				{
					{ 1, 0, 0, 1 },
					{ 1, 0, 0, 1 },
					{ 1, 0, 0, 1 },
					{ 1, 0, 0, 1 },
					{ 1, 0, 0, 1 },
					{ 0, 1, 1, 0 }
				}
			},          
            { 'Z', new int[,]
				{
					{ 1, 1, 1, 1 },
					{ 0, 0, 0, 1 },
					{ 0, 0, 1, 0 },
					{ 0, 1, 0, 0 },
					{ 1, 0, 0, 0 },
					{ 1, 1, 1, 1 }
				}
			},      
        };
    }
}
