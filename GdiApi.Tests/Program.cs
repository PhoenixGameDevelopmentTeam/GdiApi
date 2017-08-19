using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace GdiApi.Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            var g = new Game();
        }
    }

    public class Game
    {
        public int frameCntThisSecond = 0;
        public int frameRate = 0;
        public TimeSpan second = new TimeSpan();

        Context context;

        public Game()
        {
            context = new Context(new Size(1000, 1000), "Fun Time!", true);
            context.Load += Load;
            context.Render += Render;
            context.Begin();
        }

        public void Load()
        {
            BitmapBuffer.Clear();
            BitmapBuffer.Load(@"assets\");
        }

        public void Render(Graphics graphics, TimeSpan delta)
        {
            second += delta;
            if (second.TotalMilliseconds >= 1000)
            {
                frameRate = (frameRate + frameCntThisSecond) / 2;
                frameCntThisSecond = 0;
                second = new TimeSpan();
            }

            if (BitmapBuffer.Ready)
            {
                graphics.DrawBitmap(0, new Rectangle(0, 0, 1000, 1000));
            }

            frameCntThisSecond++;

            context.Title = frameRate.ToString() + " | Ready: " + BitmapBuffer.Ready;
        }
    }
}