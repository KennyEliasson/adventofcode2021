using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Solutions
{
    public class Day6
    {
        private readonly ITestOutputHelper _output;

        public Day6(ITestOutputHelper output)
        {
            _output = output;
        }

        class LanternFish
        {
            public int Timer { get; private set; }

            public LanternFish(int timer)
            {
                Timer = timer;
            }

            public LanternFish? Decrease()
            {
                Timer -= 1;
                if (Timer == -1)
                {
                    Timer = 6;
                    return new LanternFish(8);
                }

                return null;
            }
        }
    
        [Fact]
        public void Day6_Part1()
        {
            var fishes = File.ReadAllText("Day6/example.txt").Split(',').Select(int.Parse).Select(x => new LanternFish(x)).ToList();

            int days = 80;

            for (int i = 0; i < days; i++)
            {
                var newFishes = new List<LanternFish>();
                foreach (var fish in fishes)
                {
                    var lanternFish = fish.Decrease();
                    if (lanternFish != null)
                    {
                        newFishes.Add(lanternFish);
                    }   
                }
                
                fishes.AddRange(newFishes);
            }
            
            _output.WriteLine($"Total number of fishes are {fishes.Count}");
        }
        
        [Fact]
        public void Day6_Part2()
        {
            var fishes = File.ReadAllText("Day6/input.txt").Split(',').Select(int.Parse).Select(x => new LanternFish(x)).ToList();

            int days = 256;

            var fishesAtTimer = new Dictionary<int, long>()
            {
                {0, 0},
                {1, 0},
                {2, 0},
                {3, 0},
                {4, 0},
                {5, 0},
                {6, 0},
                {7, 0},
                {8, 0},
            };

            foreach (var fish in fishes)
            {
                fishesAtTimer[fish.Timer] += 1;
            }
            
            for (var i = 0; i < days; i++)
            {
                var fishesAt0 = fishesAtTimer[0];
                fishesAtTimer[0] = fishesAtTimer[1];
                fishesAtTimer[1] = fishesAtTimer[2];
                fishesAtTimer[2] = fishesAtTimer[3];
                fishesAtTimer[3] = fishesAtTimer[4];
                fishesAtTimer[4] = fishesAtTimer[5];
                fishesAtTimer[5] = fishesAtTimer[6];
                fishesAtTimer[6] = fishesAtTimer[7] + fishesAt0;
                fishesAtTimer[7] = fishesAtTimer[8];
                fishesAtTimer[8] = fishesAt0;
            }
            
            _output.WriteLine($"Total number of fishes are {fishesAtTimer.Sum(x => x.Value)}");
        }
    }
}

