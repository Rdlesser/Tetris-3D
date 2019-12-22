using System;
using System.Collections;
using System.Collections.Generic;
using Camera_scripts;
using Playfield_scripts;
using Tetris_Block_Scripts;
using UnityEngine;
using UnityEngine.EventSystems;
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

    public void OnNotification(TetrisAppNotification eventString, Object target, object[] data)
    {
        switch (eventString)
        {
            case TetrisAppNotification.GridResized:
                OnGridSizeChanged();
                break;
            
            case TetrisAppNotification.CameraMoveAttempt:
                
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
            
            case TetrisAppNotification.OnBlockSpawned:
                OnBlockSpawned();
                break;
            
            case TetrisAppNotification.PrepareMove:
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
            
            case TetrisAppNotification.OnBlockMoved:
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

            case TetrisAppNotification.BlockMovementStopped:
                SpawnNewBlock();
                break;
            
            case TetrisAppNotification.OnMoveBlockClicked:
                if (data[0] != null &&
                    data[0].GetType() == typeof(TetrisMoveDirections))
                {
                    TetrisMoveDirections moveDirection = (TetrisMoveDirections) data[0];
                    MoveCurrentBlockInDirection(moveDirection);
                }
                else
                {
                    throw new
                        ArgumentException("TetrisController.OnNotification - " +
                                          "Could not move block in chosen direction. Received incompatible type or null");
                }
                break;
            
            case TetrisAppNotification.OnRotateBlockClicked:
                if (data[0] != null &&
                    data[0].GetType() == typeof(TetrisRotateDirection))
                {
                    TetrisRotateDirection rotateDirection = (TetrisRotateDirection) data[0];
                    RotateCurrentBlockInDirection(rotateDirection);
                }

                break;
            
            case TetrisAppNotification.OnSwitchDisplayClicked:
                SwitchDisplay();
                break;
            
            case TetrisAppNotification.OnDropClicked:
                SpeedDropCurrentBlock();
                break;
        }
    }


    private void OnBlockSpawned()
    {
        App.view.AttachInputToBlock();
    }

    private void MoveCurrentBlockInDirection(TetrisMoveDirections moveDirection)
    {
        switch (moveDirection)
        {
            case TetrisMoveDirections.Left:
                App.view.MoveCurrentBlockInDirection(Vector3.left);
                break;
            case TetrisMoveDirections.Right:
                App.view.MoveCurrentBlockInDirection(Vector3.right);
                break;
            case TetrisMoveDirections.Forward:
                App.view.MoveCurrentBlockInDirection(Vector3.forward);
                break;
            case TetrisMoveDirections.Back:
                App.view.MoveCurrentBlockInDirection(Vector3.back);
                break;
        }
    }

    private void RotateCurrentBlockInDirection(TetrisRotateDirection rotationDirection)
    {
        switch (rotationDirection)
        {
            case TetrisRotateDirection.XPositive:
                App.view.RotateCurrentBlockInDirection(Vector3.right * 90.0f);
                break;
            
            case TetrisRotateDirection.XNegative:
                App.view.RotateCurrentBlockInDirection(Vector3.left * 90.0f);
                break;
            
            case TetrisRotateDirection.YPositive:
                App.view.RotateCurrentBlockInDirection(Vector3.up * 90.0f);
                break;
            
            case TetrisRotateDirection.YNegative:
                App.view.RotateCurrentBlockInDirection(Vector3.down * 90.0f);
                break;
            
            case TetrisRotateDirection.ZPositive:
                App.view.RotateCurrentBlockInDirection(Vector3.forward * 90.0f);
                break;
                
            case TetrisRotateDirection.ZNegative:
                App.view.RotateCurrentBlockInDirection(Vector3.back * 90.0f);
                break;
                
                    
        }
        
    }
    
    private void SwitchDisplay()
    {
        App.view.SwitchHudDisplay();
    }
    
    private void SpeedDropCurrentBlock()
    {
        App.view.SpeedDropCurrentBlock();
    }
}

