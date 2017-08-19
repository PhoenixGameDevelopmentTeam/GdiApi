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

            Console.Read();
        }
    }

    public class Game : Context
    {
        public Game() : base(new Size(500, 500))
        {

        }
    }
}