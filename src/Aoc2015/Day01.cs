namespace Aoc2015;

public sealed class Day01 : BaseDay
{
    private readonly string _input;

    public Day01()
    {
        _input = File.ReadAllText(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        var floor = 0;

        foreach (var c in _input)
        {
            if (c == '(')
                floor++;
            else
                floor--;
        }

        return new ValueTask<string>(floor.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        int floor = 0, pos = 0;

        foreach (var c in _input)
        {
            if (c == '(')
                floor++;
            else
                floor--;

            pos++;

            if (floor == -1)
                break;
        }

        return new ValueTask<string>(pos.ToString());
    }
}