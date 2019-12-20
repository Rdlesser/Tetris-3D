using System.Collections;
using System.Collections.Generic;
using Camera_scripts;
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

    public void OnNotification(TetrisNotifications eventString, Object target, object[] data)
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
            
            case TetrisNotifications.OnBlockMoveDown:
                if (target.GetType() == typeof(TetrisBlockView))
                {
                    TetrisBlockView tetrisBlock = (TetrisBlockView) target;
                    App.model.UpdateGrid(tetrisBlock, Vector3.down);
                }
                break;
        }
    }
}

