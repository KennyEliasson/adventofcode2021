using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Solutions
{
    public class Day4
    {
        private readonly ITestOutputHelper _output;

        public Day4(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void Day4_Part1()
        {
            var input = File.ReadAllLines("Day4/day4input.txt").ToList();

            var (numbersDrawn, boards) = ParseInput(input);

            bool winnerFound = false;
            foreach (var drawnNumber in numbersDrawn)
            {
                if (winnerFound)
                    break;
                
                foreach (var board in boards)
                {
                    board.Check(drawnNumber);
                    if (board.HasFullLine())
                    {
                        winnerFound = true;
                        var sumOfUnmarkedNumbers = board.GetAllUnmarkedNumbers().Sum(x => x.Number);
                        _output.WriteLine($"Result is {sumOfUnmarkedNumbers*drawnNumber}");
                        break;
                    }
                }
            }
        }
        
        [Fact]
        public void Day4_Part2()
        {
            var input = File.ReadAllLines("Day4/day4input.txt").ToList();

            var (numbersDrawn, boards) = ParseInput(input);

            foreach (var drawnNumber in numbersDrawn)
            {
                foreach (var board in boards.Where(x => !x.HasWon))
                {
                    board.Check(drawnNumber);
                    if (board.HasFullLine())
                    {
                        var sumOfUnmarkedNumbers = board.GetAllUnmarkedNumbers().Sum(x => x.Number);
                        _output.WriteLine($"Result is {sumOfUnmarkedNumbers*drawnNumber}");
                    }
                }
            }
        }
        
        private static (IEnumerable<int>? numbersDrawn, List<BingoBoard> boards) ParseInput(List<string>? input)
        {
            var numbersDrawn = input[0].Split(',').Select(int.Parse);

            BingoBoard current = new();
            List<BingoBoard> boards = new();
            foreach (var row in input.Skip(2))
            {
                if (string.IsNullOrWhiteSpace(row))
                {
                    boards.Add(current);
                    current = new BingoBoard();
                    continue;
                }

                current.AddRow(row.Split(' ').Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.Trim()).Select(int.Parse)
                    .ToList());
            }

            boards.Add(current);
            return (numbersDrawn, boards);
        }

        public class Coord
        {
            public Coord(int row, int column, int number)
            {
                Row = row;
                Column = column;
                Number = number;
            }

            public int Row { get; }
            public int Column { get; }
            public int Number { get; }
            public bool Marked { get; set; }
        }

        public class BingoBoard
        {
            private readonly bool[,] _cells = new bool[5, 5];
            private readonly Dictionary<int, (int row, int column)> _numbers = new();
            private readonly List<Coord> _coords = new();
            private int _currentRowAdded;
            public bool HasWon { get; set; }

            public void AddRow(List<int> row)
            {
                for (int column = 0; column < 5; column++)
                {
                    _coords.Add(new Coord(_currentRowAdded, column, row[column]));
                    _cells[_currentRowAdded, column] = false;
                    _numbers.Add(row[column], (_currentRowAdded, column));
                }

                _currentRowAdded++;
            }

            public void Check(int drawnNumber)
            {
                if (_numbers.ContainsKey(drawnNumber))
                {
                    _coords.First(x => x.Number == drawnNumber).Marked = true;
                    var position = _numbers[drawnNumber];
                    _cells[position.row, position.column] = true;
                }
            }

            public IEnumerable<Coord> GetAllUnmarkedNumbers()
            {
                return _coords.Where(x => !x.Marked);
            }
            
            public bool HasFullLine()
            {
                for (int row = 0; row < _cells.GetLength(0); row++)
                {
                    var fullRow = Enumerable.Range(0, _cells.GetLength(1)).All(col => _cells[row, col]);
                    if (fullRow)
                    {
                        HasWon = true;
                        return true;
                    }
                        
                }

                for (int col = 0; col < _cells.GetLength(1); col++)
                {
                    var fullCol = Enumerable.Range(0, _cells.GetLength(0)).All(row => _cells[row, col]);
                    if (fullCol)
                    {
                        HasWon = true;
                        return true;
                    }
                }

                return false;
            }
        }

    }
}

