using System;
using UnityEngine;

// Script to control the playing field view
namespace Playfield_scripts
{
    public class PlayfieldView : TetrisElement
    {

        public bool IsPositionInsideGrid(Vector3 position)
        {
            bool isInside = position.y >= 0 && 
                            position.x >= 0 && position.x < App.model.GridSizeX && 
                            position.z >= 0 && position.z < App.model.GridSizeZ;

            return isInside;
        }

    }
}
