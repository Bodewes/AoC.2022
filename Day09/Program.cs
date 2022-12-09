using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Day09
{
    class Program
    {
        static void Main(string[] args)
        {
            //var input = File.ReadAllLines("test2.txt");
            //var input = File.ReadAllLines("test2.txt");
            var input = File.ReadAllLines("input.txt");

            var solver = new Solution(input);

            Console.WriteLine("Day 09" + Environment.NewLine);
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

        public Solution(string[] input)
        {
            this.input = input;
        }

        public int Part1()
        {
            int hx = 0;
            int hy = 0;

            int tx = 0;
            int ty = 0;

            int dx = 0;
            int dy = 0;

            var tail_positions = new List<string>();
            tail_positions.Add($"{tx}_{ty}");

            //Print(hx, hy, tx, ty);
            foreach(var line in input) {
                //Console.WriteLine();
                //Console.WriteLine($"== {line} ==");

                var tokens = line.Split(" ");
                
                switch (tokens[0])
                {
                    case "R":
                        dx = 1;
                        dy = 0;
                        break;
                    case "L":
                        dx = -1;
                        dy = 0;
                        break;
                    case "U":
                        dx = 0;
                        dy = 1;
                        break;
                    case "D":
                        dx = 0;
                        dy = -1;
                        break;
                }

                var steps = int.Parse(tokens[1]);

                for(int s = 0; s< steps; s++)
                {
                    // move head
                    hx += dx;
                    hy += dy;

                    // move tail
                    if (Math.Abs(hx-tx) <= 1 && Math.Abs(hy-ty) <= 1)
                    {
                        // tail is close enough
                    }
                    else
                    {
                        // move closer
                        tx += Math.Clamp(hx - tx, -1, 1);
                        ty += Math.Clamp(hy - ty, -1, 1);
                    }

                    tail_positions.Add($"{tx}_{ty}");

                    //Print(hx, hy, tx, ty);
                }

            }

            return tail_positions.Distinct().Count();
        }

        public void Print(int hx, int hy, int tx, int ty)
        {
            Console.WriteLine();
            for(int h = 4; h >=0; h--)
            {
                for(int w = 0; w <= 6; w++)
                {
                    if (w == hx && h == hy)
                        Console.Write('H');
                    else if (w == tx && h == ty)
                        Console.Write('T');
                    else
                        Console.Write('.');

                }
                Console.WriteLine();
            }
        }


        public int Part2()
        {
            int[] tx = new int[10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            int[] ty = new int[10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            int dx = 0;
            int dy = 0;

            var tail_positions = new List<string>();
            tail_positions.Add($"{tx[9]}_{ty[9]}");

            //Print(hx, hy, tx, ty);
            foreach (var line in input)
            {
                //Console.WriteLine();
                //Console.WriteLine($"== {line} ==");

                var tokens = line.Split(" ");

                switch (tokens[0])
                {
                    case "R":
                        dx = 1;
                        dy = 0;
                        break;
                    case "L":
                        dx = -1;
                        dy = 0;
                        break;
                    case "U":
                        dx = 0;
                        dy = 1;
                        break;
                    case "D":
                        dx = 0;
                        dy = -1;
                        break;
                }

                var steps = int.Parse(tokens[1]);

                for (int s = 0; s < steps; s++)
                {
                    // move head
                    tx[0] += dx;
                    ty[0] += dy;

                    // move tail
                    for (int i = 1; i< 10; i++)
                    {
                        if (Math.Abs(tx[i-1] - tx[i]) <= 1 && Math.Abs(ty[i-1] - ty[i]) <= 1)
                        {
                            // tail is close enough
                        }
                        else
                        {
                            // move closer
                            tx[i] += Math.Clamp(tx[i-1] - tx[i], -1, 1);
                            ty[i] += Math.Clamp(ty[i-1] - ty[i], -1, 1);
                        }
                    }

                    tail_positions.Add($"{tx[9]}_{ty[9]}");

                    //Print(hx, hy, tx, ty);
                }

            }

            return tail_positions.Distinct().Count();
        }
    }

}
