using IATK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scatterplot : MonoBehaviour
{
    public CSVDataSource dataSource;
    public float pointSize;
    public int x, y, z;

    private GameObject[] points;

    public void Initialize(CSVDataSource dataSource, GameObject parent, float matrixPosX, float matrixPosY, float pointSize, int x, int y, int z)
    {
        this.dataSource = dataSource;
        this.pointSize = pointSize;
        this.x = x;
        this.y = y;
        this.z = z;

        float posOffset = 1;

        InitializeAxesLabel();
        CreateDataPoints();
        transform.Translate(new Vector3(matrixPosX + posOffset * matrixPosX, matrixPosY + posOffset * matrixPosY));
    }

    private void InitializeAxesLabel()
    {
        gameObject.GetComponentInChildrenWithTag<TextMesh>("X Axis Label").text = dataSource[x].Identifier;
        gameObject.GetComponentInChildrenWithTag<TextMesh>("Y Axis Label").text = dataSource[y].Identifier;
        gameObject.GetComponentInChildrenWithTag<TextMesh>("Z Axis Label").text = dataSource[z].Identifier;
    }

    private void CreateDataPoints()
    {
        points = new GameObject[dataSource.DataCount];   
        for (int i = 0; dataSource.DataCount > i; ++i)
        {
            Vector3 position = new Vector3(dataSource[x].Data[i], dataSource[y].Data[i], dataSource[z].Data[i]);
            GameObject point = Instantiate(Resources.Load<GameObject>("Prefabs/DataPoint"), transform);
            point.GetComponent<DataPoint>().Initialize(i, pointSize, position);
            points[i] = point;
        }
    }
}
