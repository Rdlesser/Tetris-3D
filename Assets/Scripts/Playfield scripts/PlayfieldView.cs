using Tetris_Block_Scripts;
using UnityEngine;

// Script to control the playing field view
namespace Playfield_scripts
{
    public class PlayfieldView : TetrisElement
    {

        private TetrisBlockView _currentMovingBlock;
        private GhostBlockView _currentGhostBlock;

        public TetrisBlockView CurrentMovingBlock => _currentMovingBlock;

        public bool IsPositionInsidePlayfield(Vector3 position)
        {
            bool isInside = position.y >= 0 && 
                            position.x >= 0 && position.x < App.model.gridSizeX && 
                            position.z >= 0 && position.z < App.model.gridSizeZ;

            return isInside;
        }

        public void SpawnNewBlock(TetrisBlockView blockToInstantiate, GhostBlockView ghostBlock, Vector3 position)
        {

            position += blockToInstantiate.centerPoint;
            
            _currentMovingBlock = Instantiate(blockToInstantiate,
                                                   position,
                                                   Quaternion.identity);
            _currentGhostBlock = Instantiate(ghostBlock,
                                             position,
                                             Quaternion.identity);
            _currentGhostBlock.SetParentBlock(_currentMovingBlock);

        }

        public void MoveCurrentBlockInDirection(Vector3 moveDirection)
        {
            _currentMovingBlock.MoveBlock(moveDirection);
        }

        public void RotateCurrentBlockInDirection(Vector3 rotationDirection)
        {
            _currentMovingBlock.RotateBlock(rotationDirection);
        }

        public void AttachInputToBlock()
        {
            if (ButtonInputView.Instance != null)
            {
                ButtonInputView.Instance.AttachedBlock = _currentMovingBlock.transform;
            }
        }

        public void SpeedDropCurrentBlock()
        {
            _currentMovingBlock.SpeedDropBlock();
        }
    }
}
