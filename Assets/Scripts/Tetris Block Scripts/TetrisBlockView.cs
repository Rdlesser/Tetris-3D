using UnityEngine;

// A view representing a tetris block
namespace Tetris_Block_Scripts
{
    public class TetrisBlockView : TetrisElement
    {

        [SerializeField] private int height;
        
        private float _previousFallTime;
        private float _fallTime = 1.0f;
        public int Height => height;

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
                    transform.position += Vector3.down;

                    App.Notify(TetrisNotifications.OnBlockMove, this, Vector3.down);
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
            
        }

        public bool MoveBlock(Vector3 direction)
        {
            bool canMove = false;
            if (IsValidMove(direction))
            {
                transform.position += direction;
                canMove = true;
                App.Notify(TetrisNotifications.OnBlockMove, this, direction);
            }

            return canMove;

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
    }
}
