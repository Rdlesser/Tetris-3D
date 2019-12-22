using System;
using UnityEngine;

// A view representing a tetris block
namespace Tetris_Block_Scripts
{
    public class TetrisBlockView : TetrisElement
    {

        private float _previousFallTime;
        private float _fallTime = 1.0f;
        public Vector3 centerPoint;

        [SerializeField] private TetrisCubeView[] childCubes;

        public TetrisCubeView[] ChildCubes => childCubes;

        private void Start()
        {
            App.Notify(TetrisAppNotification.OnBlockSpawned, this, transform);
            // ButtonInputsView.Instance.ClampToBlock(this);
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

        public void SpeedDropBlock()
        {
            _fallTime *= 0.1f;
        }

        public void MoveBlock(Vector3 direction)
        {
            bool canMove = false;
            if (IsValidMove(direction))
            {
                App.Notify(TetrisAppNotification.PrepareMove, this);
                transform.position += direction;
                canMove = true;
                App.Notify(TetrisAppNotification.OnBlockMoved, this);
            }
        }

        public void RotateBlock(Vector3 direction)
        {
            if (IsValidRotation(direction))
            {
                App.Notify(TetrisAppNotification.PrepareMove, this);
                transform.Rotate(direction, Space.World);
                App.Notify(TetrisAppNotification.OnBlockMoved, this);
            }
        }

        private bool IsValidMove(Vector3 direction)
        {
            foreach (TetrisCubeView child in childCubes)
            {
                if (!child.IsValidMove(direction))
                {
                    return false;
                }
            }
            return true;
        }

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
