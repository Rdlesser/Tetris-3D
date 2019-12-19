using UnityEngine;

namespace Camera_scripts
{
    public class CameraController : TetrisElement
    {
    
        [SerializeField] private CameraView cameraView;
    
        [SerializeField] private float cameraSensitivity = 4.0f;
        

        private void MoveCamera(Transform targetXZRotation, float locationX, float locationY)
        {
        
            float angleY = locationY * -cameraSensitivity;
            float angleX = locationX * cameraSensitivity;
        
            Vector3 angles = targetXZRotation.eulerAngles;
            angles.x += angleY;
            angles.x = ClampAngle(angles.x, -70f, 70f);
            cameraView.RotateCamera(angles, angleX);

        }

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

        public void OnCameraMoveAttempt(Transform targetXZRotation, float locationX, float locationY)
        {
            MoveCamera(targetXZRotation, locationX, locationY);
        }
    }
}
