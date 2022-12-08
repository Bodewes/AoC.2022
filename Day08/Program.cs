using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Day08
{
    class Program
    {
        static void Main(string[] args)
        {
            //var input = File.ReadAllLines("test.txt");
            var input = File.ReadAllLines("input.txt");

            var solver = new Solution(input);

            Console.WriteLine("Day 08" + Environment.NewLine);
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

        private int sizex;
        private int sizey;
        private int[][] grid;
        private bool[][] visible;

        public Solution(string[] input)
        {
            this.input = input;

            (sizex, sizey, grid, visible) = ParseInput();
        }

        public int Part1()
        {

            PrintGrid(grid);

            for (int y = 1; y < sizey - 1; y++)
            {
                // loop ltr
                int maxh = grid[y][0];
                for (int x = 1; x < sizex - 1; x++)
                {
                    visible[y][x] |= grid[y][x] > maxh;

                    maxh = Math.Max(grid[y][x], maxh);
                }

                // loop rtl
                maxh = grid[y][sizex - 1];
                for (int x = sizex - 2; x > 0; x--)
                {
                    visible[y][x] |= grid[y][x] > maxh;

                    maxh = Math.Max(grid[y][x], maxh);
                }
            }

            for (int x = 1; x < sizex - 1; x++)
            {
                // loop utd
                int maxh = grid[0][x];
                for (int y = 1; y < sizey - 1; y++)
                {
                    visible[y][x] |= grid[y][x] > maxh;

                    maxh = Math.Max(grid[y][x], maxh);
                }

                // loop dtu
                maxh = grid[sizey - 1][x];
                for (int y = sizey - 2; y > 0; y--)
                {
                    visible[y][x] |= grid[y][x] > maxh;

                    maxh = Math.Max(grid[y][x], maxh);
                }
            }

            PrintVisible(visible);

            var v = visible.SelectMany(x => x).Count(x => x);
            v += sizex + sizex + sizey + sizey - 4; // All interior + 4 edges - minus 4 corners that are double counted.

            return v;
        }

        public int Part2()
        {
            var max = 0;
            for (int y = 0; y < sizey; y++)
            {
                for (int x = 0; x < sizex; x++)
                {
                    max = Math.Max(ScenicScore(x, y), max);
                }
            }

            return max;
        }

        private int ScenicScore(int px, int py)
        {
            var h = grid[py][px];
            // to the left
            var l = 0;
            for (var x = px + 1; x < sizex; x++)
            {
                l++;
                if (grid[py][x] >= h)
                {
                    break;
                }
            }

            // to the right
            var r = 0;
            for (var x = px - 1; x >= 0; x--)
            {
                r++;
                if (grid[py][x] >= h)
                {
                    break;
                }
            }

            // down
            var d = 0;
            for (var y = py + 1; y < sizey; y++)
            {
                d++;
                if (grid[y][px] >= h)
                {
                    break;
                }
            }

            // up 
            var u = 0;
            for (var y = py - 1; y >= 0; y--)
            {
                u++;
                if (grid[y][px] >= h)
                {
                    break;
                }
            }

            return l * r * d * u;
        }

        private (int, int, int[][], bool[][]) ParseInput()
        {
            var sizex = input[0].Length;
            var sizey = input.Length;

            // Parse grid
            var grid = new int[sizey][];
            var visible = new bool[sizey][];
            for (int y = 0; y < sizey; y++)
            {
                grid[y] = input[y].Select(c => c - '0').ToArray();
                visible[y] = new bool[sizex];
            }

            return (sizex, sizey, grid, visible);
        }


        private void PrintVisible(bool[][] visible)
        {
            Print(visible, x => x ? 'T' : '_');
        }

        private void PrintGrid(int[][] grid)
        {
            Print(grid, x => (char)(x + '0'));
        }

        private void Print<T>(T[][] grid, Func<T, char> legend)
        {
            var sizey = grid.Length;
            var sizex = grid[0].Length;

            for (int y = 0; y < sizey; y++)
            {
                for (int x = 0; x < sizex; x++)
                {
                    Console.Write(legend(grid[y][x]));
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}
