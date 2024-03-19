
using Xunit;
using System.Linq;

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MatCom.Tester;

public class UnitTest
{


    public static IEnumerable<object[]> ProblemsData(bool repeatedValues)
    {
        int seed = 2024;
        int amount = 100;
        var gestor = new ProblemGestor(seed);

        var problems = gestor.GetProblems(amount, repeatedValues);
        var solutions = gestor.GetSolutions();

        for (int i = 0; i < amount; i++)
        {
            yield return new object[] { problems[i].Item1, problems[i].Item2, problems[i].Item3, solutions[i] };
        }

    }
    public static IEnumerable<object[]> RepeatedValuesData() => ProblemsData(true);
    public static IEnumerable<object[]> NotRepeatedValuesData() => ProblemsData(false);


    [Theory]
    [MemberData(nameof(RepeatedValuesData))]
    public void SolvingProblems(int[,] matrix, int[] factors, int[] operations, int expected)
    {
        var result = Utils.SolveProblem(matrix, factors, operations);
        Assert.Equal(expected, result);
    }

    [Theory]
    [MemberData(nameof(NotRepeatedValuesData))]
    public void NotRepeatedValues(int[,] matrix, int[] factors, int[] operations, int expected)
    {
        var result = Utils.SolveProblem(matrix, factors, operations);
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(1)]
    public void EmptyOperators(int seed)
    {
        var matrix = new int[,] { { 1, 2, 3 },
                                  { 1, 2, 3 },
                                  { 1, 2, 3 } };
        var factors = new int[] { 5, -2, 0 };
        var operations = new int[] { };
        var expected = 6;
        var result = ProblemGestor.Solve(matrix, factors, operations);
        Assert.Equal(expected, result);
    }

}

