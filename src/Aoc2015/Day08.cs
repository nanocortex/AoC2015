using System.Text;

namespace Aoc2015;

public class Day08 : BaseDay
{
    private readonly List<string> _strings;

    public Day08()
    {
        _strings = ParseInput();
    }

    public override ValueTask<string> Solve_1()
    {
        var characterCount = 0;
        var codeCount = 0;
        foreach (var number in _strings.Select(GetCounts))
        {
            codeCount += number.CodeCount;
            characterCount += number.CharacterCount;
        }

        var total = codeCount - characterCount;
        return new ValueTask<string>(total.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        var characterCount = 0;
        var codeCount = 0;
        foreach (var number in _strings.Select(Encode).Select(GetCounts))
        {
            codeCount += number.CodeCount;
            characterCount += number.CharacterCount;
        }

        var total = codeCount - characterCount;
        return new ValueTask<string>(total.ToString());
    }

    private StringCounts GetCounts(string str)
    {
        var characterCount = 0;
        var codeCount = str.Length;
        var s = str.TrimStart('"').TrimEnd('"');

        for (var i = 0; i < s.Length; i++)
        {
            var c = s[i];

            switch (c)
            {
                case '\\' when i < s.Length - 1 && s[i + 1] == 'x':
                    i += 3;
                    break;
                case '\\':
                    i++;
                    break;
            }

            characterCount++;
        }

        return new StringCounts(codeCount, characterCount);
    }

    private string Encode(string str)
    {
        var sb = new StringBuilder();
        sb.Append('"');

        foreach (var c in str)
        {
            switch (c)
            {
                case '"':
                    sb.Append('\\');
                    sb.Append('"');
                    break;
                case '\\':
                    sb.Append('\\');
                    sb.Append('\\');
                    break;
                default:
                    sb.Append(c);
                    break;
            }
        }

        sb.Append('"');
        return sb.ToString();
    }

    private List<string> ParseInput()
    {
        return File.ReadAllLines(InputFilePath).ToList();
    }

    private record StringCounts(int CodeCount, int CharacterCount);
}