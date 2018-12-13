using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class ControllerPointerEventListener : MonoBehaviour
{
    public bool showHoverState = false;

    private void Start()
    {
        if (null == GetComponent<VRTK_DestinationMarker>())
        {
            VRTK_Logger.Error(VRTK_Logger.GetCommonMessage(VRTK_Logger.CommonMessageKeys.REQUIRED_COMPONENT_MISSING_FROM_GAMEOBJECT, "ControllerPointerEventListener", "VRTK_DestinationMarker", "the Controller Alias"));
            return;
        }

        //Setup controller event listeners
        GetComponent<VRTK_DestinationMarker>().DestinationMarkerEnter += new DestinationMarkerEventHandler(DoPointerIn);
        if (showHoverState)
        {
            GetComponent<VRTK_DestinationMarker>().DestinationMarkerHover += new DestinationMarkerEventHandler(DoPointerHover);
        }
        GetComponent<VRTK_DestinationMarker>().DestinationMarkerExit += new DestinationMarkerEventHandler(DoPointerOut);
        GetComponent<VRTK_DestinationMarker>().DestinationMarkerSet += new DestinationMarkerEventHandler(DoPointerDestinationSet);
    }

    private void DoPointerIn(object sender, DestinationMarkerEventArgs e)
    {
        switch (e.target.tag)
        {
        case "Scatterplot":
            HandleScatterplotDoPointerIn(e.target.GetComponent<Scatterplot>());
            break;
        case "DataPoint":
            HandleDataPointDoPointerIn(e.target.GetComponent<DataPoint>());
            break;
        }
    }

    private void HandleScatterplotDoPointerIn(Scatterplot scatterplot)
    {
        Transform scatterplotMatrix = scatterplot.transform.parent;
        ResetPreviouslyActiveScatterplotColliders(scatterplotMatrix);

        scatterplot.GetComponent<BoxCollider>().enabled = false;
        foreach (Transform dataPoint in scatterplot.transform)
        {
            SphereCollider collider = dataPoint.GetComponent<SphereCollider>();
            if (null != collider)
            {
                dataPoint.GetComponent<Renderer>().material.color = Color.red;
                collider.enabled = true;
            }
        }
    }

    private void ResetPreviouslyActiveScatterplotColliders(Transform scatterplotMatrix)
    {
        foreach (Transform scatterplot in scatterplotMatrix)
        {
            if (!scatterplot.GetComponent<BoxCollider>().enabled)
            {
                scatterplot.GetComponent<BoxCollider>().enabled = true;
                foreach (Transform dataPoint in scatterplot)
                {
                    SphereCollider collider = dataPoint.GetComponent<SphereCollider>();
                    if (null != collider)
                    {
                        dataPoint.GetComponent<Renderer>().material.color = Color.white;
                        collider.enabled = false;
                    }
                }
                break;
            }
        }
    }

    private void HandleDataPointDoPointerIn(DataPoint dataPoint)
    {
        string[,] data = dataPoint.GetData();
        print(data[0, 0] + ": " + data[0, 1]);
        print(data[1, 0] + ": " + data[1, 1]);
        print(data[2, 0] + ": " + data[2, 1]);

        var go = Instantiate(Resources.Load("Prefabs/AttributeDialog"), dataPoint.gameObject.transform.position, Quaternion.identity) as GameObject;
        var canvas = dataPoint.gameObject.AddComponent<Canvas>();
        go.transform.SetParent(canvas.transform, false);
    }

    private void DoPointerOut(object sender, DestinationMarkerEventArgs e)
    {
        switch (e.target.tag)
        {
        case "DataPoint":
            HandleDataPointDoPointerOut(e.target.GetComponent<DataPoint>());
            break;
        }
    }

    private void HandleDataPointDoPointerOut(DataPoint dataPoint)
    {

    }

    private void DoPointerHover(object sender, DestinationMarkerEventArgs e)
    {
    }

    private void DoPointerDestinationSet(object sender, DestinationMarkerEventArgs e)
    {
    }
}
