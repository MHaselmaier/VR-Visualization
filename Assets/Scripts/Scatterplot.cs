using IATK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scatterplot : MonoBehaviour
{
    private GameObject axis;

    private CSVDataSource dataSource;
    private GameObject scatterplot;
    private GameObject[] points;

    private float pointSize;
    private int x, y, z;

    public void Awake()
    {
        axis = Resources.Load("Prefabs/Axis") as GameObject;
    }

    public void Initialize(CSVDataSource dataSource, GameObject parent, float pointSize, int x, int y, int z)
    {
        this.dataSource = dataSource;
        this.pointSize = pointSize;
        this.x = x;
        this.y = y;
        this.z = z;

        scatterplot = new GameObject("Scatterplot");
        scatterplot.transform.parent = parent.transform;

        CreateAxis();
        CreateDataPoints();
    }

    private void CreateAxis()
    {
        Vector3 position = Vector3.one * -0.5f;

        GameObject xAxis = Instantiate(axis, position, Quaternion.Euler(Vector3.back * 90), scatterplot.transform);
        xAxis.name = "X Axis";
        xAxis.GetComponentsInChildren<Renderer>().ForEach((renderer) =>
        {
            renderer.material.color = Color.red;
        });

        GameObject yAxis = Instantiate(axis, position, Quaternion.identity, scatterplot.transform);
        yAxis.name = "Y Axis";
        yAxis.GetComponentsInChildren<Renderer>().ForEach((renderer) =>
        {
            renderer.material.color = Color.green;
        });

        GameObject zAxis = Instantiate(axis, position, Quaternion.Euler(Vector3.right * 90), scatterplot.transform);
        zAxis.name = "Z Axis";
        zAxis.GetComponentsInChildren<Renderer>().ForEach((renderer) =>
        {
            renderer.material.color = Color.blue;
        });
    }

    private void CreateDataPoints()
    {
        Vector3 boundingBoxMin = Vector3.positiveInfinity;
        Vector3 boundingBoxMax = Vector3.negativeInfinity;
        points = new GameObject[dataSource.DataCount];
        for (int i = 0; dataSource.DataCount > i; ++i)
        {
            GameObject point = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            point.name = "DataPoint";
            point.transform.parent = scatterplot.transform;
            point.transform.localScale = Vector3.one * pointSize;
            point.transform.position = new Vector3(dataSource[x].Data[i], dataSource[y].Data[i], dataSource[z].Data[i]);
            point.GetComponent<Renderer>().material.color = Color.grey;
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

	// Update is called once per frame
	void Update()
    {
		
	}
}
