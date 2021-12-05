using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;
using Xunit.Abstractions;

namespace Solutions
{
    public class Day5
    {
        private readonly ITestOutputHelper _output;

        public Day5(ITestOutputHelper output)
        {
            _output = output;
        }

        public class Line
        {
            public Line(Coord start, Coord end)
            {
                Start = start;
                End = end;
            }

            public Coord Start { get; }
            public Coord End { get; }

            public IEnumerable<Coord> GetLineCoordinates(bool buildDiagonal)
            {
                if (Start.row == End.row)
                {
                    if (Start.col < End.col)
                    {
                        return Enumerable.Range(Start.col, (End.col - Start.col)+1).Select(col => new Coord(col, Start.row));
                    }
                    return Enumerable.Range(End.col, (Start.col - End.col)+1).Select(col => new Coord(col, Start.row));
                }

                if(Start.col == End.col)
                {
                    if (Start.row < End.row)
                    {
                        return Enumerable.Range(Start.row, (End.row - Start.row)+1).Select(row => new Coord(Start.col, row));
                    }
                    return Enumerable.Range(End.row, (Start.row - End.row)+1).Select(row => new Coord(Start.col, row));
                }

                if (!buildDiagonal)
                    return new List<Coord>();

                if (Math.Abs(Start.col - End.col) == Math.Abs(Start.row - End.row))
                {
                    var colStep = Start.col > End.col ? -1 : 1;
                    var rowStep = Start.row > End.row ? -1 : 1;

                    return Diagonal(Start, colStep, rowStep);
                }

                throw new NotImplementedException();
            }

            private IEnumerable<Coord> Diagonal(Coord current, int colSteps, int rowSteps)
            {
                while (true)
                {
                    yield return current;
                    if (current == End)
                        yield break;
                    current = new Coord(current.col + colSteps, current.row + rowSteps);
                }
            }
        }

        public record Coord(int col, int row);
        
        [Fact]
        public void Day5_Part1()
        {
            var input = File.ReadAllLines("Day5/input.txt").ToList();

            var lines = ParseLines(input);

            Dictionary<string, int> coordinatesPoint = new();
            foreach (var line in lines)
            {
                var lineCoords = line.GetLineCoordinates(buildDiagonal:false).ToList();
                foreach (var coordinate in lineCoords)
                {
                    var key = $"{coordinate.col}_{coordinate.row}";
                    if (coordinatesPoint.ContainsKey(key))
                    {
                        coordinatesPoint[key] = coordinatesPoint[key]+1;
                        continue;
                    }
                    coordinatesPoint.Add(key, 1);
                }
            }

            _output.WriteLine($"Answer is {coordinatesPoint.Count(x => x.Value >= 2)}");
        }
        
        [Fact]
        public void Day5_Part2()
        {
            var input = File.ReadAllLines("Day5/input.txt").ToList();

            var lines = ParseLines(input);

            Dictionary<string, int> coordinatesPoint = new();
            foreach (var line in lines)
            {
                var lineCoords = line.GetLineCoordinates(buildDiagonal:true).ToList();
                foreach (var coordinate in lineCoords)
                {
                    var key = $"{coordinate.col}_{coordinate.row}";
                    if (coordinatesPoint.ContainsKey(key))
                    {
                        coordinatesPoint[key] = coordinatesPoint[key]+1;
                        continue;
                    }
                    coordinatesPoint.Add(key, 1);
                }
            }

            _output.WriteLine($"Answer is {coordinatesPoint.Count(x => x.Value >= 2)}");
        }

        private static List<Line> ParseLines(List<string> input)
        {
            var regex = new Regex("(\\d*),(\\d*) -> (\\d*),(\\d*)");
            var lines = new List<Line>();
            foreach (var row in input)
            {
                var match = regex.Match(row);

                var start = new Coord(int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value));
                var end = new Coord(int.Parse(match.Groups[3].Value), int.Parse(match.Groups[4].Value));

                var line = new Line(start, end);
                lines.Add(line);
            }

            return lines;
        }
    }
}

