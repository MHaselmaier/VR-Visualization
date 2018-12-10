using IATK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visualizer : MonoBehaviour
{
    public TextAsset dataFile;

    public float pointSize = 0.03f;

    private CSVDataSource dataSource;
    private ScatterplotMatrix scatterplotMatrix;

    void Awake()
    {
        dataSource = gameObject.AddComponent<CSVDataSource>();
        dataSource.load(dataFile.text, null);
        scatterplotMatrix = gameObject.AddComponent<ScatterplotMatrix>();
        scatterplotMatrix.Initialize(dataSource, pointSize);
    }
}
