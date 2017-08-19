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
        Context context;
        FrameRateManager frameRate = new FrameRateManager();

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
            frameRate.Frame(delta);

            if (BitmapBuffer.Ready)
            {
                graphics.DrawBitmap(0, new Rectangle(0, 0, 1000, 1000));
            }

            context.Title = "Framerate: " + frameRate.ToString();
        }
    }
}