using System.Security.Cryptography;
using System.Text;

namespace Aoc2015;

public sealed class Day04 : BaseDay
{
    private readonly string _key;

    public Day04()
    {
        _key = File.ReadAllText(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        return new ValueTask<string>(FindNumber("00000").ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        return new ValueTask<string>(FindNumber("000000").ToString());
    }

    private long FindNumber(string startWith)
    {
        var number = 0;
        var md5 = string.Empty;
        while (!md5.StartsWith(startWith))
        {
            number++;
            md5 = CreateMd5($"{_key}{number}");
        }

        return number;
    }

    private static string CreateMd5(string input)
    {
        using var md5 = MD5.Create();
        var inputBytes = Encoding.ASCII.GetBytes(input);
        var hashBytes = md5.ComputeHash(inputBytes);

        var sb = new StringBuilder();
        foreach (var t in hashBytes)
        {
            sb.Append(t.ToString("X2"));
        }

        return sb.ToString();
    }
}