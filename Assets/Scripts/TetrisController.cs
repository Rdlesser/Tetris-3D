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

    public void OnNotification(TetrisAppNotifications eventString, Object target, object[] data)
    {
        switch (eventString)
        {
            case TetrisAppNotifications.GridResized:
                OnGridSizeChanged();
                break;
            
            case TetrisAppNotifications.CameraMoveAttempt:
                
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
                    throw new ArgumentException("TetrisController.OnNotification - Could not move camera. " + "Received incompatible types in data or null");
                }
                break;
            
            case TetrisAppNotifications.OnBlockSpawned:
                OnBlockSpawned();
                break;
            
            case TetrisAppNotifications.PrepareMove:
                if (target != null &&
                    target.GetType() == typeof(TetrisBlockView))
                {
                    TetrisBlockView tetrisBlock = (TetrisBlockView) target;
                    App.model.RemoveFromGrid(tetrisBlock);
                }
                else
                {
                    
                    throw new ArgumentException("TetrisController.OnNotification - move could not be prepared due to" + 
                                                " incompatible types in data or null");
                }
                break;
            
            case TetrisAppNotifications.OnBlockMoved:
                if (target != null &&
                    target.GetType() == typeof(TetrisBlockView))
                {
                    TetrisBlockView tetrisBlock = (TetrisBlockView) target;
                    App.model.UpdateGrid(tetrisBlock);
                }
                else
                {
                    throw new ArgumentException("TetrisController.OnNotification - Could not move block down. " +
                                                "Received incompatible type or null");
                }
                break;

            case TetrisAppNotifications.BlockMovementStopped:
                SpawnNewBlock();
                break;
            
            case TetrisAppNotifications.OnArrowKeyPressed:
                if (data[0] != null &&
                    data[0] is Vector3)
                {
                    Vector3 moveDirection = (Vector3) data[0];
                    RotateCurrentBlockInDirection(moveDirection);
                }
                break;
            
        }
    }

    private void OnBlockSpawned()
    {
        App.view.AttachInputToBlock();
    }

    private void MoveCurrentBlockInDirection(Vector3 moveDirection)
    {
        App.view.MoveCurrentBlockInDirection(moveDirection);
    }

    private void RotateCurrentBlockInDirection(Vector3 rotationDirection)
    {
        App.view.RotateCurrentBlockInDirection(rotationDirection);
    }
}

