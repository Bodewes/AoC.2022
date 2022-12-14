using System;
using System.Diagnostics;
using System.IO;

namespace Day02
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllLines("input.txt");
            var solver = new Solution(input);

            Console.WriteLine("Day 02");
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
            int totalScore = 0;
            for ( int i = 0; i< input.Length; i++)
            {
                var tokens = input[i].Split(' ');
                var opponent = tokens[0] == "A" ? 'r': tokens[0] == "B" ? 'p' : 'c';
                var response = tokens[1] == "X" ? 'r' : tokens[1] == "Y" ? 'p' : 'c'; ;
                var score = response == 'r' ? 1 : response == 'p' ? 2 : 3;
                score += scoreGame(opponent, response);

                //Console.WriteLine(score);
                totalScore += score;
            }

            return totalScore;
        }

        private int scoreGame(char other, char my)
        {
            if (other == my)
            {
                //Console.WriteLine("darw");
                return 3;
            }
                
            if ((other == 'r' && my == 'p') ||
                (other == 'p' && my == 'c') ||
                (other == 'c' && my == 'r'))
            {
                //Console.WriteLine("won");
                return 6;
            }
                
            //Console.WriteLine("lost");
            return 0;
        }

        public int Part2()
        {
            int totalScore = 0;
            for (int i = 0; i < input.Length; i++)
            {
                var tokens = input[i].Split(' ');
                var opponent = tokens[0] == "A" ? 'r' : tokens[0] == "B" ? 'p' : 'c';
                var response = tokens[1][0];
                var score = scoreGamePart2(opponent, response);

                //Console.WriteLine(score);
                totalScore += score;
            }

            return totalScore;
        }

        private int scoreGamePart2(char other, char action)
        {
            if (action == 'X') // Loose
            {
                switch (other)
                {
                    case 'r': return 3;
                    case 'p': return 1;
                    case 'c': return 2;
                }
            } 
            else if (action == 'Y') // DRAW
            {
                switch (other)
                {
                    case 'r': return 1+3;
                    case 'p': return 2+3;
                    case 'c': return 3+3;
                }

            }
            else // WIN
            {
                switch (other)
                {
                    case 'r': return 2+6;
                    case 'p': return 3+6;
                    case 'c': return 1+6;
                }

            }

            return 0;
        }
    }
}
