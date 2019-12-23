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

    // Handles the lose condition
    public void OnGameOver()
    {
        Debug.Log("Game Over!!!");
    }

    /// <summary>
    /// A Method to be called when the grid size has changed 
    /// </summary>
    public void OnGridSizeChanged()
    {
        App.view.OnGridSizeChanged();
    }
    
    /// <summary>
    /// Spawn a new block 
    /// </summary>
    public void SpawnNewBlock()
    {
        // Randomly choose a block to spawn according to our available blocks
        TetrisBlockView[] availableBlocks = App.model.tetrisBlocks;
        var position = transform.position;
        // Choose the position of the block - top of the center field
        Vector3 spawnPoint = new Vector3((int)(position.x + App.model.gridSizeX / 2.0f),
                                         (int)(position.y + App.model.gridSizeY),
                                         (int)(position.z + App.model.gridSizeZ / 2.0f));

        int randomIndex = Random.Range(0, availableBlocks.Length);
            
        // Spawn the block
        App.view.SpawnNewBlock(availableBlocks[randomIndex], spawnPoint);
        
        // TODO: Create "Ghost" for the block
        
        // TODO: Set Inputs
    }

    /// <summary>
    /// A listener function for the app's notifications
    /// </summary>
    /// <param name="eventString">A string (enum) representation of the event</param>
    /// <param name="target">The target of the notification - who is the notifier</param>
    /// <param name="data">Extra data that was passed with the notification</param>
    /// <exception cref="ArgumentException">
    /// An exception is thrown when a notification with insufficient or missing data was sent
    /// </exception>
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
                    App.model.RemoveBlockFromGrid(tetrisBlock);
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
                if (target != null &&
                    target.GetType() == typeof(TetrisBlockView))
                {
                    TetrisBlockView tetrisBlock = (TetrisBlockView) target;
                    DeleteLayersIfPossible(tetrisBlock);
                    SpawnNewBlock();
                }
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

    private void DeleteLayersIfPossible(TetrisBlockView tetrisBlock)
    {
        List<int> markedLayers = MarkDeletionLayers(tetrisBlock);
        markedLayers.Sort();
        int dropCount = 0;
        if (markedLayers.Count > 0)
        {
            foreach (var markedLayer in markedLayers)
            {
                App.model.DeleteLayer(markedLayer);
            }
            
            foreach (var markedLayer in markedLayers)
            {
                App.model.DropHigherLayers(markedLayer - dropCount);
                dropCount++;
            }
            
        }
    }

    private List<int> MarkDeletionLayers(TetrisBlockView tetrisBlock)
    {
        List<int> fullLayers = new List<int>();
        foreach (var child in tetrisBlock.ChildCubes)
        {
            float childYPosition = child.transform.position.y;
            if (App.model.IsLayerFull((int)childYPosition))
            {
                if (!fullLayers.Contains((int)childYPosition))
                {
                    fullLayers.Add((int) childYPosition);
                }
            }
        }

        return fullLayers;
    }


    /// <summary>
    /// Function to be called when a new block is spawned.
    /// </summary>
    private void OnBlockSpawned()
    {
        // Tell the app's view to attach the input buttons to the newly created block
        App.view.AttachInputToBlock();
    }

    /// <summary>
    /// Move the currently falling block in a given direction
    /// </summary>
    /// <param name="moveDirection">The direction to move the block</param>
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

    /// <summary>
    /// Rotate the currently falling drop in a specified direction
    /// </summary>
    /// <param name="rotationDirection">The direction to rotate</param>
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
    
    /// <summary>
    /// Switch the HUD display between movement arrows and rotation arrows
    /// </summary>
    private void SwitchDisplay()
    {
        // Tell the app's view to switch between displays
        App.view.SwitchHudDisplay();
    }
    
    /// <summary>
    /// Make the currently falling block drop faster
    /// </summary>
    private void SpeedDropCurrentBlock()
    {
        // Tell the app's view to speed up the block
        App.view.SpeedDropCurrentBlock();
    }
}

