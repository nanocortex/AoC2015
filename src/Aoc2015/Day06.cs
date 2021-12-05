using FileParser;

namespace Aoc2015;

public class Day06 : BaseDay
{
    private readonly List<Instruction> _instructions;

    public Day06()
    {
        _instructions = ParseInput().ToList();
    }

    public override ValueTask<string> Solve_1()
    {
        var grid = new Grid();
        foreach (var instruction in _instructions)
        {
            grid.ApplyInstruction(instruction);
        }

        return new ValueTask<string>(grid.CountLitLights().ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        var grid = new Grid();
        foreach (var instruction in _instructions)
        {
            grid.ApplyNewInstruction(instruction);
        }

        return new ValueTask<string>(grid.SumBrightness().ToString());
    }

    private IEnumerable<Instruction> ParseInput()
    {
        var file = new ParsedFile(InputFilePath, new[] { ' ', ',' });
        while (!file.Empty)
        {
            yield return Instruction.Parse(file.NextLine());
        }
    }

    public class Instruction
    {
        private Instruction(Operation operation, Point startPoint, Point endPoint)
        {
            Operation = operation;
            StartPoint = startPoint;
            EndPoint = endPoint;
        }

        public Operation Operation { get; set; }

        public Point StartPoint { get; set; }

        public Point EndPoint { get; set; }

        public static Instruction Parse(IParsedLine line)
        {
            var op = line.NextElement<string>();

            var s = line.PeekNextElement<string>();
            if (s is "on" or "off")
            {
                op += " " + line.NextElement<string>();
            }


            var x1 = line.NextElement<int>();
            var y1 = line.NextElement<int>();
            line.NextElement<string>();
            var x2 = line.NextElement<int>();
            var y2 = line.NextElement<int>();

            var operation = op switch
            {
                "toggle" => Operation.Toggle,
                "turn on" => Operation.TurnOn,
                "turn off" => Operation.TurnOff,
                _ => Operation.Unknown
            };

            return new Instruction(operation, new Point(x1, y1), new Point(x2, y2));
        }
    }

    public class Grid
    {
        private readonly int[,] _grid = new int[1000, 1000];

        public Grid()
        {
            for (var i = 0; i < 1000; i++)
            {
                for (var j = 0; j < 1000; j++)
                {
                    _grid[i, j] = 0;
                }
            }
        }

        public void ApplyInstruction(Instruction instruction)
        {
            for (var y = instruction.StartPoint.Y1; y <= instruction.EndPoint.Y1; y++)
            {
                for (var x = instruction.StartPoint.X1; x <= instruction.EndPoint.X1; x++)
                {
                    _grid[y, x] = instruction.Operation switch
                    {
                        Operation.TurnOn => 1,
                        Operation.TurnOff => 0,
                        Operation.Toggle => _grid[y, x] == 1 ? 0 : 1,
                        _ => _grid[y, x]
                    };
                }
            }
        }

        public void ApplyNewInstruction(Instruction instruction)
        {
            for (var y = instruction.StartPoint.Y1; y <= instruction.EndPoint.Y1; y++)
            {
                for (var x = instruction.StartPoint.X1; x <= instruction.EndPoint.X1; x++)
                {
                    _grid[y, x] = instruction.Operation switch
                    {
                        Operation.TurnOn => _grid[y, x] + 1,
                        Operation.TurnOff => _grid[y, x] == 0 ? _grid[y, x] : _grid[y, x] - 1,
                        Operation.Toggle => _grid[y, x] + 2,
                        _ => _grid[y, x]
                    };
                }
            }
        }

        public int CountLitLights()
        {
            var count = 0;
            for (var i = 0; i < _grid.GetLength(0); i++)
            {
                for (var j = 0; j < _grid.GetLength(1); j++)
                {
                    if (_grid[i, j] == 1)
                        count++;
                }
            }

            return count;
        }

        public long SumBrightness()
        {
            var sum = 0L;
            for (var i = 0; i < _grid.GetLength(0); i++)
            {
                for (var j = 0; j < _grid.GetLength(1); j++)
                {
                    sum += _grid[i, j];
                }
            }

            return sum;
        }
    }

    public record Point(int X1, int Y1);

    public enum Operation
    {
        Unknown,
        Toggle,
        TurnOff,
        TurnOn
    }
}