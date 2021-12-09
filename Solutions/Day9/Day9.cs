using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Solutions
{
    public class Day9
    {
        private readonly ITestOutputHelper _output;

        public Day9(ITestOutputHelper output)
        {
            _output = output;
        }
        
        [Fact]
        public void Day9_Part1()
        {
            var rawEntries = File.ReadLines("Day9/input.txt").ToList();

            var columns = rawEntries[0].Length;
            var rows = rawEntries.Count;

            var grid = new int[rows, columns];

            for (int row = 0; row < rows; row++)
            {
                for (var column = 0; column < columns; column++)
                {
                    var height = rawEntries[row][column];
                    grid[row, column] = int.Parse(height.ToString());
                }
            }


            var lowPoints = new List<int>();
            for (int row = 0; row < rows; row++)
            {
                for (int column = 0; column < columns; column++)
                {
                    var currentHeight = grid[row, column];

                    var left = grid.TryGetValue(row, column-1);
                    var right = grid.TryGetValue(row, column+1);
                    var top = grid.TryGetValue(row-1, column);
                    var bottom = grid.TryGetValue(row+1, column);

                    var adjacentPositions = new List<int?> {
                        left, right, top, bottom
                    };

                    
                    var hasLowerAdjacentPositions = adjacentPositions.Where(x => x != null).Any(x => currentHeight >= x);

                    if (!hasLowerAdjacentPositions)
                    {
                        lowPoints.Add(currentHeight);
                    }

                }
            }

            _output.WriteLine($"Answer is {lowPoints.Select(x => x+1).Sum()}");

        }
        
        [Fact]
        public void Day9_Part2()
        {
            var rawEntries = File.ReadLines("Day9/input.txt").ToList();

            var columns = rawEntries[0].Length;
            var rows = rawEntries.Count;

            var grid = new int[rows, columns];

            for (int row = 0; row < rows; row++)
            {
                for (var column = 0; column < columns; column++)
                {
                    var height = rawEntries[row][column];
                    grid[row, column] = int.Parse(height.ToString());
                }
            }

            
            var lowerGridPosition = new List<(int row, int column)>();
            for (int row = 0; row < rows; row++)
            {
                for (int column = 0; column < columns; column++)
                {
                    var currentHeight = grid[row, column];
                    var left = grid.TryGetValue(row, column-1);
                    var right = grid.TryGetValue(row, column+1);
                    var top = grid.TryGetValue(row-1, column);
                    var bottom = grid.TryGetValue(row+1, column);

                    var adjacentPositions = new List<int?> {
                        left, right, top, bottom
                    };
     
                    var hasLowerAdjacentPositions = adjacentPositions.Where(x => x != null).Any(x => currentHeight >= x);

                    if (!hasLowerAdjacentPositions)
                    {
                        lowerGridPosition.Add((row, column));
                    }

                }
            }

            var basinSizes = new List<int>();
            foreach (var lowestPosition in lowerGridPosition)
            {
                var basinSize = new Basin(rows, columns, lowestPosition);
                MoveThroughBasins(grid, lowestPosition.row, lowestPosition.column, basinSize);
                basinSizes.Add(basinSize.Size);
            }
            
            var top3 = basinSizes.OrderByDescending(x => x).Take(3).ToList();

            _output.WriteLine($"Answer is {top3[0] * top3[1] * top3[2]}");
        }
        
        private void MoveThroughBasins(int[,] grid, int row, int column, Basin basin)
        {
            var currentHeight = grid[row, column];
            if (currentHeight == 9) {
                return;
            }

            TryMove(grid, row, column-1, basin, currentHeight);
            TryMove(grid, row, column+1, basin, currentHeight);
            TryMove(grid, row-1, column, basin, currentHeight);
            TryMove(grid, row+1, column, basin, currentHeight);
        }

        private void TryMove(int[,] grid, int row, int column, Basin basin, int currentHeight)
        {
            var direction = grid.TryGetValue(row, column);
            if (!direction.HasValue)
                return;

            if (direction == 9)
                return;

            if (basin.Visited[row, column])
                return;
            
            
            if (currentHeight <= direction)
            {
                basin.Visited[row, column] = true;
                basin.Size++;
                MoveThroughBasins(grid, row, column, basin);
            }
        }
    }

    internal class Basin
    {
        public Basin(int rows, int columns, (int row, int column) lowestPosition)
        {
            Visited = new bool[rows, columns];
            Size = 1;
            Visited[lowestPosition.row, lowestPosition.column] = true;
        }

        public int Size { get; set; }
        public bool[,] Visited { get; }
    }

    public static class Extension
    {
        public static int? TryGetValue(this int[,] grid, int row, int column)
        {
            var maxRows = grid.GetUpperBound(0);
            var maxColumns = grid.GetUpperBound(1);
            
            if (row >= 0 && row <= maxRows && column >= 0 && column <= maxColumns)
            {
                return grid[row, column];
            }


            return null;
        }
    }
}

