using System;
using System.Collections;
using System.Collections.Generic;
using Tetris_Block_Scripts;
using UnityEngine;
using Object = UnityEngine.Object;

// Contains all data related to the app.
public class TetrisModel : TetrisElement
{

    [Header("Grid Size Config")]
    // Data
    public int gridSizeX = 7;
    public int gridSizeY = 10;
    public int gridSizeZ = 7;

    [Header("Tetris Blcok Config")]
    public TetrisBlockView[] tetrisBlocks;
    
    private Transform[,,] _theGrid;
    
    private void Start()
    {
        _theGrid = new Transform[gridSizeX, gridSizeY, gridSizeZ];
        
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
        App.Notify(TetrisAppNotifications.GridResized, this);
    }
    
    public void RemoveFromGrid(TetrisBlockView tetrisBlock)
    {
        int count = 0;
        foreach (TetrisCubeView child in tetrisBlock.ChildCubes)
        {
            // Get the child transform
            var childTransform = child.transform;
            // Save the child's position
            var childPosition = childTransform.position;
            // If the last position was inside the grid we can update the location
            if (childPosition.y < gridSizeY)
            {
                // Debug.LogError(string.Format("Removing [{0},{1},{2}]",
                //                              (int) childPosition.x,
                //                              (int) childPosition.y,
                //                              (int) childPosition.z));
                // Delete former location
                _theGrid[(int)childPosition.x, (int)childPosition.y, (int)childPosition.z] = null;
                count++;
            }
        }
    }

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
                // Debug.LogError(string.Format("Adding [{0},{1},{2}]",
                //                              (int)childPosition.x, 
                //                              (int)childPosition.y, 
                //                              (int)childPosition.z));
                _theGrid[(int)childPosition.x,
                         (int)childPosition.y,
                         (int)childPosition.z] = childTransform;
            }
            
        }
    }

    public bool IsPositionOccupied(Transform transformToCheck, Vector3 direction)
    {
        if (transformToCheck.position.y > gridSizeY)
        {
            return false;
        }

        var nextTransformPosition = transformToCheck.position + direction;
        if (nextTransformPosition.y >= gridSizeY)
        {
            return false;
        }

        var occupantTransform = _theGrid[(int)nextTransformPosition.x, 
                                         (int)nextTransformPosition.y, 
                                         (int)nextTransformPosition.z];
        return occupantTransform != null && occupantTransform.parent != transformToCheck.parent;
    }
}
