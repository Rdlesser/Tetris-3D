using System;
using UnityEngine;

// Script to control the playing field view
namespace Playfield_scripts
{
    public class PlayfieldView : TetrisElement
    {

        public bool IsPositionInsidePlayfield(Vector3 position)
        {
            bool isInside = position.y >= 0 && 
                            position.x >= 0 && position.x < App.model.gridSizeX && 
                            position.z >= 0 && position.z < App.model.gridSizeZ;

            return isInside;
        }

    }
}
