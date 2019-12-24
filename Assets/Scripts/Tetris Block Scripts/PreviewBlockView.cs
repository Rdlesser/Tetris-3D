using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewBlockView : TetrisElement
{

    private float _rotationSpeed = 50;
    
    private void Start()
    {
        _rotationSpeed = App.model.previewRotationSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(transform.position,
                               Vector3.up,
                               _rotationSpeed * Time.deltaTime);
    }
}
