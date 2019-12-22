using Tetris_Block_Scripts;
using UnityEngine;

// Script to control the playing field view
namespace Playfield_scripts
{
    public class PlayfieldView : TetrisElement
    {

        private TetrisBlockView _currentMovingBlock;

        public TetrisBlockView CurrentMovingBlock => _currentMovingBlock;

        public bool IsPositionInsidePlayfield(Vector3 position)
        {
            bool isInside = position.y >= 0 && 
                            position.x >= 0 && position.x < App.model.gridSizeX && 
                            position.z >= 0 && position.z < App.model.gridSizeZ;

            return isInside;
        }

        public void SpawnNewBlock(TetrisBlockView blockToInstantiate, Vector3 position)
        {

            position += blockToInstantiate.centerPoint;
            
            _currentMovingBlock = Instantiate(blockToInstantiate,
                                                   position,
                                                   Quaternion.identity);
            // ButtonInputsView.Instance.ClampToBlock(_currentMovingBlock);
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
            if (TetrisButtonInputView.Instance != null)
            {
                TetrisButtonInputView.Instance.AttachedBlock = _currentMovingBlock.transform;
            }
        }

        public void SpeedDropCurrentBlock()
        {
            _currentMovingBlock.SpeedDropBlock();
        }
    }
}
