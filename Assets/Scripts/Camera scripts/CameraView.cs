﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraView : TetrisElement
{
    
    // Target for the rotation around the Y Axis
    private Transform _targetYRotation;
    // Target for the rotation around the X Axis
    private Transform _targetXzRotation;
    
    
    // Awake is called before the first frame update
    void Awake()
    {
        _targetXzRotation = transform.parent;
        _targetYRotation = _targetXzRotation.parent;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(_targetYRotation);
        if (Input.GetMouseButton(0))
        {
            float locationY = Input.GetAxis("Mouse Y");
            float locationX = Input.GetAxis("Mouse X");
            App.Notify(TetrisAppNotifications.CameraMoveAttempt, this, 
                       _targetXzRotation, locationX, locationY);
        }

    }

    public void RotateCamera(Vector3 angles, float angleX)
    {
        _targetXzRotation.eulerAngles = angles;
        _targetYRotation.RotateAround(_targetYRotation.position, Vector3.up, angleX);
    }
}
