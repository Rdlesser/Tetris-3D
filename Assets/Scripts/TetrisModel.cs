using System;
using Tetris_Block_Scripts;
using UnityEngine;

/// <summary>
/// Contains all the raw data of the app
/// </summary>
public class TetrisModel : TetrisElement
{

    [Header("Grid Size Config")]
    // Data
    public int gridSizeX = 7;
    public int gridSizeY = 10;
    public int gridSizeZ = 7;

    [Header("Tetris Blcok Config")]
    [Tooltip("Fall speed in seconds to drop. Must be at least 0.1")]
    public float initialFallSpeed;
    [Tooltip("Value between 0.01 and 0.99. Represents by how much the speed increases over time")]
    public float increaseSpeedOverTime;
    public TetrisBlockView[] tetrisBlocks;
    public int[] blockWeights;

    [Header("Preview Config")] 
    public float previewRotationSpeed = 10.0f;

    [Header("Scoring Config")] 
    public int scorePerCompleteRow = 100;
    
    
    private int _score;
    private int _currentLevel = 1;
    private int _completeLayers;

    public int Score => _score; 
    public int CurrentLevel => _currentLevel;
    public int CompleteLayers => _completeLayers;

    public float CurrentFallSpeed => _currentFallSpeed;

    private Transform[,,] _theGrid;
    private float _currentFallSpeed;
    
    private void Start()
    {
        // Create the grid representing the logical 3 dimensional array that is our playfield
        _theGrid = new Transform[gridSizeX, gridSizeY, gridSizeZ];

        _currentFallSpeed = initialFallSpeed;
        
        // Null out the grid
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                for (int z = 0; z < gridSizeZ; z++)
                {
                    _theGrid[x, y, z] = null;
                }
            }
        }
    }

    private void OnValidate()
    {
        if (tetrisBlocks.Length != blockWeights.Length)
        {
            Debug.LogError("The length of tetris block list and block weights must match");
        }
        // When the grid size is changed by the inspector - notify the app that it has happened
        App.Notify(TetrisAppNotification.GridResized, this);
    }

    private void OnDrawGizmos()
    {
        initialFallSpeed = Mathf.Max(initialFallSpeed, 0.01f);
        increaseSpeedOverTime = Mathf.Clamp(increaseSpeedOverTime, 0.01f, 1f);
    }

    /// <summary>
    /// Remove the "tetrisBlock"'s child cubes from the grid
    /// </summary>
    /// <param name="tetrisBlock">The block to remove from the grid</param>
    public void RemoveBlockFromGrid(TetrisBlockView tetrisBlock)
    {
        foreach (TetrisCubeView child in tetrisBlock.ChildCubes)
        {
            // Get the child transform
            var childTransform = child.transform;
            // Save the child's position
            var childPosition = childTransform.position;
            // If the last position was inside the grid we can update the location
            if (childPosition.y < gridSizeY)
            {
                _theGrid[(int)childPosition.x, (int)childPosition.y, (int)childPosition.z] = null;
            }
        }
    }
    
    /// <summary>
    /// Update the grid with a block's child cubes' transforms
    /// </summary>
    /// <param name="tetrisBlock">The block that's location we would like to update</param>
    public void UpdateGrid(TetrisBlockView tetrisBlock)
    {
        foreach (TetrisCubeView child in tetrisBlock.ChildCubes)
        {
            // Get the child transform
            var childTransform = child.transform;
            // Save the child's position
            var childPosition = childTransform.position;
            // Update the new position
            if (childPosition.y < gridSizeY)
            {
                _theGrid[(int)childPosition.x,
                         (int)childPosition.y,
                         (int)childPosition.z] = childTransform;
            }
            
        }
    }
    
    /// <summary>
    /// Check if moving a cube's transform in a specified direction would collide with an occupied position in the grid
    /// </summary>
    /// <param name="transformToCheck"> The transform of the cube we would like to check</param>
    /// <param name="direction"> The direction in which the cube would be moved</param>
    /// <returns></returns>
    public bool IsPositionOccupied(Transform transformToCheck, Vector3 direction)
    {
        // If the position is over the playfield - it's definitely unoccupied 
        if (transformToCheck.position.y > gridSizeY)
        {
            return false;
        }

        // Save the new position
        var nextTransformPosition = transformToCheck.position + direction;
        // If the new position is over the playfield - It is definitely unoccupied
        if (nextTransformPosition.y >= gridSizeY)
        {
            return false;
        }

        // Save the current occupant of the position we would like to check
        var occupantTransform = _theGrid[(int)nextTransformPosition.x, 
                                         (int)nextTransformPosition.y, 
                                         (int)nextTransformPosition.z];
        // If the occupant is not null and the occupant's parent is not us (it is a dropped block), then the
        // position we are checking is occupied. False is returned otherwise
        return occupantTransform != null && occupantTransform.parent != transformToCheck.parent;
    }
    
    /// <summary>
    /// Check if a certain layer of the grid is full
    /// </summary>
    /// <param name="y"> The layer to be checked</param>
    /// <returns>weather or not the layer is full </returns>
    public bool IsLayerFull(int y)
    {
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int z = 0; z < gridSizeZ; z++)
            {
                if (_theGrid[x, y, z] == null)
                {
                    return false;
                }
            }
        }

        return true;
    }

    /// <summary>
    /// Delete a layer from the grid
    /// </summary>
    /// <param name="y"> The layer to be deleted </param>
    public void DeleteLayer(int y)
    {
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int z = 0; z < gridSizeZ; z++)
            {
                // Destroy the gameObject and nullify the position
                Destroy(_theGrid[x, y, z].gameObject);
                _theGrid[x, y, z] = null;
            }
        }
    }

    /// <summary>
    /// Drop all layers of the grid over a certain layer
    /// </summary>
    /// <param name="markedLayer">The layer over which we are dropping</param>
    public void DropHigherLayers(int markedLayer)
    {
        for (int y = markedLayer + 1; y < gridSizeY; y++)
        {
            for (int x = 0; x < gridSizeX; x++)
            {
                for (int z = 0; z < gridSizeZ; z++)
                {
                    if (_theGrid[x, y, z] != null)
                    {
                        // Save the transform located in [x, y, z]
                        Transform cubeTransform = _theGrid[x, y, z];
                        // Nullify the coordinate
                        _theGrid[x, y, z] = null;
                        // Save the position
                        var position = cubeTransform.position;
                        // Drop down the cube by 1
                        position += Vector3.down;
                        // Update the cube's transform with the new position
                        cubeTransform.position = position;
                        // Save the cube in the new position on the grid
                        _theGrid[(int)position.x,
                                 (int)position.y,
                                 (int)position.z] = cubeTransform;
                        
                    }
                }
            }
        }
    }

    public void AddToScore(int amount)
    {
        _score += amount;
        UpdateLevelAndSpeed();
    }

    public void AddToCompleteLayers(int amount)
    {
        _completeLayers += amount;
        AddToScore((int)Mathf.Pow(2, amount - 1) * 100 * _currentLevel);
    }

    private void UpdateLevelAndSpeed()
    {
        _currentLevel = _score / 10000 + 1;
        _currentFallSpeed = initialFallSpeed * Mathf.Pow(increaseSpeedOverTime, _currentLevel - 1);
    }
}
