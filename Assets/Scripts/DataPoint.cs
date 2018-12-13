using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataPoint : MonoBehaviour
{
    public int index = -1;

    public void Initialize(int index, float pointSize, Vector3 position)
    {
        this.index = index;
        transform.localScale = Vector3.one * pointSize;
        transform.position = position;
    }

    public Scatterplot GetScatterplot()
    {
        return transform.parent.GetComponent<Scatterplot>();
    }
}
