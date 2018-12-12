using IATK;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
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
            Task.Run(() =>
            {
                CSVDataSource dataSource = new CSVDataSource();
                dataSource.load(File.ReadAllText(filePath), null);

                UnityMainThreadDispatcher.Instance().Enqueue(() =>
                {
                    Instantiate(Resources.Load<GameObject>("Prefabs/ScatterplotMatrix"), transform)
                        .GetComponent<ScatterplotMatrix>()
                        .Initialize(dataSource, pointSize);
                });
            });
        }
    }
}
