using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointOptions : MonoBehaviour
{
    public bool isScannable = false;

    private void OnDrawGizmos()
    {
        if(isScannable)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, 0.5f);
        }
    }
}
