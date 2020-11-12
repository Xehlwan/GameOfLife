using System;
using System.Diagnostics;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading;

namespace GameOfLife
{
    class Program
    {
        static void Main(string[] args)
        {
            (int width, int height, int maxFps) = MainMenu();
            Stopwatch sw = new Stopwatch();
            long frameWait = Stopwatch.Frequency / maxFps;

            var game = new Game(width, height);
            game.RandomSeed();
            var running = true;
            var wrap = true;

            while (running)
            {
                sw.Restart();
                if (wrap)
                    game.TickWrap();
                else
                    game.Tick();

                game.Render();
                do
                {
                    if (Console.KeyAvailable)
                    {
                        ConsoleKey key = Console.ReadKey(true).Key;
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

                            case ConsoleKey.Tab:
                                NextColor();
                                break;
                        }
                    }
                } while (sw.ElapsedTicks < frameWait);
            }
        }

        private static void NextColor()
        {
            int colorIndex = Console.ForegroundColor switch
            {
                ConsoleColor.Blue => 1,
                ConsoleColor.Cyan => 2,
                ConsoleColor.Green => 3,
                ConsoleColor.Yellow => 4,
                ConsoleColor.Red => 5,
                ConsoleColor.Magenta => 6,
                _ => 0
            };

            colorIndex = colorIndex < 6 ? colorIndex + 1 : 1;

            Console.ForegroundColor = colorIndex switch
            {
                1 => ConsoleColor.Blue,
                2 => ConsoleColor.Cyan,
                3 => ConsoleColor.Green,
                4 => ConsoleColor.Yellow,
                5 => ConsoleColor.Red,
                6 => ConsoleColor.Magenta
            };
        }

        private static (int width, int height, int timer) MainMenu()
        {
            int width, height, fps;
            Console.WriteLine("Welcome to Conway's Game of Life\n\n");
            Console.Write("CharColumns: ");
            while (!int.TryParse(Console.ReadLine(), out width))
            {
                Console.CursorLeft = 0;
                Console.CursorTop -= 1;
                Console.WriteLine("                            ");
                Console.CursorTop -= 1;
                Console.Write("CharColumns: ");
            }
            Console.Write("PixelHeight: ");
            while (!int.TryParse(Console.ReadLine(), out height))
            {
                Console.CursorLeft = 0;
                Console.CursorTop -= 1;
                Console.WriteLine("                            ");
                Console.CursorTop -= 1;
                Console.Write("PixelHeight: ");
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

            Console.WriteLine("Controls:");
            Console.WriteLine("ENTER => Reseed, SPACE => Toggle wrapping, ESC => Exit \n");
            Console.WriteLine("Press any key to start!");
            Console.ReadKey(true);
            Console.Clear();

            return (width, height, fps);
        }
    }
}
