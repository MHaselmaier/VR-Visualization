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

        CreateDataPoints();
        transform.Translate(new Vector3(matrixPosX + posOffset * matrixPosX, matrixPosY + posOffset * matrixPosY));
    }

    private void CreateDataPoints()
    {
        Vector3 boundingBoxMin = Vector3.positiveInfinity;
        Vector3 boundingBoxMax = Vector3.negativeInfinity;
        points = new GameObject[dataSource.DataCount];
        
        for (int i = 0; dataSource.DataCount > i; ++i)
        {
            Vector3 position = new Vector3(dataSource[x].Data[i], dataSource[y].Data[i], dataSource[z].Data[i]);
            GameObject point = Instantiate(Resources.Load<GameObject>("Prefabs/DataPoint"), transform);
            point.GetComponent<DataPoint>().Initialize(i, pointSize, position);
            points[i] = point;

            boundingBoxMin = Vector3.Min(boundingBoxMin, point.transform.position);
            boundingBoxMax = Vector3.Max(boundingBoxMax, point.transform.position);
        }

        ScaleDataPoints(boundingBoxMin, boundingBoxMax);
    }

    private void ScaleDataPoints(Vector3 boundingBoxMin, Vector3 boundingBoxMax)
    {
        Vector3 maxDistanceFromCenter = (boundingBoxMax - boundingBoxMin) / 2;
        Vector3 cloudCenter = boundingBoxMin + maxDistanceFromCenter;
        float scaleFactor = Mathf.Max(maxDistanceFromCenter.x, maxDistanceFromCenter.y, maxDistanceFromCenter.z) * 2;

        foreach (GameObject point in points)
        {
            point.transform.Translate(-cloudCenter);
            point.transform.position /= scaleFactor;
        }
    }

    public string GetData(int index)
    {
        string data = "";

        for (int dimension = 0; dataSource.DimensionCount > dimension; ++dimension)
        {
            data += dataSource[dimension].Identifier + ": " + dataSource.getOriginalValue(dataSource[dimension].Data[index], dataSource[dimension].Identifier) + ", ";
        }
        return  data + "Index: " + index;
    }
}
