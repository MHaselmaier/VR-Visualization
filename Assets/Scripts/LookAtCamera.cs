using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    void Update()
    {
        if (gameObject.activeSelf)
        {
            GameObject head = GameObject.Find("HeadNode");
            if (null != head)
            {
                Vector3 lookDir = transform.position - head.transform.position;
                transform.rotation = Quaternion.LookRotation(lookDir);
            }
            else
            {
                print(null);
            }
        }
    }
}
