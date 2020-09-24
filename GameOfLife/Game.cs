using System;
using System.Collections.Generic;
using System.Text;

namespace GameOfLife
{
    class Game
    {
        public int Width { get; }
        public int Height { get; }
        public int RealHeight { get; }
        private bool[,] CurrentGen;
        private bool[,] nextGen;
        private char[] screen;

        public Game(int width, int height)
        {
            this.Width = width;
            Height = height;
            RealHeight = Height / 2;
            CurrentGen = new bool[Width, Height];
            nextGen = new bool[Width, Height];
            screen = new char[Width * RealHeight];

            Console.WindowHeight = RealHeight < Console.BufferHeight ? RealHeight : Console.BufferHeight;
            Console.WindowWidth = Width < Console.BufferWidth ? Width : Console.BufferWidth;
            Console.SetBufferSize(Width, RealHeight);
            Console.SetWindowSize(Width, RealHeight);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.OutputEncoding = Encoding.Unicode;
            Console.InputEncoding = Encoding.Unicode;
            Console.CursorVisible = false;
        }

        public void RandomSeed()
        {
            var rng = new Random();
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    CurrentGen[x, y] = rng.Next(0, 2) == 1;
                }
            }
        }

        public void Tick()
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    int neighbors = 0;
                    for (int nx = x - 1; nx <= x + 1; nx++)
                    {
                        if (nx < 0 || nx >= Width) continue;
                        for (int ny = y - 1; ny <= y + 1; ny++)
                        {
                            if (ny < 0 || ny >= Height || (nx == x && ny == y)) continue;
                            if (CurrentGen[nx, ny]) neighbors++;
                        }
                    }

                    if (CurrentGen[x, y])
                    {
                        if (neighbors < 2) nextGen[x, y] = false;
                        else if (neighbors > 3)
                            nextGen[x, y] = false;
                        else
                            nextGen[x, y] = true;
                    }
                    else if (neighbors == 3)
                        nextGen[x, y] = true;
                    else
                        nextGen[x, y] = false;
                }
            }

            var temp = CurrentGen;
            CurrentGen = nextGen;
            nextGen = temp;
        }

        public void TickWrap()
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    int neighbors = 0;
                    for (int nx = x - 1; nx <= x + 1; nx++)
                    {
                        int tx;
                        if (nx < 0) 
                            tx = Width - 1;
                        else if (nx >= Width)
                            tx = 0;
                        else
                            tx = nx;
                        for (int ny = y - 1; ny <= y + 1; ny++)
                        {
                            if (nx == x && ny == y) continue;
                            int ty;
                            if (ny < 0) 
                                ty = Height - 1;
                            else if (ny >= Height)
                                ty = 0;
                            else
                                ty = ny;
                            if (CurrentGen[tx, ty]) neighbors++;
                        }
                    }

                    if (CurrentGen[x, y])
                    {
                        if (neighbors < 2) nextGen[x, y] = false;
                        else if (neighbors > 3)
                            nextGen[x, y] = false;
                        else
                            nextGen[x, y] = true;
                    }
                    else if (neighbors == 3)
                        nextGen[x, y] = true;
                    else
                        nextGen[x, y] = false;
                }
            }

            var temp = CurrentGen;
            CurrentGen = nextGen;
            nextGen = temp;
        }


        public void Render()
        {
            Console.SetCursorPosition(0, 0);
            //Console.Clear();

            for (int ry = 0, gy = 0; ry < RealHeight; ry++, gy += 2)
            {
                for (int x = 0; x < Width; x++)
                {
                    var top = CurrentGen[x, gy];
                    var bottom = CurrentGen[x, gy + 1];
                    if (top && bottom) screen[x + ry * Width] = '\u2588';
                    else if (top && !bottom) screen[x + ry * Width] = '\u2580';
                    else if (!top && bottom)
                        screen[x + ry * Width] = '\u2584';
                    else
                        screen[x + ry * Width] = '\uFEFF';
                }
            }

            Console.Write(screen);
        }
    }
}
