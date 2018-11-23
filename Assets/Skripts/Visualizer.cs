using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visualizer : MonoBehaviour
{
    public TextAsset file;
    public float pointSize = 0.025f;

    float[,] data = new float[10000, 3];
    GameObject[] points;

    // Use this for initialization
    void Start()
    {
        string[] lines = file.text.Split('\n');
        float[,] data = new float[lines.GetLength(0) - 1, 3];
        for (int i = 1; lines.GetLength(0) > i; ++i)
        {
            string[] values = lines[i].Split(',');
            for (int j = 1; 3 >= j; ++j)
            {
                float f = float.Parse(values[j]);
                data[i - 1, j - 1] = f;
            }
        }
        Vector3 min = Vector3.positiveInfinity;
        Vector3 max = Vector3.negativeInfinity;
        GameObject[] points = new GameObject[data.GetLength(0)];
        for (int i = 0; data.GetLength(0) > i; ++i)
        {
            GameObject point = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            point.name = "DataPoint";
            point.transform.parent = this.transform;
            point.transform.localScale = Vector3.one * pointSize;
            point.transform.position = new Vector3(data[i, 0], data[i, 1], data[i, 2]);
            point.GetComponent<Renderer>().material.color = Color.red;
            points[i] = point;

            min = Vector3.Min(min, point.transform.position);
            max = Vector3.Max(max, point.transform.position);
        }

        Vector3 cloudCenter = min + (max - min) / 2;

        Vector3 size = (max - min) / 2;
        float scaleFactor = Mathf.Max(size.x, size.y, size.z);
        foreach (GameObject point in points)
        {
            point.transform.Translate(-cloudCenter);
            point.transform.position = point.transform.position / scaleFactor;
        }
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(Vector3.down * Time.deltaTime * 10);
    }
}
