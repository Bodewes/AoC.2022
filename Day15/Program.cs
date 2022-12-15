using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Day15
{
    class Program
    {
        static void Main(string[] args)
        {
            //var input = File.ReadAllLines("test.txt");
            //int yh = 10;

            var input = File.ReadAllLines("input.txt");
            int yh = 2000000;


            var solver = new Solution(input);

            Console.WriteLine("Day 15" + Environment.NewLine);
            TimeAction<int>(() => solver.Part1(yh));
            TimeAction<int>(() => solver.Part2(yh));

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

        public List<Point> Sensors = new List<Point>();
        public List<Point> Beacons = new List<Point>();

        public Solution(string[] input)
        {
            this.input = input;
            ParseInput();
        }

        public int Part1(int yh)
        {
            int offset;
            bool[] x = GetRowData(yh, clearBeacon: true, out offset);

            return x.Count(z => z);
        }

        public int Part2(int yh)
        {
            Parallel.For(0, yh * 2 + 1, (i) =>
            {

                //for (int i = 0; i <= yh*2; i++)
                //{
                int offset;
                bool[] x = GetRowData(i, clearBeacon: false, out offset);

                var occupied = x.Skip(-offset).Take(yh * 2).Count(z => z);
                if (i%1000 == 0)
                {
                    Console.WriteLine($"[{offset}] {i} => {occupied}");
                }


                if (occupied < yh * 2)
                {
                    for (int t = 0; t < x.Length; t++)
                    {
                        if (!x[t])
                        {
                            Console.WriteLine((t + offset) * 4_000_000 + i);
                            //return (t + offset) * 4_000_000 + i;
                            throw new Exception();
                        }
                    }
                }

                //}
            });

            return 0;
        }

        private bool[] GetRowData(int yh, bool clearBeacon, out int offset)
        {
            var ranges = new List<(int y1, int y2)>();

            var count = Sensors.Count;
            for (int i = 0; i < count; i++)
            {
                var s = Sensors[i];
                var b = Beacons[i];
                var d = dist(s, b);

                //Console.WriteLine($"{s.x},{s.y} -> {b.x},{b.y} -> {d}");

                // in range van s.y-d t/m s.y+d
                if (yh >= s.y - d && yh <= s.y + d)
                {
                    var w = d - Math.Abs(s.y - yh);
                    var low = s.x - w;
                    var high = s.x + w;
                    ranges.Add((low, high));
                }
            }

            // poormans combine
            var min = ranges.Select(r => r.y1).Min();
            var max = ranges.Select(r => r.y2).Max();
            var size = max - min;
            //Console.WriteLine($"{min}-{max} ({size})");
            offset = min;
            var x = new bool[size + 1];
            foreach (var r in ranges)
            {
                for (int i = r.y1; i <= r.y2; i++)
                {
                    x[i - offset] = true;
                }
            }
            // remove beacons.
            if (clearBeacon)
            {
                var beaconsOnYh = Beacons.Where(b => b.y == yh);
                foreach (var b in beaconsOnYh)
                {
                    x[b.x - offset] = false;
                }
            }

            return x;
        }

        private void ParseInput()
        {
            Sensors = new List<Point>();
            Beacons = new List<Point>();
            foreach(var line in input)
            {
                var tokens = line.Split(' ');
                var sensor = new Point(int.Parse(tokens[2].Substring(2).TrimEnd(',')), int.Parse(tokens[3].Substring(2).TrimEnd(':')));
                var beacon = new Point(int.Parse(tokens[8].Substring(2).TrimEnd(',')), int.Parse(tokens[9].Substring(2)));

                Sensors.Add(sensor);
                Beacons.Add(beacon);

                Console.WriteLine($"{sensor.x},{sensor.y} sees {beacon.x},{beacon.y} at distance {dist(sensor, beacon)}");
            }
        }

        private int dist(Point p1, Point p2)
        {
            return Math.Abs(p1.x - p2.x) + Math.Abs(p1.y - p2.y);
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
