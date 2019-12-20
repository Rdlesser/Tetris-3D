using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisCubeView : TetrisElement
{
    public bool IsValidMove(Vector3 direction)
    {
        if (!App.view.IsPositionInsidePlayfield(transform.position + direction))
        {
            return false;
        }

        if (App.model.IsPositionOccupied(transform, direction))
        {
            return false;
        }

        return true;
    }
}
