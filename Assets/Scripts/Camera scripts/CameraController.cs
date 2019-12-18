using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : TetrisElement
{
    // Target for the rotation around the Y Axis
    [SerializeField] private Transform targetYRotation;
    // Target for the rotation around the X Axis
    [SerializeField] private Transform targetXZRotation;

    [SerializeField] private Transform cameraTransform;
    
    [SerializeField] private float cameraSensitivity = 4.0f;


    private Vector3 _lastTouchPosition;

    
    // Start is called before the first frame update
    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        cameraTransform.LookAt(targetYRotation);
        if (Input.GetMouseButton(0))
        {
            _lastTouchPosition = Input.mousePosition;
            Orbit();
        }

        
    }

    private void Orbit()
    {
        
        Vector3 rotationDelta = Input.mousePosition - _lastTouchPosition;
        float angleY = Input.GetAxis("Mouse Y") * -cameraSensitivity;
        float angleX = Input.GetAxis("Mouse X") * cameraSensitivity;
        
        // X Axis
        var transform1 = targetXZRotation.transform;
        Vector3 angles = transform1.eulerAngles;
        angles.x += angleY;
        angles.x = ClampAngle(angles.x, -70f, 70f);
        transform1.eulerAngles = angles;

        // Y Axis
        targetYRotation.RotateAround(targetYRotation.position, Vector3.up, angleX);
        _lastTouchPosition = Input.mousePosition;
        
        
    }

    private float ClampAngle(float angle, float from, float to)
    {
        if (angle < 0)
        {
            angle += 360;
        }

        if (angle > 180f)
        {
            return Mathf.Max(angle, 360 + from);
        }

        return Mathf.Min(angle, to);
    }
}
