using System;

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

    public class Game : Context
    {
        public Game() : base(new Size(1000, 1000))
        {

        }
    }
}