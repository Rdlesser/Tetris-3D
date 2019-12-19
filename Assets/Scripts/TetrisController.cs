using System.Collections;
using System.Collections.Generic;
using Playfield_scripts;
using UnityEngine;

// Controls the app workflow.
public class TetrisController : TetrisElement
{

    [SerializeField] private CameraController cameraController;

    // Handles the win condition
    public void OnGameOver()
    {
        Debug.Log("Game Over!!!");
    }

    public void OnGridSizeChanged()
    {
        App.view.OnGridSizeChanged();
    }

    public void OnNotification(string eventString, Object target, object[] data)
    {
        switch (eventString)
        {
            case TetrisNotifications.GridResize:
                OnGridSizeChanged();
                break;
            
            case TetrisNotifications.CameraMoveAttempt:
                if (data[0] != null && 
                    data[1] != null && 
                    data[2] != null &&
                    data[0].GetType() == typeof(Transform) &&
                    data[1] is float &&
                    data[2] is float)
                {
                    cameraController.OnCameraMoveAttempt((Transform)data[0], 
                                                         (float)data[1],
                                                         (float) data[2]);
                }
                else
                {
                    Debug.LogError("TetrisController.OnNotification received incompatible types in data or null");
                }
                break;
        }
    }
}

