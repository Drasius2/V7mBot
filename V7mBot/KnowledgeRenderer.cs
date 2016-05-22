using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using V7mBot.AI;

namespace V7mBot
{
    static public class KnowledgeRenderer
    {
        static Dictionary<int, Color> MINE_COLORS = new Dictionary<int, Color>()
        {
            {-1, Color.Black },
            {1, Color.Crimson },
            {2, Color.Blue },
            {3, Color.Green },
            {4, Color.Goldenrod }
        };

        static Dictionary<int, Color> HERO_COLORS = new Dictionary<int, Color>()
        {
            {1, Color.Tomato },
            {2, Color.DodgerBlue },
            {3, Color.LawnGreen },
            {4, Color.Gold }
        };

        public static Color DistanceGradient(float distance, float maxDistance)
        {
            float x = Math.Min(1, distance / maxDistance);
            byte red = (byte)Math.Min(255, x * 512);
            byte green = (byte)Math.Min(255, (1-x) * 512);
            return Color.FromArgb(red, green, 0);
        }

        public static Bitmap RenderMap(TileMap map, int upscale = 1)
        {
            Bitmap result = new Bitmap(map.Width, map.Height);
            Render(result, (x, y) =>
            {
                AI.TileMap.Tile tile = map[x, y];
                switch(tile.Type)
                {
                    case TileMap.TileType.Free:
                        return Color.White;
                    case TileMap.TileType.Impassable:
                        return Color.Gray;
                    case TileMap.TileType.Tavern:
                        return Color.FromArgb(255, 100, 255);
                    case TileMap.TileType.Hero:
                        return HERO_COLORS[tile.Owner];
                    case TileMap.TileType.GoldMine:
                        return MINE_COLORS[tile.Owner];
                    default:
                        return Color.Black;
                }
            });
            return Upscale(result, upscale);
        }

        public static Bitmap RenderHeroDistance(NavGrid navgrid, int upscale = 1)
        {
            Bitmap result = new Bitmap(navgrid.Width, navgrid.Height);
            float maxPathCost = navgrid.MaxPathCost;
            Render(result, (x, y) =>
            {
                NavGrid.Node node = navgrid[x, y];
                return (node.Previous == -1) ? Color.Black : DistanceGradient(node.PathCost, maxPathCost);
            });
            return Upscale(result, upscale);
        }


        private static Bitmap Upscale(Bitmap source, int upscale)
        {
            Bitmap upscaled = new Bitmap(source.Width * upscale, source.Height * upscale);
            using (Graphics gfx = Graphics.FromImage(upscaled))
            {
                gfx.InterpolationMode = InterpolationMode.NearestNeighbor;
                gfx.PixelOffsetMode = PixelOffsetMode.Half;
                gfx.DrawImage(source, 0, 0, upscaled.Width, upscaled.Height);
            }
            return upscaled;
        }

        delegate Color PixelQuery(int x, int y);
        static void Render(Bitmap bmp, PixelQuery query)
        {
            //lock bitmapData & copy it to buffer
            Rectangle area = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData data = bmp.LockBits(area, ImageLockMode.ReadWrite, bmp.PixelFormat);
            int bpp = Bitmap.GetPixelFormatSize(bmp.PixelFormat) / 8;
            int size = data.Stride * data.Height;
            byte[] buffer = new byte[size];
            IntPtr ptr = data.Scan0;
            Marshal.Copy(ptr, buffer, 0, size);
            //query a color for each pixel and write it into the buffer
            for (int x = 0; x < bmp.Width; x++)
                for (int y = 0; y < bmp.Height; y++)
                {
                    Color c = query(x, y);
                    int i = y * data.Stride + x * bpp;
                    buffer[i] = c.B;
                    buffer[i+1] = c.G;
                    buffer[i+2] = c.R;
                    buffer[i + 3] = c.A;
                }
            //copy buffer back to bitmapData & unlock
            Marshal.Copy(buffer, 0, ptr, size);
            bmp.UnlockBits(data);
        }
    }
}
