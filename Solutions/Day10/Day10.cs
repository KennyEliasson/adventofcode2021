using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Solutions
{
    public class Day10
    {
        private readonly ITestOutputHelper _output;

        public Day10(ITestOutputHelper output)
        {
            _output = output;
        }
       
        [Fact]
        public void Day10_Part1()
        {
            var syntaxLines = File.ReadLines("Day10/input.txt").ToList();

            HashSet<char> openingChars = new(){'{', '[', '(', '<'};
            HashSet<char> closeningChars = new(){'}', ']', ')', '>'};
            
            var illegalChars = new List<char>();
            foreach (var line in syntaxLines)
            {
                var chunkOpenings = new Stack<char>();
                foreach (var c in line)
                {
                    if (openingChars.Contains(c))
                    {
                        chunkOpenings.Push(c);
                        continue;
                    }

                    if (closeningChars.Contains(c))
                    {
                        var pop = chunkOpenings.Pop();
                        if(pop == c-2 || pop == c-1)
                            continue;

                        illegalChars.Add(c);
                        break;
                    }

                    throw new Exception("WTF?");
                }
            }

            var result = 0;
            var points = new Dictionary<char, int>()
            {
                {')', 3},
                {']', 57},
                {'}', 1197},
                {'>', 25137}
            };



            _output.WriteLine($"{illegalChars.Sum(x => points[x])}");

        }
        
        [Fact]
        public void Day10_Part2()
        {
            var syntaxLines = File.ReadLines("Day10/input.txt").ToList();

            HashSet<char> openingChars = new(){'{', '[', '(', '<'};
            var points = new Dictionary<char, int>()
            {
                {'(', 1},
                {'[', 2},
                {'{', 3},
                {'<', 4}
            };

            var results = new List<long>();
            
            foreach (var line in syntaxLines)
            {
                var chunkOpenings = new Stack<char>();
                bool valid = true;
                foreach (var c in line)
                {
                    if (openingChars.Contains(c))
                    {
                        chunkOpenings.Push(c);
                        continue;
                    }

                    var pop = chunkOpenings.Pop();
                    if(pop == c-2 || pop == c-1)
                        continue;

                    valid = false;
                    break;
                }
                
                if(!valid)
                    continue;
                
                long result = 0;
                foreach (var openingChar in chunkOpenings.ToList())
                {
                    result *= 5;
                    result += points[openingChar];
                }
                
                results.Add(result);
            }


            var sortedResults = results.OrderBy(x => x);
            var middle = results.Count / 2;
            var answer = sortedResults.ElementAt(middle);

            _output.WriteLine($"{answer}");

        }

    }
}

