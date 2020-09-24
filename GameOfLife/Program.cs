using System;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading;

namespace GameOfLife
{
    class Program
    {
        static void Main(string[] args)
        {
            int width, height, fps;
            Console.WriteLine("Welcome to Conway's Game of Life\n\n");
            Console.Write("Width: ");
            while (!int.TryParse(Console.ReadLine(), out width))
            {
                Console.CursorLeft = 0;
                Console.CursorTop -= 1;
                Console.WriteLine("                            ");
                Console.CursorTop -= 1;
                Console.Write("Width: ");
            }
            Console.Write("Height: ");
            while (!int.TryParse(Console.ReadLine(), out height))
            {
                Console.CursorLeft = 0;
                Console.CursorTop -= 1;
                Console.WriteLine("                            ");
                Console.CursorTop -= 1;
                Console.Write("Height: ");
            }
            Console.Write("Max fps: ");
            while (!int.TryParse(Console.ReadLine(), out fps))
            {
                Console.CursorLeft = 0;
                Console.CursorTop -= 1;
                Console.WriteLine("                            ");
                Console.CursorTop -= 1;
                Console.Write("max fps: ");
            }

            int timer = 1000 / fps > 1 ? 1000 / fps : 1;
            Console.WriteLine("Controls:");
            Console.WriteLine("ENTER => Reseed, SPACE => Toggle wrapping, ESC => Exit \n");
            Console.WriteLine("Press any key to start!");
            Console.ReadKey(true);
            Console.Clear();

            var game = new Game(width, height);
            game.RandomSeed();
            bool running = true;
            bool wrap = true;

            while (running)
            {
                if (wrap)
                    game.TickWrap();
                else
                    game.Tick();

                game.Render();
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true).Key;
                    switch (key)
                    {
                        case ConsoleKey.Escape:
                            running = false;
                            break;
                        case ConsoleKey.Enter:
                            game.RandomSeed();
                            break;
                        case ConsoleKey.Spacebar:
                            wrap = !wrap;
                            break;
                    }
                }
                Thread.Sleep(timer);
            }
        }
    }
}
