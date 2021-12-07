using System.Text;

namespace Aoc2015;

public sealed class Day10 : BaseDay
{
    private readonly string _input;

    public Day10()
    {
        _input = File.ReadAllText(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        return new ValueTask<string>(Generate(_input, 40).Length.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        return new ValueTask<string>(Generate(_input, 50).Length.ToString());
    }


    private string Generate(string input, int times)
    {
        var output = input;
        for (var i = 0; i < times; i++)
        {
            output = Generate(output);
        }

        return output;
    }

    private string Generate(string s)
    {
        var sb = new StringBuilder();

        var lastChar = '?';
        var charCount = 0;

        for (var i = 0; i < s.Length; i++)
        {
            if (i == 0)
            {
                lastChar = s[i];
                if (i == s.Length - 1)
                    charCount++;
            }


            if (s[i] != lastChar || i == s.Length - 1)
            {
                if (i == s.Length - 1)
                {
                    if (s[i] == lastChar || i == 0)
                    {
                        sb.Append(charCount + 1);
                        sb.Append(lastChar);
                    }
                    else
                    {
                        sb.Append(charCount);
                        sb.Append(lastChar);
                        sb.Append(1);
                        sb.Append(s[i]);
                    }

                    break;
                }

                sb.Append(charCount);
                sb.Append(lastChar);
                lastChar = s[i];
                charCount = 0;
            }

            charCount++;
        }

        return sb.ToString();
    }
}