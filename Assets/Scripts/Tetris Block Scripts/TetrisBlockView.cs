using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A view representing a tetris block
public class TetrisBlockView : TetrisElement
{

    private float _previousFallTime;
    private float _fallTime = 1.0f;

    [SerializeField] private TetrisCubeView[] childCubes;

    public TetrisCubeView[] ChildCubes => childCubes;
    

    // Update is called once per frame
    void Update()
    {
        // If it is time to drop position down
        if (Time.time - _previousFallTime > _fallTime)
        {
            if (IsValidMove())
            {
                transform.position += Vector3.down;

                App.Notify(TetrisNotifications.OnBlockMoveDown, this);
            }
            else
            {
                //Todo: Delete layer if possible
                enabled = false;
                
                //Todo: Create a new tetris block
            }
            
            _previousFallTime = Time.time;
        }
    }

    private bool IsValidMove()
    {
        foreach (TetrisCubeView child in childCubes)
        {
            if (!child.IsValidMove())
            {
                return false;
            }
        }
        return true;
    }
}
