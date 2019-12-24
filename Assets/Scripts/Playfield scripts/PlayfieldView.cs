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

        /// <summary>
        /// Check if a certain position is inside the visual grid 
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public bool IsPositionInsidePlayfield(Vector3 position)
        {
            bool isInside = position.y >= 0 && 
                            position.x >= 0 && position.x < App.model.gridSizeX && 
                            position.z >= 0 && position.z < App.model.gridSizeZ;

            return isInside;
        }

        /// <summary>
        /// Spawn a new block
        /// </summary>
        /// <param name="blockToInstantiate">The block to instantiate</param>
        /// <param name="ghostBlock">The ghost block (shadow) to be used for the given block</param>
        /// <param name="position">Position to instantiate the block</param>
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

        /// <summary>
        /// Move the current dropping block in a certain direction
        /// </summary>
        /// <param name="moveDirection">The direction to move</param>
        public void MoveCurrentBlockInDirection(Vector3 moveDirection)
        {
            //Tell the block to move in the direction
            _currentMovingBlock.MoveBlock(moveDirection);
        }

        /// <summary>
        /// Rotate the current dropping block in a certain direction
        /// </summary>
        /// <param name="rotationDirection">The direction in which to rotate the block</param>
        public void RotateCurrentBlockInDirection(Vector3 rotationDirection)
        {
            // Tell the block to rotate in the direction
            _currentMovingBlock.RotateBlock(rotationDirection);
        }

        /// <summary>
        /// Attach the input buttons to the current moving block
        /// </summary>
        public void AttachInputToBlock()
        {
            if (ButtonInputView.Instance != null)
            {
                ButtonInputView.Instance.AttachedBlock = _currentMovingBlock.transform;
            }
        }
        
        /// <summary>
        /// Speed up the fall of the current movign block
        /// </summary>
        public void SpeedDropCurrentBlock()
        {
            // Tell the current moving block to speed up 
            _currentMovingBlock.SpeedDropBlock();
        }
    }
}
