using System;
using UnityEngine;

// Script to control the playing field view
namespace Playfield_scripts
{
    public class PlayfieldView : TetrisElement
    {

        public bool IsPositionInsideGrid(Vector3 position)
        {
            bool isInside = (int) position.x >= 0 && (int) position.x < App.model.GridSizeX &&
                            (int) position.z >= 0 && (int) position.y < App.model.GridSizeZ &&
                            (int) position.y >= 0;
            return isInside;
        }

    }
}
