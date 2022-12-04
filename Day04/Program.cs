using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Day04
{

    class Program
    {
        static void Main(string[] args)
        {
            //var input = File.ReadAllLines("test.txt");
            var input = File.ReadAllLines("input.txt");
            var solver = new Solution(input);

            Console.WriteLine("Day 04");
            TimeAction<int>(solver.Part1);
            TimeAction<int>(solver.Part2);

            Console.ReadKey();
        }

        private static void TimeAction<T>(Func<T> action)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            Console.WriteLine(action());

            sw.Stop();
            Console.WriteLine($"Took: {sw.Elapsed}");
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
            return input.Where(x => OverlapsComplete(Split(x))).Count();
        }

        public int Part2()
        {
            return input.Where(x => Overlaps(Split(x))).Count();
        }

        private bool OverlapsComplete((int a, int b, int c, int d) ranges)
        {
            var (a1, a2, b1, b2) = ranges;
            if (a1 <= b1 && b2 <= a2)
            {
                return true;
            }
            else if (b1 <= a1 && a2 <= b2)
            {
                return true;
            }
            return false;
        }

        private bool Overlaps((int a, int b, int c, int d) ranges)
        {
            var (a1, a2, b1, b2) = ranges;
            if (a1 <= b1 && b2 <= a2)  // b binnen a
            {
                return true;
            }
            else if (b1 <= a1 && a2 <= b2) // a binnen b
            {
                return true;
            }
            else if (b1 <= a1 && a1 <= b2)  // begin a binnen b
            {
                return true;
            }
            else if (b1 <= a2 && a2 <= b2)  // eind a binnen b
            {
                return true;
            }
            return false;
        }

        private (int,int,int,int) Split(string x)
        {
            var tokens = x.Split(',');
            var a = tokens[0].Split('-');
            var b = tokens[1].Split('-');
            return (int.Parse(a[0]), int.Parse(a[1]), int.Parse(b[0]), int.Parse(b[1]));
        }


    }

}
