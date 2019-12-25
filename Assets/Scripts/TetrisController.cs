using System;
using System.Collections.Generic;
using Camera_scripts;
using Tetris_Block_Scripts;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

// Controls the app workflow.
public class TetrisController : TetrisElement
{

    [SerializeField] private CameraController cameraController;

    private int _gridSizeX;
    private int _gridSizeY;
    private int _gridSizeZ;
    
    private TetrisBlockView[] _weightedBlockList;
    private int _nextBlockIndex;

    private void Start()
    {
        FirstPreparations();
        CalculateNextBlock();
        SpawnNewBlock();
    }

    /// <summary>
    /// Prepare the private fields 
    /// </summary>
    private void FirstPreparations()
    {
        // Locally save the grid size
        _gridSizeX = App.model.gridSizeX;
        _gridSizeY = App.model.gridSizeY;
        _gridSizeZ = App.model.gridSizeZ;
        
        // Create a weighted block list to be used for spawning blocks
        TetrisBlockView[] availableBlocks = App.model.tetrisBlocks;
        int[] blockWeights = App.model.blockWeights;
        _weightedBlockList = createWeightedBlockList(availableBlocks, blockWeights);
    }

    // Handles the lose condition
    public void OnGameOver()
    {
        App.model.IsGameOver = true;
        App.view.ShowGameOverWindow();
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
        
        var position = App.view.GetPlayfieldPosition();
        // Choose the position of the block - top of the center field
        Vector3 spawnPoint = new Vector3((int)(position.x + _gridSizeX / 2.0f),
                                         (int)(position.y + _gridSizeY),
                                         (int)(position.z + _gridSizeZ / 2.0f));
        
        // Spawn the block
        App.view.SpawnNewBlock(_weightedBlockList[_nextBlockIndex], 
                               _weightedBlockList[_nextBlockIndex].GhostBlock,
                               spawnPoint);

        // Calculate the next preview index
        CalculateNextBlock();
        App.view.ShowPreview(_weightedBlockList[_nextBlockIndex].Preview);

    }

    public void CalculateNextBlock()
    {
        _nextBlockIndex = Random.Range(0, _weightedBlockList.Length);
    }

    private TetrisBlockView[] createWeightedBlockList(TetrisBlockView[] blocks, int[] weights)
    {
        List<TetrisBlockView> blockList = new List<TetrisBlockView>();
        foreach (var block in blocks)
        {
            for (int i = 0; i < weights.Length; i++)
            {
                blockList.Add(block);
            }
        }
        return blockList.ToArray();
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
                    OnBlockDropped((TetrisBlockView) target);
                    
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
                
            case TetrisAppNotification.OnBlockBlocked:
                OnGameOver();
                break;
        }
    }

    private void OnBlockDropped(TetrisBlockView tetrisBlock)
    {
        App.model.AddToScore(10 * App.model.CurrentLevel);
        DeleteLayersIfPossible(tetrisBlock);
        App.view.UpdateUI();
        if (!App.model.IsGameOver)
        {
            SpawnNewBlock();
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
            
            App.model.AddToCompleteLayers(markedLayers.Count);
            
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

