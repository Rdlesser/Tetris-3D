using System;
using System.Collections;
using System.Collections.Generic;
using Camera_scripts;
using Playfield_scripts;
using Tetris_Block_Scripts;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

// Controls the app workflow.
public class TetrisController : TetrisElement
{

    [SerializeField] private CameraController cameraController;

    private void Start()
    {
        SpawnNewBlock();
    }

    // Handles the win condition
    public void OnGameOver()
    {
        Debug.Log("Game Over!!!");
    }

    public void OnGridSizeChanged()
    {
        App.view.OnGridSizeChanged();
    }
    
    public void SpawnNewBlock()
    {
        TetrisBlockView[] availableBlocks = App.model.tetrisBlocks;
        var position = transform.position;
        Vector3 spawnPoint = new Vector3((int)(position.x + App.model.gridSizeX / 2.0f),
                                         (int)(position.y + App.model.gridSizeY),
                                         (int)(position.z + App.model.gridSizeZ / 2.0f));

        int randomIndex = Random.Range(0, availableBlocks.Length);
            
        // Spawn the block
        App.view.SpawnNewBlock(availableBlocks[randomIndex], spawnPoint);
        
        // TODO: Create "Ghost" for the block
        
        // TODO: Set Inputs
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
                    Debug.LogError("TetrisController.OnNotification - Could not move camera. Received incompatible types in data or null");
                }
                break;
            
            case TetrisNotifications.OnBlockMove:
                if (target != null &&
                    target.GetType() == typeof(TetrisBlockView) &&
                    data[0] != null &&
                    data[0] is Vector3)
                {
                    TetrisBlockView tetrisBlock = (TetrisBlockView) target;
                    Vector3 moveDirection = (Vector3) data[0];
                    App.model.UpdateGrid(tetrisBlock, moveDirection);
                }
                else
                {
                    Debug.LogError("TetrisController.OnNotification - Could not move block down. Received incompatible type");
                }
                break;
            
            case TetrisNotifications.BlockMovementStopped:
                SpawnNewBlock();
                break;
            
            case TetrisNotifications.OnArrowKeyPressed:
                if (data[0] != null &&
                    data[0] is Vector3)
                {
                    Vector3 moveDirection = (Vector3) data[0];
                    MoveCurrentBlockInDirection(moveDirection);
                }
                break;
            
        }
    }

    private void MoveCurrentBlockInDirection(Vector3 moveDirection)
    {
        App.view.MoveCurrentBlockInDirection(moveDirection);
    }
}

