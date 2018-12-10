using IATK;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScatterplotMatrix : MonoBehaviour
{
    private CSVDataSource dataSource;
    private float pointSize;
    private GameObject scatterplotMatrix;

    public void Initialize(CSVDataSource dataSource, float pointSize)
    {
        this.dataSource = dataSource;
        this.pointSize = pointSize;

        scatterplotMatrix = new GameObject("Scatterplotmatrix");
        scatterplotMatrix.transform.parent = gameObject.transform;


        CreateScatterplots();
    }

    public void CreateScatterplots()
    {
        int[,] dimensionCombinations = CalculateDimensionCombinations();
        int matrixWidth = (int)(Mathf.Sqrt(dimensionCombinations.GetLength(0)) + 1);
        for (int i = 0; dimensionCombinations.GetLength(0) > i; ++i)
        {
            int matrixPosX = i % matrixWidth;
            int matrixPosY = i / matrixWidth;
            int x = dimensionCombinations[i, 0];
            int y = dimensionCombinations[i, 1];
            int z = dimensionCombinations[i, 2];
            gameObject.AddComponent<Scatterplot>().Initialize(dataSource, scatterplotMatrix, matrixPosX, matrixPosY, pointSize, x, y, z);
        }
    }

    private int[,] CalculateDimensionCombinations()
    {
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
