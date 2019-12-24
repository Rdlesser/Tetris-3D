using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A Class representing a cube - the smalles piece in the game that is the building block of the tetris block
/// </summary>
public class TetrisCubeView : TetrisElement
{
    /// <summary>
    /// Check if a move in a specified direction is valid
    /// </summary>
    /// <param name="direction">The direction we would like to check the move for</param>
    /// <returns>
    /// True if the move is valid
    /// False otherwise
    /// </returns>
    public bool IsValidMove(Vector3 direction)
    {
        // Check if the new position is inside the playfield
        if (!App.view.IsPositionInsidePlayfield(transform.position + direction))
        {
            return false;
        }
        
        // Check if the position we are trying to move the cube to is occupied by another cube
        if (App.model.IsPositionOccupied(transform, direction))
        {
            return false;
        }

        return true;
    }
}
