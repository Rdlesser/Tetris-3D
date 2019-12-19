using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A view representing a tetris block
public class TetrisBlockView : TetrisElement
{

    private float _previousFallTime;
    private float _fallTime = 1.0f;
    

    // Update is called once per frame
    void Update()
    {
        // If it is time to drop position down
        if (Time.time - _previousFallTime > _fallTime)
        {
            if (IsValidMove())
            {
                Debug.LogError("It is a vlid move");
                transform.position += Vector3.down;
            }
            else
            {
                Debug.LogError("It is not a valid move");
                //Todo: Delete layer if possible
                enabled = false;
                
                //Todo: Create a new tetris block
            }
            
            _previousFallTime = Time.time;
        }
    }

    private bool IsValidMove()
    {
        foreach (Transform child in transform)
        {
            Vector3 position = App.model.RoundUpVector(child.position + Vector3.down);
            if (!App.view.IsPositionInsideGrid(position))
            {
                return false;
            }
        }
        return true;
    }
}
