namespace Aoc2015;

public class Day05 : BaseDay
{
    private readonly string[] _strings;

    public Day05()
    {
        _strings = ParseInput();
    }

    public override ValueTask<string> Solve_1()
    {
        var count = _strings.Where(IsNice).Count();
        return new ValueTask<string>(count.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        var count = _strings.Where(IsNiceBetter).Count();
        return new ValueTask<string>(count.ToString());
    }

    private bool IsNice(string s)
    {
        if (s.Count(x => "aeiou".Contains(x)) < 3)
            return false;

        var hasDouble = false;
        for (var index = 1; index < s.Length; index++)
        {
            var c = s[index];
            if (c != s[index - 1]) continue;

            hasDouble = true;
            break;
        }

        if (!hasDouble)
            return false;

        return !s.Contains("ab") && !s.Contains("cd") && !s.Contains("pq") && !s.Contains("xy");
    }

    private bool IsNiceBetter(string s)
    {
        var letterInMiddle = false;
        for (var index = 1; index < s.Length - 1; index++)
        {
            var triplet = $"{s[index - 1]}{s[index]}{s[index + 1]}";
            if (triplet[0] == triplet[2])
                letterInMiddle = true;
        }

        var pairs = new Dictionary<string, int>();
        string? lastPair = null;
        var overlapping = true;
        for (var index = 1; index < s.Length; index++)
        {
            var pair = $"{s[index - 1]}{s[index]}";
            if (lastPair == null || lastPair != pair || overlapping)
            {
                if (pairs.ContainsKey(pair))
                    pairs[pair]++;
                else
                    pairs.Add(pair, 1);
            }

            overlapping = pair == lastPair && !overlapping;
            lastPair = pair;
        }

        return letterInMiddle && pairs.Values.Any(x => x > 1);
    }

    private string[] ParseInput() => File.ReadAllLines(InputFilePath);
}