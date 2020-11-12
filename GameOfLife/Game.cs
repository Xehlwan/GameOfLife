using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading;

namespace GameOfLife
{
    class Game
    {
        private ConsoleColor alive = ConsoleColor.Yellow;
        private readonly ConsoleColor dead = ConsoleColor.Black;
        public int CharColumns { get; }
        public int PixelHeight { get; }
        public int CharRows { get; }
        public int PixelWidth => CharColumns;

        private bool[,] currentGen;
        private bool[,] nextGen;

        private readonly Random rng = new();
        private readonly StringBuilder sb = new();

        public Game(int width, int height)
        {
            // Set the game to valid dimensions.
            if (height % 2 != 0) height -= 1;
            CharColumns = width <= Console.LargestWindowWidth ? width : Console.LargestWindowWidth;
            CharRows = height / 2 <= Console.LargestWindowHeight ? height / 2 : Console.LargestWindowHeight;
            PixelHeight = CharRows * 2;

            // Initialize arrays.
            currentGen = new bool[PixelWidth, PixelHeight];
            nextGen = new bool[PixelWidth, PixelHeight];

            // Prepare Terminal
            Console.WindowHeight = CharRows <= Console.BufferHeight ? CharRows : Console.BufferHeight;
            Console.WindowWidth = CharColumns <= Console.BufferWidth ? CharColumns : Console.BufferWidth;
            Console.SetBufferSize(CharColumns, CharRows);
            Console.SetWindowSize(CharColumns, CharRows);

            Console.CursorVisible = false;

            Console.ForegroundColor = alive;
            Console.BackgroundColor = dead;
        }

        public void RandomSeed()
        {
            for (int x = 0; x < PixelWidth; x++)
            {
                for (int y = 0; y < PixelHeight; y++)
                {
                    currentGen[x, y] = rng.Next(0, 2) == 1;
                }
            }
        }

        public void Tick()
        {
            for (int x = 0; x < CharColumns; x++)
            {
                for (int y = 0; y < PixelHeight; y++)
                {
                    int neighbors = 0;
                    for (int nx = x - 1; nx <= x + 1; nx++)
                    {
                        if (nx < 0 || nx >= CharColumns) continue;
                        for (int ny = y - 1; ny <= y + 1; ny++)
                        {
                            if (ny < 0 || ny >= PixelHeight || (nx == x && ny == y)) continue;
                            if (currentGen[nx, ny]) neighbors++;
                        }
                    }

                    if (currentGen[x, y])
                    {
                        if (neighbors < 2 || neighbors > 3) 
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

            (currentGen, nextGen) = (nextGen, currentGen);
        }

        public void TickWrap()
        {
            for (int x = 0; x < CharColumns; x++)
            {
                for (int y = 0; y < PixelHeight; y++)
                {
                    int neighbors = 0;
                    for (int nx = x - 1; nx <= x + 1; nx++)
                    {
                        int tx;
                        if (nx < 0) 
                            tx = CharColumns - 1;
                        else if (nx >= CharColumns)
                            tx = 0;
                        else
                            tx = nx;
                        for (int ny = y - 1; ny <= y + 1; ny++)
                        {
                            if (nx == x && ny == y) continue;
                            int ty;
                            if (ny < 0) 
                                ty = PixelHeight - 1;
                            else if (ny >= PixelHeight)
                                ty = 0;
                            else
                                ty = ny;
                            if (currentGen[tx, ty]) neighbors++;
                        }
                    }

                    if (currentGen[x, y])
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

            (currentGen, nextGen) = (nextGen, currentGen);
        }

        public void SetColor(ConsoleColor color)
        {
            alive = color;
            Console.ForegroundColor = alive;
        }

        public void Render()
        {
            PrepareCharBuffer();
            DrawScreen();
        }

        private void DrawScreen()
        {
            Console.SetCursorPosition(0,0);
            Console.Write(sb.ToString());
        }

        private void PrepareCharBuffer()
        {
            sb.Clear();
            for (int y = 0; y < CharRows; y++)
            {
                for (int x = 0; x < CharColumns; x++)
                {
                    (bool Top, bool Bottom) status = (currentGen[x, y * 2], currentGen[x, y * 2 + 1]);
                    char c = status switch
                    {
                        (true, true) => '\u2588',
                        (true, false) => '\u2580',
                        (false, true) => '\u2584',
                        (false, false) => '\u0020'
                    };

                    sb.Append(c);
                }
            }
        }
    }
}
