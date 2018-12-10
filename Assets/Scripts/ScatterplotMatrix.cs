using IATK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScatterplotMatrix : MonoBehaviour
{
    private CSVDataSource dataSource;
    private float pointSize;

    public void initialize(CSVDataSource dataSource, float pointSize)
    {
        this.dataSource = dataSource;
        this.pointSize = pointSize;
        createScatterplots();
    }

    public void createScatterplots()
    {
        gameObject.AddComponent<Scatterplot>().initialize(dataSource, pointSize, 0, 1, 2);
        gameObject.AddComponent<Scatterplot>().initialize(dataSource, pointSize, 0, 1, 3);
        gameObject.AddComponent<Scatterplot>().initialize(dataSource, pointSize, 0, 2, 3);
        gameObject.AddComponent<Scatterplot>().initialize(dataSource, pointSize, 1, 2, 3);
    }

	// Update is called once per frame
	void Update ()
    {
		
	}
}
