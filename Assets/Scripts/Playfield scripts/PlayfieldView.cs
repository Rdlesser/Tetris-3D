using Tetris_Block_Scripts;
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

        public void SpawnNewBlock(TetrisBlockView blockToInstantiate, Vector3 position)
        {

            position.y += blockToInstantiate.Height / 2;
            
            TetrisBlockView newBlock = Instantiate(blockToInstantiate,
                                                   position,
                                                   Quaternion.identity);
        }
    }
}
