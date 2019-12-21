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
    

        // Update is called once per frame
        void Update()
        {
            // If it is time to drop position down
            if (Time.time - _previousFallTime > _fallTime)
            {
                if (IsValidMove(Vector3.down))
                {
                    
                    App.Notify(TetrisNotifications.PrepareMove, this);
                    transform.position += Vector3.down;

                    App.Notify(TetrisNotifications.OnBlockMoved, this);
                }
                // The block's down movement is blocked by either another block or by the floor
                else
                {
                    //Todo: Delete layer if possible
                    enabled = false;
                
                    // Notify the controller that the tetris block has finished its movement
                    App.Notify(TetrisNotifications.BlockMovementStopped, this);
                }
            
                _previousFallTime = Time.time;
            }

            OnArrowKeyInput();

        }

        public bool MoveBlock(Vector3 direction)
        {
            bool canMove = false;
            if (IsValidMove(direction))
            {
                App.Notify(TetrisNotifications.PrepareMove, this);
                transform.position += direction;
                canMove = true;
                App.Notify(TetrisNotifications.OnBlockMoved, this);
            }

            return canMove;

        }

        public bool RotateBlock(Vector3 direction)
        {
            Debug.LogError("Rotating Block!!!!!!!!!!!!!!! Direction: " + direction);
            bool canRotate = false;
            if (IsValidRotation(direction))
            {
                App.Notify(TetrisNotifications.PrepareMove, this);
                transform.Rotate(direction, Space.World);
                canRotate = true;
                App.Notify(TetrisNotifications.OnBlockMoved, this);
            }

            return canRotate;
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
        
        private void OnArrowKeyInput()
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                // App.Notify(TetrisNotifications.OnArrowKeyPressed, this, Vector3.left * 90.0f);
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                // App.Notify(TetrisNotifications.OnArrowKeyPressed, this, Vector3.right * 90.0f);
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                App.Notify(TetrisNotifications.OnArrowKeyPressed, this, Vector3.right * 90.0f);
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                App.Notify(TetrisNotifications.OnArrowKeyPressed, this, Vector3.left * 90.0f);
            }
        }
    }
}
