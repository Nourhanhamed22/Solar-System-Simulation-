using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AstronautThirdPersonCamera
{
    public class AstronautThirdPersonCamera : MonoBehaviour
    {
        private const float Y_ANGLE_MIN = -30.0f;
        private const float Y_ANGLE_MAX = 60.0f;

        public Transform lookAt;
        public Transform camTransform;

        public float distance = 10.0f;

        private float currentX = 0.0f;
        private float currentY = 20.0f;

        public float sensitivityX = 3.0f;
        public float sensitivityY = 3.0f;

        private void Start()
        {
            camTransform = transform;
            Cursor.lockState = CursorLockMode.Locked; // locks mouse to screen
            Cursor.visible = false;
        }

        private void Update()
        {
            currentX += Input.GetAxis("Mouse X") * sensitivityX;
            currentY -= Input.GetAxis("Mouse Y") * sensitivityY;
            currentY = Mathf.Clamp(currentY, Y_ANGLE_MIN, Y_ANGLE_MAX);
        }

        private void LateUpdate()
        {
            if (lookAt == null) return;
            Vector3 dir = new Vector3(0, 0, -distance);
            Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
            camTransform.position = Vector3.Lerp(
                camTransform.position,
                lookAt.position + rotation * dir,
                Time.deltaTime * 10f
            );
            camTransform.LookAt(lookAt.position);
        }
    }
}