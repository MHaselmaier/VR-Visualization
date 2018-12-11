using IATK;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualizerController : MonoBehaviour
{
    public float pointSize = 0.03f;

    private CSVDataSource dataSource;
    private ScatterplotMatrix scatterplotMatrix;

    void Awake(){}

    public void LoadData(string filePath){
        
        if(File.Exists(filePath)){
            dataSource = gameObject.AddComponent<CSVDataSource>();
            dataSource.load(File.ReadAllText(filePath), null);
            scatterplotMatrix = gameObject.AddComponent<ScatterplotMatrix>();
            scatterplotMatrix.initialize(dataSource, pointSize);
        }
    }
}
