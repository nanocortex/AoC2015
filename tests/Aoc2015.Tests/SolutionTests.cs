using System;
using System.Threading.Tasks;
using AoCHelper;
using Shouldly;
using Xunit;
using Xunit.Sdk;

namespace Aoc2015.Tests;

public class SolutionTests
{
    [Theory]
    [InlineData(typeof(Day01), "232", "1783")]
    [InlineData(typeof(Day02), "1598415", "3812909")]
    [InlineData(typeof(Day03), "2565", "2639")]
    [InlineData(typeof(Day04), "117946", "3938038")]
    [InlineData(typeof(Day05), "236", "51")]
    [InlineData(typeof(Day06), "543903", "14687245")]
    [InlineData(typeof(Day07), "46065", "14134")]
    [InlineData(typeof(Day08), "1333", "2046")]
    [InlineData(typeof(Day09), "117", "909")]
    [InlineData(typeof(Day10), "360154", "5103798")]
    [InlineData(typeof(Day11), "cqjxxyzz", "cqkaabcc")]
    public async Task Test(Type type, string solution1, string solution2)
    {
        if (Activator.CreateInstance(type) is BaseDay instance)
        {
            var answer1 = await instance.Solve_1();
            var answer2 = await instance.Solve_2();

            if (!string.IsNullOrWhiteSpace(solution1))
                answer1.ShouldBe(solution1);
            if (!string.IsNullOrWhiteSpace(solution2))
                answer2.ShouldBe(solution2);
        }
        else
        {
            throw new XunitException();
        }
    }
}