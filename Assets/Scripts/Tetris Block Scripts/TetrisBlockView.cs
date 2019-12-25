using System;
using UnityEngine;

// A view representing a tetris block
namespace Tetris_Block_Scripts
{
    public class TetrisBlockView : TetrisElement
    {

        private float _previousFallTime;
        private float _fallTime;
        public Vector3 centerPoint;

        [SerializeField] private TetrisCubeView[] childCubes;
        [SerializeField] private GhostBlockView ghostBlock;
        [SerializeField] private PreviewBlockView preview;

        public TetrisCubeView[] ChildCubes => childCubes;
        public GhostBlockView GhostBlock => ghostBlock;
        public PreviewBlockView Preview => preview;

        private void Start()
        {
            _fallTime = App.model.CurrentFallSpeed;
            App.Notify(TetrisAppNotification.OnBlockSpawned, this, transform);
            if (!IsValidMove(Vector3.down))
            {
                App.Notify(TetrisAppNotification.OnBlockBlocked, this);
            }
        }


        // Update is called once per frame
        void Update()
        {
            // If it is time to drop position down
            if (Time.time - _previousFallTime > _fallTime)
            {
                if (IsValidMove(Vector3.down))
                {
                    
                    App.Notify(TetrisAppNotification.PrepareMove, this);
                    transform.position += Vector3.down;

                    App.Notify(TetrisAppNotification.OnBlockMoved, this);
                }
                // The block's down movement is blocked by either another block or by the floor
                else
                {
                    //Todo: Delete layer if possible
                    enabled = false;
                
                    // Notify the controller that the tetris block has finished its movement
                    App.Notify(TetrisAppNotification.BlockMovementStopped, this);
                }
            
                _previousFallTime = Time.time;
            }

        }

        /// <summary>
        /// Quickly move the block down
        /// </summary>
        public void SpeedDropBlock()
        {
            _fallTime *= 0.01f;
        }
        
        /// <summary>
        /// Move the block in a specified direction
        /// </summary>
        /// <param name="direction">The direction in which to move the block</param>
        public void MoveBlock(Vector3 direction)
        {
            if (IsValidMove(direction))
            {
                // Notify the app that the block is about to move in case there need to be some preparations
                App.Notify(TetrisAppNotification.PrepareMove, this);
                // Move the block in the specific direction
                transform.position += direction;
                // Notify the app that the block has moved
                App.Notify(TetrisAppNotification.OnBlockMoved, this);
            }
        }

        /// <summary>
        /// Rotate the block in a specified direction
        /// </summary>
        /// <param name="direction">The direction in which to rotate the block</param>
        public void RotateBlock(Vector3 direction)
        {
            if (IsValidRotation(direction))
            {
                // Notify the app that the block is about to move in case there need to be some preparations
                App.Notify(TetrisAppNotification.PrepareMove, this);
                // Rotate the block in the given direction
                transform.Rotate(direction, Space.World);
                // Notify the app that the block has moved to a new position
                App.Notify(TetrisAppNotification.OnBlockMoved, this);
            }
        }

        /// <summary>
        /// Check if the block can move in a certain direction
        /// </summary>
        /// <param name="direction">The direction in which we would like to move the block</param>
        /// <returns>
        /// True if the move is valid
        /// False otherwise
        /// </returns>
        private bool IsValidMove(Vector3 direction)
        {
            // For each cube of the tetris block - check if the cube can move in the direction
            foreach (TetrisCubeView child in childCubes)
            {
                if (!child.IsValidMove(direction))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Check if the block can rotate in a specified direction
        /// </summary>
        /// <param name="rotationDirection">The direction of the rotation we would like to check</param>
        /// <returns>
        /// True - if we the rotation is valid
        /// False - otherwise
        /// </returns>
        private bool IsValidRotation(Vector3 rotationDirection)
        {
            // Perform rotation and check current location is valid
            transform.Rotate(rotationDirection, Space.World);
            if (!IsValidMove(Vector3.zero))
            {
                transform.Rotate(-rotationDirection, Space.World);
                return false;
            }
            transform.Rotate(-rotationDirection, Space.World);
            return true;
        }

    }
}
