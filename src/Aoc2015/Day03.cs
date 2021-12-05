namespace Aoc2015;

public class Day03 : BaseDay
{
    private readonly List<Direction> _directions;

    public Day03()
    {
        _directions = ParseInput();
    }

    public override ValueTask<string> Solve_1()
    {
        var map = new Map(1);
        foreach (var direction in _directions)
        {
            map.ApplyDirection(direction, 0);
        }

        return new ValueTask<string>(map.CalculateHouses().ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        var map = new Map(2);
        var santa = 0;
        foreach (var direction in _directions)
        {
            santa = santa == 0 ? 1 : 0;
            map.ApplyDirection(direction, santa);
        }

        return new ValueTask<string>(map.CalculateHouses().ToString());
    }

    private List<Direction> ParseInput()
    {
        var data = File.ReadAllText(InputFilePath);

        return data.Select(x =>
        {
            return x switch
            {
                '^' => Direction.Up,
                '>' => Direction.Right,
                'v' => Direction.Down,
                '<' => Direction.Left,
                _ => Direction.None
            };
        }).ToList();
    }

    private enum Direction
    {
        Up,
        Right,
        Down,
        Left,
        None
    }

    private class Map
    {
        private readonly List<House> _points = new();
        private readonly List<Location> _santas = new();

        public Map(int santas)
        {
            for (var i = 0; i < santas; i++)
            {
                _santas.Add(new Location());
                ApplyPoint(_santas[i].X, _santas[i].Y);
            }
        }

        public void ApplyDirection(Direction direction, int santa)
        {
            switch (direction)
            {
                case Direction.Up:
                    _santas[santa].Y--;
                    break;
                case Direction.Right:
                    _santas[santa].X++;
                    break;
                case Direction.Down:
                    _santas[santa].Y++;
                    break;
                case Direction.Left:
                    _santas[santa].X--;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }

            ApplyPoint(_santas[santa].X, _santas[santa].Y);
        }

        private void ApplyPoint(int x, int y)
        {
            var point = _points.FirstOrDefault(house => house.X == x && house.Y == y);

            if (point == null)
            {
                _points.Add(new House(x, y));
            }
            else
            {
                point.Increment();
            }
        }

        public int CalculateHouses()
        {
            return _points.Count;
        }
    }

    private class Location
    {
        public Location()
        {
            X = 0;
            Y = 0;
        }

        public int X { get; set; }
        public int Y { get; set; }
    }

    private class House
    {
        public int X { get; set; }
        public int Y { get; set; }
        private int Presents { get; set; }


        public House(int x, int y)
        {
            X = x;
            Y = y;
            Presents = 1;
        }

        public void Increment()
        {
            Presents++;
        }
    }
}