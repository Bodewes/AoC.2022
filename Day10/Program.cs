using System;
using System.Diagnostics;
using System.IO;

namespace Day10
{

    class Program
    {
        static void Main(string[] args)
        {
            //var input = File.ReadAllLines("test.txt");
            //var input = File.ReadAllLines("test2.txt");
            var input = File.ReadAllLines("input.txt");

            var solver = new Solution(input);

            Console.WriteLine("Day 10" + Environment.NewLine);
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
            int X = 1;

            string[] paddedInput = psdInput();

            var signalStrength = 0;

            var cycleCount = 0;
            for (int i = 0; i < paddedInput.Length; i++)
            {
                cycleCount++;

                if ((cycleCount + 20) % 40 == 0)
                {
                    signalStrength += cycleCount * X;
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine($"{cycleCount}: {X}  (ss: {cycleCount * X})");
                    Console.ResetColor();
                }
                else
                {
                    //Console.WriteLine($"{cycleCount}: {X}  (ss: {cycleCount * X})");
                }

                if (paddedInput[i] != "noop")
                {
                    var dx = int.Parse(paddedInput[i].Substring(5));
                    X += dx;
                }
            }

            return signalStrength;
        }

        public int Part2()
        {
            int X = 1; //sprite pos

            string[] paddedInput = psdInput();

            var cycleCount = 0;
            for (int i = 0; i < paddedInput.Length; i++)
            {
                cycleCount++;

                var pos = (cycleCount - 1) % 40;

                if (pos == X -1 || pos == X || pos == X + 1)
                {
                    Console.Write('#');
                }
                else
                {
                    Console.Write('.');
                }

                if (cycleCount % 40 == 0)
                {
                    Console.WriteLine();
                }

                if (paddedInput[i] != "noop")
                {
                    var dx = int.Parse(paddedInput[i].Substring(5));
                    X += dx;
                }
            }

            return 0;
        }


        private string[] psdInput()
        {
            // prepend every Add with extra noop
            var oneLine = String.Join(",", input);
            var padded = oneLine.Replace("addx", "noop,addx") + ",noop,noop,noop";
            var paddedInput = padded.Split(",");
            return paddedInput;
        }


    }

}
