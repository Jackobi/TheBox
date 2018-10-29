using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets.Cameras
{
    public class Scr_playerCam : PivotBasedCameraRig
    {
        //Public variables
        [Range(0f,10f)]public float turnSpeed = 1.5f;
        public float lerpSpeed = 1f;
        public float lookMin = 45f;
        public float lookMax = 75f;
        public float tiltMin = 45f;
        public float tiltMax = 75f;
        public float turnSmoothing = 0.0f;
        //Boom length should be multiplied by -1 when called
        [Range(0f, 5f)]public float boomLength = 2f;
        public bool verticalAutoReturn = false;

        //Private variables
        private Transform pivotRef;
        private Transform cameraRef;
        private float lookAngle;
        private float tiltAngle;
        private Vector3 cameraPos;
        private Vector3 pivotEulers;
        private Quaternion pivotTargetRot;
        private Quaternion transformTargetRot;

        protected override void Awake()
        {
            
            pivotRef = transform.GetChild(0);
            cameraRef = pivotRef.transform.GetChild(0);
            pivotEulers = pivotRef.rotation.eulerAngles;
            pivotTargetRot = pivotRef.transform.localRotation;
            transformTargetRot = transform.localRotation;

            cameraPos = new Vector3(cameraRef.transform.position.x, cameraRef.transform.position.y, -boomLength);
            cameraRef.transform.position = cameraPos;

        }

        protected void Update()
        {
            if(Input.GetAxis("Fire2") > 0)
            {
                HandleRotationMovement();
            }
        }

        private void HandleRotationMovement()
        {
            if (Time.timeScale < float.Epsilon)
            {
                return;
            }

            var x = CrossPlatformInputManager.GetAxis("Mouse X");
            var y = CrossPlatformInputManager.GetAxis("Mouse Y");

            //Handle horizontal looking
            lookAngle += x * turnSpeed;
            lookAngle = Mathf.Clamp(lookAngle, -lookMin, lookMax);

            //Handle vertical tilting
            transformTargetRot = Quaternion.Euler(0f, lookAngle, 0f);
            if (verticalAutoReturn)
            {
                //For mobile
                tiltAngle = y > 0 ? Mathf.Lerp(0, -tiltMin, y) : Mathf.Lerp(0, tiltMax, -y);
            }
            else
            {
                //For mouse
                tiltAngle -= y * turnSpeed;
                tiltAngle = Mathf.Clamp(tiltAngle, -tiltMin, tiltMax);
            }

            //Apply tilt to pivot
            pivotTargetRot = Quaternion.Euler(tiltAngle, pivotEulers.y, pivotEulers.z);

            //Apply turn smoothing if enabled
            if (turnSmoothing > 0)
            {
                pivotRef.localRotation = Quaternion.Slerp(pivotRef.localRotation, pivotTargetRot, turnSmoothing * Time.deltaTime);
                transform.localRotation = Quaternion.Slerp(transform.localRotation, transformTargetRot, turnSmoothing * Time.deltaTime);
            }
            else
            {
                pivotRef.localRotation = pivotTargetRot;
                transform.localRotation = transformTargetRot;
            }
        }

        protected override void FollowTarget(float deltaTime)
        {
            if (m_Target == null)
            {
                return;
            }

            // Move the rig towards target position.
            transform.position = Vector3.Lerp(transform.position, m_Target.position, deltaTime * lerpSpeed);
            //Adjust boom distance
            cameraPos = new Vector3(cameraRef.transform.position.x, cameraRef.transform.position.y, -boomLength);
            cameraRef.transform.position = Vector3.Lerp(cameraRef.transform.position, cameraPos,deltaTime * lerpSpeed);
        }

        public void ChangeBoomLength(float newLength)
        {
            boomLength = newLength;
        }

    }
}
