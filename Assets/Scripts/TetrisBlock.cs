using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisBlock : TetrisElement
{

    private float _previousFallTime;
    private float _fallTime = 1.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - _previousFallTime > _fallTime)
        {
            transform.position += Vector3.down;
            _previousFallTime = Time.time;
        }
    }

    private bool CheckValidMove()
    {
        return false;
    }
}
