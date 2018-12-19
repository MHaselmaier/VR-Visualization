using IATK;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScatterplotMatrix : MonoBehaviour
{
    public CSVDataSource dataSource;
    public float pointSize;

    private Scatterplot[] scatterplots;

    public void Initialize(CSVDataSource dataSource, int[,] dimCombinations, float pointSize)
    {
        this.dataSource = dataSource;
        this.pointSize = pointSize;

        CreateScatterplots(dimCombinations);
    }

    private void CreateScatterplots(int[,] dimCombinations)
    {
        GameObject scatterplotPrefab = Resources.Load("Prefabs/Scatterplot") as GameObject;
        int matrixWidth = (int)(Mathf.Sqrt(dimCombinations.GetLength(0)) + 1);

        scatterplots = new Scatterplot[dimCombinations.GetLength(0)];
        StartCoroutine(CreateScatterplotsCoroutine(scatterplotPrefab, dimCombinations, matrixWidth));
    }

    IEnumerator CreateScatterplotsCoroutine(GameObject scatterplotPrefab, int[,] dimCombinations, int matrixWidth)
    {
        for (int i = 0; dimCombinations.GetLength(0) > i; ++i)
        {
            int matrixPosX = i % matrixWidth * 2;
            int matrixPosZ = i / matrixWidth * 2;
            int xDim = dimCombinations[i, 0];
            int yDim = dimCombinations[i, 1];
            int zDim = dimCombinations[i, 2];

            Scatterplot scatterplot = Instantiate(scatterplotPrefab, transform).GetComponent<Scatterplot>();
            scatterplot.Initialize(dataSource, matrixPosX, matrixPosZ, pointSize, xDim, yDim, zDim);
            scatterplots[i] = scatterplot;

            scatterplot.transform.localScale += Vector3.one;

            yield return null;
        }
    }

    public void SelectDataPoint(int index)
    {
        foreach (Scatterplot scatterplot in scatterplots)
        {
            scatterplot.SelectDataPoint(index);
        }
    }
}
