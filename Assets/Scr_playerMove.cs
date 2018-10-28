using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityStandardAssets.Characters.FirstPerson;

public class Scr_playerMove : MonoBehaviour
{
    [SerializeField] private MouseLook mouseLook;

    public float rayDistance = 500f;
    public LayerMask selectableLayer;
    public GameObject cameraRef;
    public float sensativity = 0.1f;
    public float xRotLowerClamp = 275f;
    public float xRotUpperClamp = 85f;

    private Ray ray;
    private RaycastHit hitInfo;
    private float mousePosX;
    private float mousePosY;
    private float mousePosXDiff;
    private float mousePosYDiff;
    private float xRot;
    private float halfWidth;
    private float halfHeight;
    private Camera camera;

    
    void Start()
    {
        halfWidth = Screen.width / 2;
        halfHeight = Screen.height / 2;

        mousePosX = Input.mousePosition.x;
        mousePosY = Input.mousePosition.y;

        camera = Camera.main;
        mouseLook.Init(transform, camera.transform);
    }

    void Update()
    {
        RotateView();

        mousePosXDiff = Input.mousePosition.x - mousePosX;
        mousePosYDiff = Input.mousePosition.y - mousePosY;

        if (Input.GetAxis("Fire2") > 0)
        {
            //SetRotation();

        }

        if (Input.GetAxis("Fire1") > 0)
        {
            if(CastRay())
            {
                print(hitInfo.transform.gameObject);
            }
        }

        mousePosX = Input.mousePosition.x;
        mousePosY = Input.mousePosition.y;
    }

    private void FixedUpdate()
    {
        mouseLook.UpdateCursorLock();
    }

    private void RotateView()
    {
        mouseLook.LookRotation(transform, camera.transform);
    }

    private void SetRotation()
    {
        xRot = ClampCameraRotation(transform.rotation.eulerAngles.x + mousePosYDiff);

        transform.SetPositionAndRotation(
            transform.position,
            Quaternion.Euler(
                xRot,
                transform.rotation.eulerAngles.y + mousePosXDiff,
                0));
    }

    private bool CastRay()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        bool hit = Physics.Raycast(ray, out hitInfo);
        if(hit)
        {
            Debug.DrawLine(cameraRef.transform.position, hitInfo.point, Color.green, 1f);
        }
    
        return hit;
    }

    private float ClampCameraRotation(float x)
    {
        if (x > xRotUpperClamp && x < xRotLowerClamp)
        {
            if (x + 180 < 360)
            {
                return xRotUpperClamp;
            }
            else
            {
                return xRotLowerClamp;
            }
        }
        else
        {
            return x;
        }
    }
}