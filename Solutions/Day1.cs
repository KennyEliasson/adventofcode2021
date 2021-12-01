using System;
using System.IO;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Solutions
{
    public class Day1
    {
        private readonly ITestOutputHelper _output;

        public Day1(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void Day1_Part1()
        {
            var depthMeasurements = File.ReadAllLines("day1input.txt").Select(x => Convert.ToInt32(x)).ToList();

            int increases = 0;
            for (int i = 0; i < depthMeasurements.Count; i++)
            {
                if (i == 0)
                    continue;

                if (depthMeasurements[i] > depthMeasurements[i - 1])
                {
                    increases++;
                }
            }

            _output.WriteLine($"Answer is {increases}");
        }

        [Fact]
        public void Day1_Part2()
        {
            var depthMeasurements = File.ReadAllLines("day1input.txt").Select(x => Convert.ToInt32(x)).ToList();

            int increases = 0;
            for (int i = 0; i < depthMeasurements.Count; i++)
            {
                if (i + 2 >= depthMeasurements.Count)
                    break;

                if (i - 1 < 0)
                    continue;

                var slidingSum = depthMeasurements[i] + depthMeasurements[i + 1] + depthMeasurements[i + 2];
                var previousSlidingSum = depthMeasurements[i - 1] + depthMeasurements[i] + depthMeasurements[i + 1];

                if (slidingSum > previousSlidingSum)
                    increases++;
            }

            _output.WriteLine($"Answer is {increases}");
        }
    }
}

