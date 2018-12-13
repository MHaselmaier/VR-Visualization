using IATK;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visualizer : MonoBehaviour
{
    public float pointSize = 0.03f;

    public void LoadData(string filePath)
    {    
        if (File.Exists(filePath))
        {
            CSVDataSource dataSource = new CSVDataSource();
            dataSource.load(File.ReadAllText(filePath), null);
                
            Instantiate(Resources.Load<GameObject>("Prefabs/ScatterplotMatrix"), transform)
                .GetComponent<ScatterplotMatrix>()
                .Initialize(dataSource, pointSize);
        }
    }
}
