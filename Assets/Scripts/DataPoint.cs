using IATK;
using System;
using UnityEngine;

/// <summary>
/// Represents one DataPoint as a sphere.
/// </summary>
public class DataPoint : MonoBehaviour
{
    /// <summary>
    /// The index of this DataPoint in the CSV-File.
    /// </summary>
    private int index;

    /// <summary>
    /// The size of the DataPoints.
    /// </summary>
    private float _pointSize;
    public float pointSize
    {
        get { return _pointSize; }
        set
        {
            _pointSize = value;
            transform.localScale = Vector3.one * value;
        }
    }

    private Scatterplot scatterplot;
    
    /// <summary>
    /// The dialog which holds the attribute values of this DataPoint.
    /// Will be shown when the DataPoint is selected.
    /// </summary>
    private GameObject attributes;

    /// <summary>
    /// Initializes the DataPoint.
    /// Should always be called after creating this component.
    /// </summary>
    /// <param name="index"></param>
    /// <param name="pointSize"></param>
    /// <param name="position"></param>
    public void Initialize(int index, float pointSize, Vector3 position)
    {
        this.index = index;
        this.pointSize = pointSize;
        transform.position = position;

        scatterplot = transform.parent.GetComponent<Scatterplot>();
        attributes = transform.Find("Attributes").gameObject;

        initTextMeshes();
        ShowText(false);
    }

    /// <summary>
    /// Sets the text inside the attribute dialog and
    /// fits the backgound to the width of the text.
    /// </summary>
    private void initTextMeshes()
    {
        var data = GetData();

        TextMesh attribute1 = attributes.transform.Find("attribute1").GetComponent<TextMesh>();
        TextMesh attribute2 = attributes.transform.Find("attribute2").GetComponent<TextMesh>();
        TextMesh attribute3 = attributes.transform.Find("attribute3").GetComponent<TextMesh>();

        attribute1.text = String.Format("{0}: {1}", data[0,0], data[0,1]);
        attribute2.text = String.Format("{0}: {1}", data[1,0], data[1,1]);
        attribute3.text = String.Format("{0}: {1}", data[2,0], data[2,1]);
        
        Transform background = attributes.transform.Find("Background");
        Vector3 newScale = background.localScale;
        newScale.x = Mathf.Max(GetTextMeshWidth(attribute1), GetTextMeshWidth(attribute2), GetTextMeshWidth(attribute3));
        background.localScale = newScale;

        Vector3 newPosition = background.localPosition;
        newPosition.x = attribute3.transform.localPosition.x + background.localScale.x / 2f;
        background.localPosition = newPosition;
    }

    /// <summary>
    /// Utility method to get the width of a TextMesh.
    /// </summary>
    /// <param name="mesh"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Set wether the attribute dialog is shown or not.
    /// </summary>
    /// <param name="show"></param>
    public void ShowText(bool show)
    {
        attributes.SetActive(show);
    }
    
    /// <summary>
    /// Returns the data from the CSV-File which this DataPoint represents.
    /// The first dimension holds the three columns.
    /// The second dimension holds the columns identifier in index 0 and the value in index 1.
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// This is called when the user selects the DataPoint.
    /// The call is delegated to the ScatterplotMatrix so that it
    /// can be forwarded to all the Scatterplots.
    /// This way if a DataPoint with a given index is selected, the
    /// same DataPoint in other Scatterplots are highlighted as well.
    /// </summary>
    /// <param name="iSelection"></param>
    protected void VRAction(VRSelection iSelection)
    {
        ScatterplotMatrix scatterplotMatrix = scatterplot.GetComponentInParent<ScatterplotMatrix>();
        scatterplotMatrix.SelectDataPoint(index);
    }
}
