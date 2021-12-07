using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Solutions
{
    public class Day7
    {
        private readonly ITestOutputHelper _output;

        public Day7(ITestOutputHelper output)
        {
            _output = output;
        }
        
        [Fact]
        public void Day7_Part1()
        {
            var crabPositions = File.ReadAllText("Day7/input.txt").Split(',').Select(int.Parse).ToList();

            var maxPosition = crabPositions.Max();
            var minPosition = crabPositions.Min();

            Dictionary<int, int> stepsToPosition = new();
            for (var i = minPosition; i <= maxPosition; i++)
            {
                stepsToPosition[i] = crabPositions.Sum(position => Math.Abs(position - i));
            }

            _output.WriteLine($"The answer is {stepsToPosition.Min(x => x.Value)}");
        }
        
        [Fact]
        public void Day7_Part2()
        {
            var crabPositions = File.ReadAllText("Day7/input.txt").Split(',').Select(int.Parse).ToList();

            var maxPosition = crabPositions.Max();
            var minPosition = crabPositions.Min();

            Dictionary<int, int> fuelToPosition = new();
            for (int i = minPosition; i <= maxPosition; i++)
            {
                var fuel = 0;
                foreach (var position in crabPositions)
                {
                    var steps = Math.Abs(position - i);
                    var crabFuel = steps switch
                    {
                        0 => 0,
                        1 => 1,
                        _ => steps * ((steps - 1) / (float)2 + 1)
                    };
                    fuel += (int) crabFuel;
                }

                fuelToPosition[i] = fuel;
            }

            _output.WriteLine($"The answer is {fuelToPosition.Min(x => x.Value)}");
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(2, 3)]
        [InlineData(3, 6)]
        [InlineData(4, 10)]
        [InlineData(5, 15)]
        [InlineData(6, 21)]
        public void A(int step, int result)
        {
            Assert.Equal(result, 
            step * ((step - 1) / (float)2 + 1)
            );
        }
    }
}

