using IATK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scatterplot : MonoBehaviour
{
    public CSVDataSource dataSource;
    public float pointSize;
    public int xDim, yDim, zDim;

    private DataPoint[] dataPoints;

    public void Initialize(CSVDataSource dataSource, float matrixPosX, float matrixPosY, float pointSize, int xDim, int yDim, int zDim)
    {
        this.dataSource = dataSource;
        this.pointSize = pointSize;
        this.xDim = xDim;
        this.yDim = yDim;
        this.zDim = zDim;

        float posOffset = 1;

        InitializeAxesLabel();
        CreateDataPoints();
        transform.Translate(new Vector3(matrixPosX + posOffset * matrixPosX, matrixPosY + posOffset * matrixPosY));
    }

    private void InitializeAxesLabel()
    {
        gameObject.GetComponentInChildrenWithTag<TextMesh>("X Axis Label").text = dataSource[xDim].Identifier;
        gameObject.GetComponentInChildrenWithTag<TextMesh>("Y Axis Label").text = dataSource[yDim].Identifier;
        gameObject.GetComponentInChildrenWithTag<TextMesh>("Z Axis Label").text = dataSource[zDim].Identifier;
    }

    private void CreateDataPoints()
    {
        GameObject pointPrefab = Resources.Load<GameObject>("Prefabs/DataPoint");

        dataPoints = new DataPoint[dataSource.DataCount];
        for (int i = 0; dataSource.DataCount > i; ++i)
        {
            Vector3 position = new Vector3(dataSource[xDim].Data[i], dataSource[yDim].Data[i], dataSource[zDim].Data[i]);
            DataPoint dataPoint = Instantiate(pointPrefab, transform).GetComponent<DataPoint>();
            dataPoint.Initialize(i, pointSize, position);
            dataPoints[i] = dataPoint;
        }
    }

    public void SelectDataPoint(int index)
    {
        foreach (DataPoint dataPoint in dataPoints)
        {
            if (dataPoint.index == index)
            {
                dataPoint.GetComponent<Renderer>().material.color = Color.red;
            }
            else
            {
                dataPoint.GetComponent<Renderer>().material.color = Color.white;
            }
        }
    }
}
