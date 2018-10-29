using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_mouseClick : MonoBehaviour
{
    private Ray ray;
    [HideInInspector]public RaycastHit hitInfo;
    private GameObject cameraRef;

    private void Update()
    {
        if(Input.GetAxis("Fire1") > 0)
        {
            if (CastRay())
            {
                print(hitInfo.collider.gameObject);
                gameObject.GetComponent<UnityStandardAssets.Cameras.Scr_playerCam>().ChangeBoomLength(hitInfo.transform.GetComponent<Scr_focusPoint>().boomLength);
            }
        }
    }

    private bool CastRay()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        bool hit = Physics.Raycast(ray, out hitInfo);
        /*
        if (hit)
        {
            Debug.DrawLine(cameraRef.transform.position, hitInfo.point, Color.green, 1f);
        }
        */

        return hit;
    }
}
