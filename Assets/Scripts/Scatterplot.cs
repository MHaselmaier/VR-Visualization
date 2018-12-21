using IATK;
using UnityEngine;

/// <summary>
/// Capsules and creates DataPoints.
/// </summary>
public class Scatterplot : MonoBehaviour
{
    public CSVDataSource dataSource;

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
            foreach (DataPoint dataPoint in dataPoints)
            {
                dataPoint.pointSize = value;
            }
        }
    }

    /// <summary>
    /// The three indeces of the columns in the CSV-File.
    /// </summary>
    public int xDim, yDim, zDim;

    private DataPoint[] dataPoints = new DataPoint[0];

    /// <summary>
    /// Initializes the ScatterplotMatrix.
    /// Should always be called after creating this component.
    /// </summary>
    /// <param name="dataSource"></param>
    /// <param name="matrixPosX"></param>
    /// <param name="matrixPosZ"></param>
    /// <param name="pointSize"></param>
    /// <param name="xDim"></param>
    /// <param name="yDim"></param>
    /// <param name="zDim"></param>
    public void Initialize(CSVDataSource dataSource, float matrixPosX, float matrixPosZ, float pointSize, int xDim, int yDim, int zDim)
    {
        this.dataSource = dataSource;
        this.pointSize = pointSize;
        this.xDim = xDim;
        this.yDim = yDim;
        this.zDim = zDim;

        float posOffset = 1;

        InitializeAxesLabel();
        CreateDataPoints();
        transform.Translate(new Vector3(matrixPosX + posOffset * matrixPosX, 0, matrixPosZ + posOffset * matrixPosZ));
    }

    /// <summary>
    /// Sets the text of the Axes to the identifiers of the columns of the CSV-File.
    /// </summary>
    private void InitializeAxesLabel()
    {
        Transform axes = transform.Find("Axes");
        axes.Find("X Axis").GetComponentInChildren<TextMesh>().text = dataSource[xDim].Identifier;
        axes.Find("Y Axis").GetComponentInChildren<TextMesh>().text = dataSource[yDim].Identifier;
        axes.Find("Z Axis").GetComponentInChildren<TextMesh>().text = dataSource[zDim].Identifier;
    }

    private void CreateDataPoints()
    {
        GameObject pointPrefab = Resources.Load<GameObject>("Prefabs/DataPoint");

        dataPoints = new DataPoint[dataSource.DataCount];
        for (int i = 0; dataSource.DataCount > i; ++i)
        {
            Vector3 position = new Vector3(dataSource[xDim].Data[i], dataSource[yDim].Data[i], dataSource[zDim].Data[i]);
            DataPoint dataPoint = Instantiate(pointPrefab, transform).GetComponent<DataPoint>();
            dataPoint.Initialize(i, pointSize, position);
            dataPoints[i] = dataPoint;
        }
    }

    /// <summary>
    /// Iterates through all DataPoints and highligths the selected one.
    /// Also shows his attributes in a dialog.
    /// This Method is called from ScatterplotMatrix.
    /// </summary>
    /// <param name="index"></param>
    public void SelectDataPoint(int index)
    {
        foreach (DataPoint dataPoint in dataPoints)
        {
            if (dataPoint.index == index)
            {
                dataPoint.GetComponent<Renderer>().material.color = Color.red;
                dataPoint.ShowText(true);
                dataPoint.pointSize = pointSize + 0.01f;
            }
            else
            {
                dataPoint.GetComponent<Renderer>().material.color = Color.white;
                dataPoint.ShowText(false);
                dataPoint.pointSize = pointSize;
            }
        }
    }

    /// <summary>
    /// This gets call if the Wand enters a Scatterplot.
    /// Normally the colliders of the DataPoints are disabled.
    /// This is done because Unity can only handle so many colliders
    /// at once. If the user imports to many DataPoints, Unity 
    /// throws an error. Because of this the colliders of the DataPoints
    /// are only enabled if the Wand enters their Scatterplot. The
    /// collider of this Scatterplot gets disabled so the Wand can 
    /// interact with the DataPoints. Once another Scatterplot is entered
    /// by the Wand the collider of the previous entered Scatterplot is
    /// enabled again and the colliders of his included DataPoints are 
    /// disabled. This reduces the amount of active colliders at a time
    /// dramatically.
    /// </summary>
    /// <param name="iSelection"></param>
    protected void OnMVRWandEnter(VRSelection iSelection)
    {
        foreach (Transform scatterplot in transform.parent)
        {
            Collider collider = scatterplot.GetComponent<BoxCollider>();
            bool previouslySelected = !collider.enabled;

            collider.enabled = previouslySelected;
            foreach (Transform dataPoint in scatterplot)
            {
                collider = dataPoint.GetComponent<SphereCollider>();
                if (null != collider)
                {
                    collider.enabled = !previouslySelected;
                }
            }
        }
    }
}
