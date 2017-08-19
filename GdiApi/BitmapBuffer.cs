using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace GdiApi
{
    public static class BitmapBuffer
    {
        private static List<Bitmap> memory;
        public static bool Ready => memory != null;

        public static void Clear()
        {
            memory = new List<Bitmap>();
            memory.Clear();
        }

        public static void Load(string loadPath)
        {
            if (Directory.Exists(loadPath))
            {
                foreach (var filePath in Directory.GetFiles(loadPath))
                {
                    if (filePath.EndsWith(".png"))
                    {
                        memory.Add(new Bitmap(filePath));
                    }
                }
            }
        }

        public static Bitmap FromIndex(int index) => memory[index];
        public static void DrawBitmap(this Graphics g, int index, Rectangle r)
        {
            if (BitmapBuffer.Ready)
            {
                g.DrawImage(FromIndex(index), r);
            }
        }
    }
}