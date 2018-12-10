﻿using IATK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scatterplot : MonoBehaviour
{
    private CSVDataSource dataSource;
    private float pointSize;
    private int x, y, z;

    public void initialize(CSVDataSource dataSource, float pointSize, int x, int y, int z)
    {
        this.dataSource = dataSource;
        this.pointSize = pointSize;
        GameObject parent = new GameObject();
        parent.name = "Scatterplot";

        for (int i = 0; dataSource.DataCount > i; ++i)
        {
            GameObject point = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            point.name = "DataPoint";
            point.transform.parent = parent.transform;
            point.transform.localScale = Vector3.one * this.pointSize;
            point.transform.position = new Vector3(dataSource[x].Data[i], dataSource[y].Data[i], dataSource[z].Data[i]);
            point.GetComponent<Renderer>().material.color = Color.red;
        }
    }

	// Update is called once per frame
	void Update()
    {
		
	}
}
