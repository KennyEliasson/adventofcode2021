using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;
using Xunit.Abstractions;

namespace Solutions
{
    public class Day2
    {
        private readonly ITestOutputHelper _output;

        public Day2(ITestOutputHelper output)
        {
            _output = output;
        }

        abstract class MovementCommand {
            
            public MovementCommand(int steps) {
                Steps = steps;
            }

            public int Steps { get; }

            public abstract void Move(Position position);
            public abstract void Move(AimPosition position);
        }

        class Up : MovementCommand
        {
            public Up(int steps) : base(steps)
            { }

            public override void Move(Position position)
            {
                position.VerticalPosition -= Steps;
            }
            
            public override void Move(AimPosition position)
            {
                position.Aim -= Steps;
            }
        }
        
        class Down : MovementCommand
        {
            public Down(int steps) : base(steps)
            { }

            public override void Move(Position position)
            {
                position.VerticalPosition += Steps;
            }
            
            public override void Move(AimPosition position)
            {
                position.Aim += Steps;
            }
        }
        
        class Forward : MovementCommand
        {
            public Forward(int steps) : base(steps)
            { }

            public override void Move(Position position)
            {
                position.HorizontalPosition += Steps;
            }
            
            public override void Move(AimPosition position)
            {
                position.HorizontalPosition += Steps;
                position.VerticalPosition += position.Aim * Steps;
            }
        }

        class Position
        {
            public int HorizontalPosition { get; set; }
            public int VerticalPosition { get; set; }

            public int Answer()
            {
                return VerticalPosition * HorizontalPosition;
            }
        }
        
        class AimPosition
        {
            public int HorizontalPosition { get; set; }
            public int VerticalPosition { get; set; }
            public int Aim { get; set; }

            public int Answer()
            {
                return VerticalPosition * HorizontalPosition;
            }
        }

        private static List<MovementCommand> ParseMovementCommands()
        {
            Regex r = new("([A-z]*) (\\d*)");
            var movementCommands = File.ReadAllLines("day2/day2input.txt").ToList();
            var movements = new List<MovementCommand>();

            foreach (var movementCommand in movementCommands)
            {
                var match = r.Match(movementCommand);

                var steps = int.Parse(match.Groups[2].Value);
                switch (match.Groups[1].Value)
                {
                    case "up":
                        movements.Add(new Up(steps));
                        break;
                    case "down":
                        movements.Add(new Down(steps));
                        break;
                    case "forward":
                        movements.Add(new Forward(steps));
                        break;
                }
            }

            return movements;
        }

        [Fact]
        public void Day2_Part1()
        {
            var movements = ParseMovementCommands();

            var position = new Position();
            foreach (var command in movements)
            {
                command.Move(position);
            }
            
            _output.WriteLine($"Horizontal position: {position.HorizontalPosition}. Vertical position: {position.VerticalPosition}. Answer is {position.Answer()}");
        }

        [Fact]
        public void Day2_Part2()
        {
            var movements = ParseMovementCommands();

            var position = new AimPosition();
            foreach (var command in movements)
            {
                command.Move(position);
            }
            
            _output.WriteLine($"Horizontal position: {position.HorizontalPosition}. Vertical position: {position.VerticalPosition}. Answer is {position.Answer()}");
        }
    }
}

