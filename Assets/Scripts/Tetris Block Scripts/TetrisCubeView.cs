using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisCubeView : TetrisElement
{
    public bool IsValidMove()
    {
        if (!App.view.IsPositionInsidePlayfield(transform.position + Vector3.down))
        {
            return false;
        }

        if (App.model.IsPositionOccupied(transform))
        {
            return false;
        }

        return true;
    }
}
