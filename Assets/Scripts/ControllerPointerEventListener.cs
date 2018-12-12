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
        print("IN: " + e.target.tag);
        switch (e.target.tag)
        {
        case "Scatterplot":
            HandleScatterplotDoPointerIn(e.target);
            break;
        case "DataPoint":
            HandleDataPointDoPointerIn(e.target);
            break;
        }
    }

    private void HandleScatterplotDoPointerIn(Transform scatterplot)
    {
        Transform scatterplotMatrix = scatterplot.parent;
        ResetPreviouslyActiveScatterplotColliders(scatterplotMatrix);

        scatterplot.GetComponent<BoxCollider>().enabled = false;
        foreach (Transform dataPoint in scatterplot)
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

    private void HandleDataPointDoPointerIn(Transform dataPoint)
    {
        print("Selected DataPoint");
    }

    private void DoPointerOut(object sender, DestinationMarkerEventArgs e)
    {
        print("Out: " + e.target.tag);
        switch (e.target.tag)
        {
        case "DataPoint":
            HandleDataPointDoPointerOut(e.target);
            break;
        }
    }

    private void HandleDataPointDoPointerOut(Transform dataPoint)
    {
        print("Deselected DataPoint");
    }

    private void DoPointerHover(object sender, DestinationMarkerEventArgs e)
    {
        //print("HOVER: " + e.target.name);
    }

    private void DoPointerDestinationSet(object sender, DestinationMarkerEventArgs e)
    {
        //print("DESTINATION: " + e.target.name);
    }
}
