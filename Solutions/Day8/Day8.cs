using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Solutions
{
    public class Day8
    {
        private readonly ITestOutputHelper _output;

        public Day8(ITestOutputHelper output)
        {
            _output = output;
        }
        
        [Fact]
        public void Day8_Part1()
        {
            var rawEntries = File.ReadLines("Day8/input.txt").ToList();

            var entries = new List<Entry>();
            foreach (var rawEntry in rawEntries)
            {
                var split = rawEntry.Split(" | ");
                var signalPatterns = split[0].Split(" ");
                var outputs = split[1].Split(" ");

                entries.Add(new Entry(signalPatterns, outputs));
            }

            _output.WriteLine($"Answer is {entries.Sum(x => x.SimpleDigitsInOutput().Count)}");
        }
        
        [Fact]
        public void Day8_Part2()
        {
            var rawEntries = File.ReadLines("Day8/input.txt").ToList();

            var entries = new List<Entry>();
            foreach (var rawEntry in rawEntries)
            {
                var split = rawEntry.Split(" | ");
                var signalPatterns = split[0].Split(" ");
                var outputs = split[1].Split(" ");

                entries.Add(new Entry(signalPatterns, outputs));
            }

            var answer = 0;
            foreach (var entry in entries)
            {
                answer += entry.Calc();
            }
            

            _output.WriteLine($"Answer is {answer}");
        }
    }
    
    /*public string[] Zero = "abcefg";
    
    0 = abcefg
    1 = cf
    2 = acdeg
    3 = acdfg
    4 = bcdf
    5 = abdfg
    6 = abdefg
    7 = acf
    8 = abcdefg
    9 = abcdfg*/

    public class Entry  
    {
        private readonly string[] _signalPatterns;
        private readonly string[] _outputs;

        public Entry(string[] signalPatterns, string[] outputs)
        {
            _signalPatterns = signalPatterns;
            _outputs = outputs;
        }

        public HashSet<char> One()
        {
            return UniqueLengthMatch(2);
        }
        
        public HashSet<char> Four()
        {
            return UniqueLengthMatch(4);
        }
        
        public HashSet<char> Seven()
        {
            return UniqueLengthMatch(3);
        }

        private HashSet<char> UniqueLengthMatch(int length)
        {
            var matches = _signalPatterns.Where(x => x.Length == length).ToList();
            // matches.AddRange(_outputs.Where(x => x.Length == length));
            return new HashSet<char>(matches.SingleOrDefault()?.ToList() ?? new List<char>());
        }
        
        private HashSet<string> LengthMatch(int length)
        {
            var matches = _signalPatterns.Where(x => x.Length == length).ToList();
            // matches.AddRange(_outputs.Where(x => x.Length == length));
            return new HashSet<string>(matches.Distinct());
        }

        public int Calc()
        {
            var digits = Enumerable.Range(0, 10).Select(_ => new HashSet<char>()).ToList();
            
            var one = One();
            var seven = Seven();
            var four = Four();
            var digitsWithLengthEqSix = LengthMatch(6); // 6, 9, 0
            var digitsWithLengthEqFive = LengthMatch(5); // 2, 3, 5
            
            digits[1] = one;
            digits[8] = UniqueLengthMatch(7);
            digits[7] = seven;
            digits[4] = four;

            foreach (var s in digitsWithLengthEqSix)
            {
                var hash = new HashSet<char>(s);
                var intersectionWithOne = hash.Intersect(one).ToList();
                var intersectionWithFour = four.Intersect(hash).Except(one).ToList();
                if (intersectionWithOne.Count == 1)
                {
                    
                    digits[6] = hash;
                }
                else if (intersectionWithFour.Count == 1)
                {
                    digits[0] = hash;
                }
                else
                {
                    digits[9] = hash;
                }
            }
            
            foreach (var s in digitsWithLengthEqFive)
            {
                var hash = new HashSet<char>(s);
                var intersectionWithSix = digits[6].Except(s).ToList();
                var intersectionWithNine = digits[9].Except(s).ToList();
                if (intersectionWithSix.Count == 1)
                {
                    digits[5] = hash;
                } else if (intersectionWithNine.Count == 1)
                {
                    digits[3] = hash;
                }
                else
                {
                    digits[2] = hash;
                }
            }


            var answer = "";
            foreach (var output in _outputs)
            {
                for (int i = 0; i < digits.Count; i++)
                {
                    if(digits[i].Count != output.Length)
                        continue;

                    if (digits[i].Except(output).Count() == 0)
                    {
                        answer += i.ToString();
                        break;
                    }
                }

            }

            return int.Parse(answer);
        }
        
        public List<string> SimpleDigitsInOutput()
        {
            return _outputs.Where(x => x.Length is 2 or 3 or 4 or 7).ToList();
        }
    }
}

