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
        public bool verticalAutoReturn = false;

        //Private variables
        private Transform pivot;
        private float lookAngle;
        private float tiltAngle;
        private Vector3 pivotEulers;
        private Quaternion pivotTargetRot;
        private Quaternion transformTargetRot;

        protected override void Awake()
        {
            //
            pivot = transform.GetChild(0);
            pivotEulers = pivot.rotation.eulerAngles;
            pivotTargetRot = pivot.transform.localRotation;
            transformTargetRot = transform.localRotation;
        }

        protected void Update()
        {
            HandleRotationMovement();
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
                pivot.localRotation = Quaternion.Slerp(pivot.localRotation, pivotTargetRot, turnSmoothing * Time.deltaTime);
                transform.localRotation = Quaternion.Slerp(transform.localRotation, transformTargetRot, turnSmoothing * Time.deltaTime);
            }
            else
            {
                pivot.localRotation = pivotTargetRot;
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
        }

    }
}
