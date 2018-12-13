using IATK;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScatterplotMatrix : MonoBehaviour
{
    public CSVDataSource dataSource;
    public float pointSize;

    public void Initialize(CSVDataSource dataSource, float pointSize)
    {
        this.dataSource = dataSource;
        this.pointSize = pointSize;

        CreateScatterplots();
    }

    private void CreateScatterplots()
    {
        GameObject scatterplotPrefab = Resources.Load("Prefabs/Scatterplot") as GameObject;
        int[,] dimCombinations = CalculateDimensionCombinations();
        int matrixWidth = (int)(Mathf.Sqrt(dimCombinations.GetLength(0)) + 1);
        
        StartCoroutine(CreateScatterplotsCoroutine(scatterplotPrefab, dimCombinations, matrixWidth));
    }

    IEnumerator CreateScatterplotsCoroutine(GameObject scatterplotPrefab, int[,] dimCombinations, int matrixWidth)
    {
        for (int i = 0; dimCombinations.GetLength(0) > i; ++i)
        {
            int matrixPosX = i % matrixWidth;
            int matrixPosY = i / matrixWidth;
            int xDim = dimCombinations[i, 0];
            int yDim = dimCombinations[i, 1];
            int zDim = dimCombinations[i, 2];

            Instantiate(scatterplotPrefab, transform)
                .GetComponent<Scatterplot>()
                .Initialize(dataSource, matrixPosX, matrixPosY, pointSize, xDim, yDim, zDim);
            yield return null;
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
