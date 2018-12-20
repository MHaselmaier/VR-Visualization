using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private GameObject headNode;

    void Start()
    {
        headNode = GameObject.Find("HeadNode");
    }

    void Update()
    {
        if (gameObject.activeSelf)
        {
            transform.rotation = Quaternion.LookRotation(transform.position - headNode.transform.position);
        }
    }
}
