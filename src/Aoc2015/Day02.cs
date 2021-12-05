namespace Aoc2015;

public class Day02 : BaseDay
{
    private readonly List<Present> _presents;

    public Day02()
    {
        _presents = ParseInput();
    }

    public override ValueTask<string> Solve_1()
    {
        var sum = _presents.Sum(x => x.CalculateWrappingPaper());
        return new ValueTask<string>(sum.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        var sum = _presents.Sum(x => x.CalculateRibbon());
        return new ValueTask<string>(sum.ToString());
    }

    private List<Present> ParseInput()
    {
        return File.ReadAllLines(InputFilePath).Select(x => new Present(x)).ToList();
    }

    private class Present
    {
        public Present(string line)
        {
            var split = line.Split("x", StringSplitOptions.RemoveEmptyEntries);
            Length = int.Parse(split[0]);
            Width = int.Parse(split[1]);
            Height = int.Parse(split[2]);
        }


        public int Length { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public long CalculateWrappingPaper() => 2 * Length * Width + 2 * Width * Height + 2 * Height * Length + CalculateAreaOfSmallestSide();

        public long CalculateRibbon() => CalculateShortestSidesForRibbon() + Length * Width * Height;

        private int CalculateAreaOfSmallestSide() => Math.Min(Math.Min(Width * Length, Width * Height), Length * Height);

        private int CalculateShortestSidesForRibbon() => Math.Min(Math.Min(2 * Length + 2 * Width, 2 * Length + 2 * Height), 2 * Width + 2 * Height);
    }
}