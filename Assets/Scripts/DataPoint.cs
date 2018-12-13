using IATK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataPoint : MonoBehaviour
{
    public int index = -1;

    private Scatterplot scatterplot;

    public void Initialize(int index, float pointSize, Vector3 position)
    {
        this.index = index;
        transform.localScale = Vector3.one * pointSize;
        transform.position = position;

        scatterplot = transform.parent.GetComponent<Scatterplot>();
    }
    
    public string[,] GetData()
    {
        CSVDataSource dataSource = scatterplot.dataSource;
        int xDim = scatterplot.xDim;
        int yDim = scatterplot.yDim;
        int zDim = scatterplot.zDim;

        string[,] data = new string[3, 2];

        data[0, 0] = dataSource[xDim].Identifier;
        data[0, 1] = dataSource.getOriginalValue(dataSource[xDim].Data[index], dataSource[xDim].Identifier).ToString();

        data[1, 0] = dataSource[yDim].Identifier;
        data[1, 1] = dataSource.getOriginalValue(dataSource[yDim].Data[index], dataSource[yDim].Identifier).ToString();

        data[2, 0] = dataSource[zDim].Identifier;
        data[2, 1] = dataSource.getOriginalValue(dataSource[zDim].Data[index], dataSource[zDim].Identifier).ToString();

        return data;
    }

    public void Select()
    {
        ScatterplotMatrix scatterplotMatrix = scatterplot.GetComponentInParent<ScatterplotMatrix>();
        scatterplotMatrix.SelectDataPoint(index);
    }
}
