using UnityEngine;

namespace Camera_scripts
{
    /// <summary>
    /// A controller for the camera
    /// </summary>
    public class CameraController : TetrisElement
    {
    
        [SerializeField] private CameraView cameraView;
    
        [SerializeField] private float cameraSensitivity = 4.0f;
        

        /// <summary>
        /// Move and rotate the camera around the Y axis and XZ field 
        /// </summary>
        /// <param name="targetXZRotation">The target of the XZRotation</param>
        /// <param name="locationX">The location of the mouse click on the x axis</param>
        /// <param name="locationY">The location of the mouse click on the Y axis</param>
        private void MoveCamera(Transform targetXZRotation, float locationX, float locationY)
        {
        
            float angleY = locationY * -cameraSensitivity;
            float angleX = locationX * cameraSensitivity;
        
            Vector3 angles = targetXZRotation.eulerAngles;
            angles.x += angleY;
            // Prevent moving camera beyond -70 and 70 to prevent the jumping motion that happens when reaching 90 degrees
            angles.x = ClampAngle(angles.x, -70f, 70f);
            cameraView.RotateCamera(angles, angleX);

        }

        /// <summary>
        /// Clamp an angle between 2 values
        /// </summary>
        /// <param name="angle">The angle to clamp</param>
        /// <param name="from">The min value</param>
        /// <param name="to">The max value</param>
        /// <returns></returns>
        private float ClampAngle(float angle, float from, float to)
        {
            if (angle < 0)
            {
                angle += 360;
            }

            if (angle > 180f)
            {
                return Mathf.Max(angle, 360 + from);
            }

            return Mathf.Min(angle, to);
        }

        /// <summary>
        /// Method called when an attempt to move the camera has been registered by the view
        /// </summary>
        /// <param name="targetXZRotation">The transform to be used for the camera movement </param>
        /// <param name="locationX">Location of the mouse click on the x axis</param>
        /// <param name="locationY">Location of the mouse click on the Y axis</param>
        public void OnCameraMoveAttempt(Transform targetXZRotation, float locationX, float locationY)
        {
            MoveCamera(targetXZRotation, locationX, locationY);
        }
    }
}
