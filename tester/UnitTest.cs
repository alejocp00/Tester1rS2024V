
using Xunit;
using System.Linq;

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MatCom.Tester;

public class UnitTest
{
    // TODO: Aquí van las pruebas a ejecutar. Han de coincidir con el nombre de los enum de TestType

    [Theory]
    [InlineData(1)]
    public void AllPositive(int seed)
    {
        var generator = new ProblemGenerator(seed);
        var matrix = new int[,] {
            {1,2,3},
            {2,4,1},
            {3,1,2}
        };
        var factors = new int[] { 1, 2, 3 };
        var operations = new int[] { 1, 2 };
        var result = Utils.SolveProblem(matrix, factors, operations);
        /*
            312  1
            412  1   3
            312  1

            123  2
            241  4   6  9
            312  1
        */
        var expected = 9;
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(2)]
    public void FactorWithNegatives(int seed)
    {
        var generator = new ProblemGenerator(seed);
        var matrix = new int[,] {
            {1,2,3},
            {2,4,1},
            {3,1,2}
        };
        var factors = new int[] { 1, 3, -2 };
        var operations = new int[] { 1, 2 };
        var result = Utils.SolveProblem(matrix, factors, operations);
        /*
            312  1
            241  4   8
            231  3

            123  2
            241  4   3  11
            312  1
        */
        var expected = 11;
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(3)]
    public void OperatorsWithNegatives(int seed)
    {
        var generator = new ProblemGenerator(seed);
        var matrix = new int[,] {
            {1,2,3},
            {2,4,1},
            {3,1,2}
        };
        var factors = new int[] { 3, 1, 2 };
        var operations = new int[] { -1, -2 };
        var result = Utils.SolveProblem(matrix, factors, operations);
        /*
            123  2
            412  1   6
            231  3

            123  2
            241  4   5  11
            312  1
        */
        var expected = 11;
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(4)]
    public void AllNegatives(int seed)
    {
        var generator = new ProblemGenerator(seed);
        var matrix = new int[,] {
            {-1,-2,-3},
            {-2,-4,-1},
            {-3,-1,-2}
        };
        var factors = new int[] { -1, -2, -3 };
        var operations = new int[] { -1, -2 };
        var result = Utils.SolveProblem(matrix, factors, operations);
        /*
            -3 -1 -2  -1
            -4 -1 -2  -2  -3
            -3 -1 -2  -1

            -1 -2 -3  -2
            -2 -4 -1  -4  -6  -9
            -3 -1 -2  -1
        */
        var expected = -9;
        Assert.Equal(expected, result);
    }

    // [Theory]
    // [InlineData(5)]
    // public void NoOperators(int seed)
    // {
    //     var generator = new ProblemGenerator(seed);
    //     var matrix = new int[,] {
    //         {1,2,3},
    //         {2,4,1},
    //         {3,1,2}
    //     };
    //     var factors = new int[] { 1, 2, 3 };
    //     var operations = new int[] { };
    //     var result = Utils.SolveProblem(matrix, factors, operations);
    //     var expected = 7;
    //     Assert.Equal(expected, result);
    // }

    [Theory]
    [InlineData(6)]
    public void NonRepeatedValuesAllPositive(int seed)
    {
        var generator = new ProblemGenerator(seed);
        var matrix = new int[,] {
            {1,2,3},
            {4,5,6},
            {7,8,9}
        };
        var factors = new int[] { 1, 2, 3 };
        var operations = new int[] { 1, 2 };
        var result = Utils.SolveProblem(matrix, factors, operations);
        /*
            312  1
            564  6   15
            789  8

            123  2
            456  5   7  22
            789  8
        */
        var expected = 22;
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(7)]
    public void NonRepeatedValuesFactorWithNegatives(int seed)
    {
        var generator = new ProblemGenerator(seed);
        var matrix = new int[,] {
            {1,2,3},
            {4,5,6},
            {7,8,9}
        };
        var factors = new int[] { 1, 3, -2 };
        var operations = new int[] { 1, 2 };
        var result = Utils.SolveProblem(matrix, factors, operations);
        /*
            312  1
            456  5   13
            978  7

            123  2
            456  5   10  23
            789  8
        */
        var expected = 23;
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(8)]
    public void NonRepeatedValuesOperatorsWithNegatives(int seed)
    {
        var generator = new ProblemGenerator(seed);
        var matrix = new int[,] {
            {1,2,3},
            {4,5,6},
            {7,8,9}
        };
        var factors = new int[] { 3, 1, 2 };
        var operations = new int[] { -1, -2 };
        var result = Utils.SolveProblem(matrix, factors, operations);
        /*
            123  2
            564  6   15
            978  7

            123  2
            456  5   13  28
            789  8
        */
        var expected = 28;
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(9)]
    public void NonRepeatedValuesAllNegatives(int seed)
    {
        var generator = new ProblemGenerator(seed);
        var matrix = new int[,] {
            {-1,-2,-3},
            {-4,-5,-6},
            {-7,-8,-9}
        };
        var factors = new int[] { -1, -2, -3 };
        var operations = new int[] { -1, -2 };
        var result = Utils.SolveProblem(matrix, factors, operations);
        /*
            -3 -1 -2  -1
            -5 -6 -4  -6  -15
            -7 -8 -9  -8

            -1 -2 -3  -2
            -4 -5 -6  -5  -7  -22
            -7 -8 -9  -8
        */
        var expected = -22;
        Assert.Equal(expected, result);
    }

}

