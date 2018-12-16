using IATK;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataPoint : MonoBehaviour
{
    public int index = -1;

    private Scatterplot scatterplot;
    private GameObject attributes;

    public void Initialize(int index, float pointSize, Vector3 position)
    {
        this.index = index;
        transform.localScale = Vector3.one * pointSize;
        transform.position = position;

        scatterplot = transform.parent.GetComponent<Scatterplot>();
        attributes = transform.Find("Attributes").gameObject;

        initTextMeshes();
        ShowText(false);
    }

    private void initTextMeshes()
    {
        var data = GetData();

        TextMesh attribute1 = attributes.GetComponentInChildrenWithTag<TextMesh>("Attribute1");
        TextMesh attribute2 = attributes.GetComponentInChildrenWithTag<TextMesh>("Attribute2");
        TextMesh attribute3 = attributes.GetComponentInChildrenWithTag<TextMesh>("Attribute3");

        attribute1.text = String.Format("{0}: {1}", data[0,0], data[0,1]);
        attribute2.text = String.Format("{0}: {1}", data[1,0], data[2,1]);
        attribute3.text = String.Format("{0}: {1}", data[2,0], data[2,1]);
        
        Transform background = attributes.transform.Find("Background");
        Vector3 newScale = background.localScale;
        newScale.x = Mathf.Max(GetTextMeshWidth(attribute1), GetTextMeshWidth(attribute2), GetTextMeshWidth(attribute3));
        background.localScale = newScale;

        Vector3 newPosition = background.localPosition;
        newPosition.x = attribute3.transform.localPosition.x + background.localScale.x / 2f;
        background.localPosition = newPosition;
    }

    private float GetTextMeshWidth(TextMesh mesh)
    {
        // from http://answers.unity.com/comments/1072098/view.html
        float width = 0;
        foreach (char symbol in mesh.text)
        {
            CharacterInfo info;
            if (mesh.font.GetCharacterInfo(symbol, out info, mesh.fontSize, mesh.fontStyle))
            {
                width += info.advance;
            }
        }
        return width * mesh.characterSize * 0.1f * mesh.transform.localScale.x;
    }

    public void ShowText(bool show)
    {
        attributes.SetActive(show);
        /*gameObject.GetComponentInChildrenWithTag<TextMesh>("Attribute1").gameObject.SetActive(show);
        gameObject.GetComponentInChildrenWithTag<TextMesh>("Attribute2").gameObject.SetActive(show);
        gameObject.GetComponentInChildrenWithTag<TextMesh>("Attribute3").gameObject.SetActive(show);*/
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

    protected void VRAction(VRSelection iSelection)
    {
        ScatterplotMatrix scatterplotMatrix = scatterplot.GetComponentInParent<ScatterplotMatrix>();
        scatterplotMatrix.SelectDataPoint(index);
    }
}
