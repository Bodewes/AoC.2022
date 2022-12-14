using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Day14
{
    class Program
    {
        static void Main(string[] args)
        {
            //var input = File.ReadAllLines("test.txt");
            var input = File.ReadAllLines("input.txt");

            var solver = new Solution(input);

            Console.WriteLine("Day 14" + Environment.NewLine);
            TimeAction<int>(solver.Part1);
            TimeAction<int>(solver.Part2);

            Console.ReadKey();
        }

        private static void TimeAction<T>(Func<T> action)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            var result = action();

            Console.WriteLine($"Answer = {result}");

            sw.Stop();
            Console.WriteLine($"Took: {sw.Elapsed}" + Environment.NewLine);
        }
    }

    public class Solution
    {
        private readonly string[] input;
        private char[,] grid;
        private int xo; // x-offset

        public Solution(string[] input)
        {
            this.input = input;
        }

        public int Part1()
        {
            List<List<Point>> rockLines = ParseInput();

            int xsize, ysize;
            InitGrid(rockLines, out xsize, out ysize);

            PrintGrind(xsize, ysize);

            var grains = 0;
            while (DropSand(xsize, ysize))
            {
                grains++;
                //PrintGrind(xsize, ysize);
            }

            PrintGrind(xsize, ysize);

            return grains;
        }

        public int Part2()
        {
            List<List<Point>> rockLines = ParseInput();

            int xsize, ysize;
            InitGrid(rockLines, out xsize, out ysize, addFloor: true);

            PrintGrind(xsize, ysize);

            var grains = 0;
            while (DropSand(xsize, ysize))
            {
                grains++;
                //PrintGrind(xsize, ysize);
            }

            PrintGrind(xsize, ysize);

            return grains + 1;
        }


        private void InitGrid(List<List<Point>> rockLines, out int xsize, out int ysize, bool addFloor = false)
        {
            var minx = rockLines.SelectMany(r => r.Select(p => p.x)).Min();
            var maxx = rockLines.SelectMany(r => r.Select(p => p.x)).Max();
            var miny = 0; //rockLines.SelectMany(r => r.Select(p => p.y)).Min();  Start is a 500,0
            var maxy = rockLines.SelectMany(r => r.Select(p => p.y)).Max();

            Console.WriteLine("TopLeft: " + minx + "," + miny);
            Console.WriteLine("BottomR: " + maxx + "," + maxy);

            // if addFloor
            if (addFloor)
            {
                rockLines.Add(new List<Point>() { new Point(minx - maxy - 2, maxy + 2), new Point(minx + 2 * maxy + 2, maxy + 2) });

                minx = rockLines.SelectMany(r => r.Select(p => p.x)).Min();
                maxx = rockLines.SelectMany(r => r.Select(p => p.x)).Max();
                miny = 0; //rockLines.SelectMany(r => r.Select(p => p.y)).Min();  Start is a 500,0
                maxy = rockLines.SelectMany(r => r.Select(p => p.y)).Max();
            }

            // make grid 2 wider then minx//maxx and 2 higher then maxy
            xsize = maxx - minx + 3;
            ysize = maxy + 3;
            xo = -minx + 1; // xoffset

            grid = new char[xsize, ysize];
            for (int i = 0; i < xsize; i++)
            {
                for (int j = 0; j < ysize; j++)
                {
                    grid[i, j] = '.';
                }
            }

            // Start
            grid[500 + xo, 0] = '+';

            // Draw lines
            foreach (var rockline in rockLines)
            {
                for (int i = 0; i < rockline.Count - 1; i++)
                {
                    DrawLine(rockline[i], rockline[i + 1]);
                }
            }
        }

        private List<List<Point>> ParseInput()
        {
            var rockLines = new List<List<Point>>();
            foreach (var line in input)
            {
                var rockLine = new List<Point>();
                var ps = line.Split(" -> ", StringSplitOptions.RemoveEmptyEntries);
                foreach (var p in ps)
                {
                    var tokens = p.Split(",");
                    rockLine.Add(new Point(int.Parse(tokens[0]), int.Parse(tokens[1])));
                }
                rockLines.Add(rockLine);
            }

            return rockLines;
        }

        private bool DropSand(int xsize, int ysize)
        {
            // one sand.
            var moved = true;
            var sand = new Point(500, 0);
            do
            {
                if (grid[sand.x + xo, sand.y + 1] == '.')
                {
                    sand.y += 1;
                }
                else if (grid[sand.x - 1 + xo, sand.y + 1] == '.')
                {
                    sand.x -= 1;
                    sand.y += 1;
                }
                else if (grid[sand.x + 1 + xo, sand.y + 1] == '.')
                {
                    sand.x += 1;
                    sand.y += 1;
                }
                else
                {
                    grid[sand.x + xo, sand.y] = '0';
                    moved = false;
                }

                if (sand.y == ysize - 1)
                {
                    return false; // into the void
                };

                if (!moved && sand.x == 500 && sand.y == 0)
                {
                    return false; //stopped at start.
                }

            } while (moved);

            return true;
        }

        public void DrawLine(Point p1, Point p2)
        {
            var dx = 0;
            var dy = 0;
            if (p1.y == p2.y) // vertical
            {
                if (p1.x < p2.x)
                {
                    dx = 1;
                }
                else
                {
                    dx = -1;
                }
            }
            else // horizontal
            {
                if (p1.y < p2.y)
                {
                    dy = 1;
                }
                else
                {
                    dy = -1;
                }
            }
            var length = Math.Abs(p1.x - p2.x) + Math.Abs(p1.y - p2.y);
            for (int i = 0; i <= length; i++)
            {
                //Console.WriteLine($"{p1.x + xo + i * dx},{p1.y + i * dy}");
                grid[p1.x + xo + i * dx, p1.y + i * dy] = '#';
            }
        }

        public void PrintGrind(int w, int h)
        {
            //Console.SetCursorPosition(0, 0);
            for (int i = 0; i < h; i++)
            {
                for (int j = 0; j < w; j++)
                {
                    Console.Write(grid[j, i]);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }


    }

    [DebuggerDisplay("{x},{y}")]
    public struct Point
    {
        public Point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public int x;
        public int y;
    }
}
