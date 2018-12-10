using IATK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScatterplotMatrix : MonoBehaviour
{
    private CSVDataSource dataSource;
    private float pointSize;
    private GameObject scatterplotMatrix;

    public void Initialize(CSVDataSource dataSource, float pointSize)
    {
        this.dataSource = dataSource;
        this.pointSize = pointSize;

        scatterplotMatrix = new GameObject("Scatterplotmatrix");
        scatterplotMatrix.transform.parent = gameObject.transform;


        createScatterplots();
    }

    public void createScatterplots()
    {
        gameObject.AddComponent<Scatterplot>().Initialize(dataSource, scatterplotMatrix, pointSize, 0, 1, 2);
        gameObject.AddComponent<Scatterplot>().Initialize(dataSource, scatterplotMatrix, pointSize, 0, 1, 3);
        gameObject.AddComponent<Scatterplot>().Initialize(dataSource, scatterplotMatrix, pointSize, 0, 2, 3);
        gameObject.AddComponent<Scatterplot>().Initialize(dataSource, scatterplotMatrix, pointSize, 1, 2, 3);
    }

	// Update is called once per frame
	void Update ()
    {
		
	}
}
