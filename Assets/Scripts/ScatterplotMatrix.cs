using IATK;
using System.Collections;
using UnityEngine;

/// <summary>
/// Capsules and creates Scatterplots.
/// </summary>
public class ScatterplotMatrix : MonoBehaviour
{
    /// <summary>
    /// The size of the DataPoints.
    /// </summary>
    private float _pointSize;
    public float pointSize
    {
        get { return _pointSize; }
        set
        {
            _pointSize = value;
            foreach (Scatterplot scatterplot in scatterplots)
            {
                scatterplot.pointSize = value;
            }
        }
    }

    private Scatterplot[] scatterplots = new Scatterplot[0];

    /// <summary>
    /// Initializes the ScatterplotMatrix.
    /// Should always be called after creating this component.
    /// </summary>
    /// /// <param name="dataSource"></param>
    /// /// <param name="dimCombinations"></param>
    /// /// <param name="pointSize"></param>
    public void Initialize(CSVDataSource dataSource, int[,] dimCombinations, float pointSize)
    {
        this.pointSize = pointSize;

        CreateScatterplots(dataSource, dimCombinations);
    }

    /// <summary>
    /// Calls the subroutine which creates the Scatterplots.
    /// </summary>
    /// <param name="dimCombinations"></param>
    /// <param name="dimCombinations"></param>
    private void CreateScatterplots(CSVDataSource dataSource, int[,] dimCombinations)
    {
        GameObject scatterplotPrefab = Resources.Load("Prefabs/Scatterplot") as GameObject;
        int matrixWidth = (int)(Mathf.Sqrt(dimCombinations.GetLength(0)) + 1);

        scatterplots = new Scatterplot[dimCombinations.GetLength(0)];
        StartCoroutine(CreateScatterplotsCoroutine(scatterplotPrefab, dataSource, dimCombinations, matrixWidth));
    }

    /// <summary>
    /// Creates the Scatterplots in a subroutine, one per frame.
    /// This was done to keep the programm from freezen when creating 
    /// the Scatterplots.
    /// </summary>
    /// <param name="scatterplotPrefab"></param>
    /// /// <param name="dataSource"></param>
    /// <param name="dimCombinations"></param>
    /// <param name="matrixWidth"></param>
    /// <returns></returns>
    IEnumerator CreateScatterplotsCoroutine(GameObject scatterplotPrefab, CSVDataSource dataSource, int[,] dimCombinations, int matrixWidth)
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

    /// <summary>
    /// Delegates the call to all the Scatterplots.
    /// This Method is called from DataPoint when it is selected by the user.
    /// </summary>
    /// <param name="index"></param>
    public void SelectDataPoint(int index)
    {
        foreach (Scatterplot scatterplot in scatterplots)
        {
            scatterplot.SelectDataPoint(index);
        }
    }
}
