using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class ControllerPointerEventListener : MonoBehaviour
{
    public bool showHoverState = false;

    private void Start()
    {
        if (GetComponent<VRTK_DestinationMarker>() == null)
        {
            VRTK_Logger.Error(VRTK_Logger.GetCommonMessage(VRTK_Logger.CommonMessageKeys.REQUIRED_COMPONENT_MISSING_FROM_GAMEOBJECT, "VRTK_ControllerPointerEvents_ListenerExample", "VRTK_DestinationMarker", "the Controller Alias"));
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
        print("IN: " + e.target.name);
    }

    private void DoPointerOut(object sender, DestinationMarkerEventArgs e)
    {
        print("OUT: " + e.target.name);
    }

    private void DoPointerHover(object sender, DestinationMarkerEventArgs e)
    {
        print("HOVER: " + e.target.name);
    }

    private void DoPointerDestinationSet(object sender, DestinationMarkerEventArgs e)
    {
        print("DESTINATION: " + e.target.name);
    }
}
