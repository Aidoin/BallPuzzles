using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public Transform CameraTransform;
    public Transform PlayerTransform;

    private float mouseY;


    private void LateUpdate()
    {
        transform.position = PlayerTransform.position;
        transform.Rotate(0f, Input.GetAxis("Mouse X"), 0f);
        mouseY = Mathf.Clamp(mouseY += -Input.GetAxis("Mouse Y"), 10, 40);
        CameraTransform.localEulerAngles = new Vector3(mouseY, 0f, 0f);
    }
}
