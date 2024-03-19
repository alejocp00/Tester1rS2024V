
using Xunit;
using System.Linq;
using MatCom.Examen;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;
using System.Text.Json;
namespace MatCom.Tester;

public enum TestType
{
    // TODO: Aquí se definen los tipos de test. Han de coincidir con el nombre de las pruebas a ejecutar
    AllPositive,
    FactorWithNegatives,
    OperatorsWithNegatives,
    AllNegatives,
    // NoOperators,
    NonRepeatedValuesAllPositive,
    NonRepeatedValuesFactorWithNegatives,
    NonRepeatedValuesOperatorsWithNegatives,
    NonRepeatedValuesAllNegatives,

}

public static class Utils
{
    public static int SolveProblem(int[,] matrix, int[] factors, int[] operations)
    {
        return JaimesPineapple.Solve(matrix, factors, operations);
    }

}

public class ProblemGenerator
{
    public int Seed { get; }
    private Random randomGenerator;

    public ProblemGenerator(int seed)
    {
        Seed = seed;
        randomGenerator = new Random(seed);

    }

    private int[] GenerateArray(int minFactorsSize, int maxFactorSize, int minValue, int maxValue, bool fixedArray = false)
    {
        var result = fixedArray ? new int[maxFactorSize] : new int[randomGenerator.Next(minFactorsSize, maxFactorSize)];
        for (int i = 0; i < result.Length; i++)
        {
            result[i] = randomGenerator.Next(minValue, maxValue);
        }
        return result;
    }

    private int[,] GenerateMatrix(int rows, int columns, int minValue, int maxValue, bool repeatedValues)
    {
        var result = new int[rows, columns];
        int counter = 1;
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                int number = repeatedValues ? randomGenerator.Next(minValue, maxValue) : counter++;
                result[i, j] = number;
            }
        }
        return result;
    }

    public Tuple<int[,], int[], int[]> GetProblem(int rows, int columns, int minValueMatrix, int maxValueMatrix, bool repeatedValues, int minFactorsSize, int maxFactorSize, int minValueFactors, int maxValueFactors, int minOperationsSize, int maxOperationsSize, int minValueOperations, int maxValueOperations)
    {
        var matrix = GenerateMatrix(rows, columns, minValueMatrix, maxValueMatrix, repeatedValues);
        var factors = GenerateArray(minFactorsSize, rows, minValueFactors, maxValueFactors, true);
        var operations = GenerateArray(minOperationsSize, maxOperationsSize, minValueOperations, maxValueOperations);
        return new Tuple<int[,], int[], int[]>(matrix, factors, operations);
    }
}

public class ProblemGestor
{
    public int Seed { get; }
    public int SeedForLimits { get; }
    private List<Tuple<int[,], int[], int[]>> problems;
    public ProblemGestor(int seed)
    {
        Seed = seed;
        SeedForLimits = seed - 1;
        problems = new List<Tuple<int[,], int[], int[]>>();
    }

    public List<Tuple<int[,], int[], int[]>> GetProblems(int amount, bool repeatedValues = true)
    {

        var generator = new ProblemGenerator(Seed);
        var randomGenerator = new Random(SeedForLimits);
        int maxRows = 100;
        int rows = randomGenerator.Next(1, maxRows);
        int maxColumns = 100;
        int columns = randomGenerator.Next(1, maxColumns);
        int minValueMatrix = -100;
        int maxValueMatrix = 100;
        int minFactorsSize = 0;
        int maxFactorSize = 100;
        int minValueFactors = -100;
        int maxValueFactors = 100;
        int minOperationsSize = 0;
        int maxOperationsSize = 100;
        int minValueOperations = -100;
        int maxValueOperations = 100;

        for (int i = 0; i < amount; i++)
        {
            problems.Add(generator.GetProblem(rows, columns, minValueMatrix, maxValueMatrix, repeatedValues, minFactorsSize, maxFactorSize, minValueFactors, maxValueFactors, minOperationsSize, maxOperationsSize, minValueOperations, maxValueOperations));
        }

        return CloneProblems(problems);


    }

    private List<Tuple<int[,], int[], int[]>> CloneProblems(List<Tuple<int[,], int[], int[]>> inProblems)
    {
        var result = new List<Tuple<int[,], int[], int[]>>();
        foreach (var problem in inProblems)
        {
            var matrix = new int[problem.Item1.GetLength(0), problem.Item1.GetLength(1)];
            Array.Copy(problem.Item1, matrix, problem.Item1.Length);
            var factors = new int[problem.Item2.Length];
            Array.Copy(problem.Item2, factors, problem.Item2.Length);
            var operations = new int[problem.Item3.Length];
            Array.Copy(problem.Item3, operations, problem.Item3.Length);
            result.Add(new Tuple<int[,], int[], int[]>(matrix, factors, operations));
        }
        return result;
    }

    public void ExportProblems(string path)
    {
        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Add("seed", Seed);
        data.Add("seedForLimits", SeedForLimits);
        data.Add("problems", problems);
        var json = JsonSerializer.Serialize(data);
        File.WriteAllText(path, json);
    }

    public static int Solve(int[,] matrix, int[] factors, int[] operations)
    {
        int[] center = new int[factors.Length];
        bool[,] mask = new bool[matrix.GetLength(0), matrix.GetLength(1)];

        for (int i = 0; i < factors.Length; i++)
            center[i] = factors.Length / 2;

        int sum = 0;
        for (int i = 0; i < operations.Length; i++)
        {
            for (int j = 0; j < factors.Length; j++)
            {
                center[j] = FindCenter(center[j], operations[i] * factors[j], matrix.GetLength(1));
                if (!mask[j, center[j]])
                    sum += matrix[j, center[j]];
                mask[j, center[j]] = true;
            }
        }
        return sum;
    }

    public static int FindCenter(int center, int rotation, int size)
    {
        int result = (center - rotation) % size;
        if (result < 0) return size + result;
        else return result;
    }

    public List<int> GetSolutions()
    {
        var results = new List<int>();
        foreach (var problem in CloneProblems(problems))
        {
            results.Add(Solve(problem.Item1, problem.Item2, problem.Item3));
        }
        return results;
    }
}
