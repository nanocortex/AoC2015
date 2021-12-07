using System.Text;

namespace Aoc2015;

public sealed class Day11 : BaseDay
{
    private readonly string _input;

    public Day11()
    {
        _input = File.ReadAllText(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        return new ValueTask<string>(IncrementUntilValid(_input));
    }

    public override ValueTask<string> Solve_2()
    {
        return new ValueTask<string>(IncrementUntilValid(Increment(IncrementUntilValid(_input))));
    }

    private string IncrementUntilValid(string password)
    {
        while (!Validate(password))
        {
            password = Increment(password);
        }

        return password;
    }

    private string Increment(string password)
    {
        var sb = new StringBuilder(password);
        for (var i = sb.Length - 1; i >= 0; i--)
        {
            if (sb[i] != 'z')
            {
                sb[i]++;
                break;
            }

            sb[i] = 'a';
        }

        return sb.ToString();
    }

    private bool Validate(string password)
    {
        if (password.Contains('i') || password.Contains('o') || password.Contains('l'))
            return false;

        return HasTriplet(password) && HasDouble(password);
    }

    private bool HasTriplet(string password)
    {
        for (var i = 0; i < password.Length - 2; i++)
        {
            if (password[i] != password[i + 1] - 1 || password[i] != password[i + 2] - 2) continue;
            return true;
        }

        return false;
    }

    private bool HasDouble(string password)
    {
        var pairs = 0;
        var overlapping = true;
        string? lastPair = null;
        for (var i = 1; i < password.Length; i++)
        {
            if (password[i - 1] != password[i])
                continue;

            var pair = $"{password[i - 1]}{password[i]}";


            if (lastPair == null || lastPair != pair || overlapping)
            {
                pairs++;
            }

            if (pairs == 2)
                return true;

            overlapping = pair == lastPair && !overlapping;
            lastPair = pair;
        }

        return false;
    }
}