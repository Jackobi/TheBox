using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_debugSphere : MonoBehaviour
{
    public Color colour = Color.black;
    public float radius;
    private void OnDrawGizmos()
    {
        Gizmos.color = colour;
        Gizmos.DrawSphere(transform.position, radius);
    }

}
