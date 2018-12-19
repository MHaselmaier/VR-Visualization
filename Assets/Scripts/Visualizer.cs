using IATK;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Visualizer : MonoBehaviour
{
    public float pointSize = 0.03f;

    private CSVDataSource dataSource;
    private int[,] possibleScatterplots;

    public void LoadDataSource(string filePath)
    {
        if (File.Exists(filePath))
        {
            dataSource = new CSVDataSource();
            dataSource.load(File.ReadAllText(filePath), null);
            possibleScatterplots = CalculatePossibleScatterplots();
            Debug.Log("Loaded CSV file from: " + filePath);
        }
        else
        {
            Debug.LogError("Filepath to CSV file doeas not exist: " + filePath);
        }
    }

    public string[] GetIdentifiers()
    {
        if (!dataSource.IsLoaded) return new string[0];

        string[] identifiers = new string[dataSource.DimensionCount];
        for (int i = 0; identifiers.GetLength(0) > i; ++i)
        {
            identifiers[i] = dataSource[i].Identifier;
        }
        return identifiers;
    }

    public void CreateScatterplotMatrix(int[] scatterplotIndices)
    {
        if (dataSource.IsLoaded)
        {
            foreach (Transform scatterplotMatrix in transform)
            {
                GameObject.Destroy(scatterplotMatrix.gameObject);
            }

            int[,] dimCombinations = new int[scatterplotIndices.GetLength(0), 3];
            for (int i = 0; dimCombinations.GetLength(0) > i; ++i)
            {
                for (int j = 0; 3 > j; ++j)
                {
                    dimCombinations[i, j] = possibleScatterplots[scatterplotIndices[i], j];
                }
            }

            Instantiate(Resources.Load<GameObject>("Prefabs/ScatterplotMatrix"), transform)
                .GetComponent<ScatterplotMatrix>()
                .Initialize(dataSource, dimCombinations, pointSize);
            Debug.Log("ScatterplotMatrix was created.");
        }
        else
        {
            Debug.LogError("CSVDataSource was not loaded!");
        }
    }

    public string[] GetPossibleScattersplots()
    {
        string[] possibilities = new string[possibleScatterplots.GetLength(0)];
        for (int i = 0; possibleScatterplots.GetLength(0) > i; ++i)
        {
            possibilities[i] = dataSource[possibleScatterplots[i, 0]].Identifier + " - "
                + dataSource[possibleScatterplots[i, 1]].Identifier + " - "
                + dataSource[possibleScatterplots[i, 2]].Identifier;
        }
        return possibilities;
    }

    private int[,] CalculatePossibleScatterplots()
    {
        if (!dataSource.IsLoaded) return new int[0, 0];

        int[] indices = new int[dataSource.DimensionCount];
        for (int i = 0; dataSource.DimensionCount > i; ++i)
        {
            indices[i] = i;
        }

        IEnumerable<int>[] combinations = indices.Combinations(3).ToArray();
        int[,] result = new int[combinations.GetLength(0), 3];
        for (int i = 0; combinations.GetLength(0) > i; ++i)
        {
            for (int j = 0; 3 > j; ++j)
            {
                result[i, j] = combinations[i].ToArray()[j];
            }
        }

        return result;
    }
}

static class Extension
{
    // Copied from https://stackoverflow.com/a/1898744
    public static IEnumerable<IEnumerable<T>> Combinations<T>(this IEnumerable<T> elements, int k)
    {
        return k == 0 ? new[] { new T[0] } :
            elements.SelectMany((e, i) =>
                elements.Skip(i + 1).Combinations(k - 1).Select(c => (new[] { e }).Concat(c)));
    }
}
