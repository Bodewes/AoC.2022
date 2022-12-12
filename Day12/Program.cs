using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Day12
{
    class Program
    {
        static void Main(string[] args)
        {
            //var input = File.ReadAllLines("test.txt");
            var input = File.ReadAllLines("input.txt");

            var solver = new Solution(input);

            Console.WriteLine("Day 12" + Environment.NewLine);
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

        char[,] grid;
        int w;
        int h;
        int sx, sy, ex, ey;

        public Solution(string[] input)
        {
            this.input = input;
            ParseInput();
        }

        public int Part1()
        {
            Console.WriteLine($"From S({sx},{sy}) to E({ex},{ey})");

            // Init distances
            int[,] dist = new int[h, w];
            for (int i = 0; i < h; i++)
            {
                for (int j = 0; j < w; j++)
                {
                    dist[i, j] = int.MaxValue;
                }
            }

            // Set current to starting position
            (int x, int y) current = (sx, sy);
            var q = new List<(int x, int y)>();

            dist[current.y, current.x] = 0; ;

            // While not a finish...
            while (!(current.x == ex && current.y == ey))
            {
                // min step to get to current location
                var step = dist[current.y, current.x];
                //Console.WriteLine($"At {current.x}, {current.y} - dist {step}");
                var neighbours = Neighbours(current);
                foreach((int x, int y) n in neighbours)
                {
                    if (ValidStep(current, n))
                    {
                        if (dist[n.y, n.x] > step + 1)
                        {
                            //Console.WriteLine($"Found shorter path ({step + 1})to {n.x},{n.y} ");
                            dist[n.y, n.x] = step + 1;
                            q.Add(n); // Queue for inspection.
                        }
                        else
                        {
                            // not shorter, do not add to Queue
                        }
                    }
                    else
                    {
                        // not valid step, do not add to Queue
                    }
                }

                // Pick lowest.
                q = q.OrderBy(q => dist[q.y, q.x]).ToList();

                //Console.WriteLine("Q length:" + q.Count);
                if (!q.Any())
                    break;

                // pop from queue
                current = q.First();
                q.RemoveAt(0);

                //Console.WriteLine($" new current: {current.x},{current.y}");
            }


            return dist[ey, ex];
        }

        public int Part2()
        {
            var minA = int.MaxValue;
            for(int i =0; i < w; i++)
            {
                for(int j =0; j < h; j++)
                {
                    if (grid[j, i] == 'a')
                    {
                        sx = i; sy = j;
                        var dist = Part1();
                        minA = Math.Min(minA, dist);
                    }
                }
            }

            return minA;
        }

        /// <summary>
        /// 3 -> 2 OK
        /// 3 -> 3 OK
        /// 3 -> 4 OK
        /// 3 -> 5 NOK
        /// 3 -> 6 NOK
        /// 3 -> 7 NOK
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        private bool ValidStep((int x, int y) from, (int x, int y) to)
        {
            return (grid[from.y, from.x] + 1 >= grid[to.y, to.x]);
        }

        // Get all 4-connected
        private List<(int, int)> Neighbours((int x, int y) p)
        {
            var n = new List<(int, int)>();

            if (p.x - 1 >= 0)
            {
                n.Add((p.x - 1, p.y));
            }
            if (p.x + 1 < w)
            {
                n.Add((p.x + 1, p.y));
            }
            if (p.y - 1 >= 0)
            {
                n.Add((p.x, p.y - 1));
            }
            if (p.y + 1 < h)
            {
                n.Add((p.x, p.y + 1));
            }

            //Console.WriteLine(" N: " + string.Join("; ", n.Select(p => $"{p.Item1},{p.Item2}")));

            return n;
        }



        private void ParseInput()
        {
            h = input.Length;
            w = input[0].Length;
            grid = new char[h, w];
            for (int i = 0; i < h; i++)
            {
                var row = input[i].ToCharArray();
                for (int j = 0; j < w; j++)
                {
                    grid[i, j] = row[j];
                }

                if (input[i].IndexOf('S') >= 0)
                {
                    sx = input[i].IndexOf('S');
                    sy = i;
                }

                if (input[i].IndexOf('E') >= 0)
                {
                    ex = input[i].IndexOf('E');
                    ey = i;
                }
            }
            grid[sy, sx] = 'a';
            grid[ey, ex] = 'z';
        }
    }
}
