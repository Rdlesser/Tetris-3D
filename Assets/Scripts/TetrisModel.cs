using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

// Contains all data related to the app.
public class TetrisModel : TetrisElement
{

    // Data
    public int GridSizeX = 7;
    public int GridSizeY = 10;
    public int GridSizeZ = 7;
    
    private Transform[,,] _theGrid;
    
    private void Start()
    {
        _theGrid = new Transform[GridSizeX, GridSizeY, GridSizeZ];
        
        // Null out the grid
        for (int x = 0; x < GridSizeX; x++)
        {
            for (int y = 0; y < GridSizeY; y++)
            {
                for (int z = 0; z < GridSizeZ; z++)
                {
                    _theGrid[x, y, z] = null;
                }
            }
        }
    }

    private void OnValidate()
    {
        App.Notify(TetrisNotifications.GridResize, this);
    }

    // public Vector3 RoundUpVector(Vector3 toRoundUp)
    // {
    //     return new Vector3(Mathf.RoundToInt(toRoundUp.x),
    //                        Mathf.RoundToInt(toRoundUp.y),
    //                        Mathf.RoundToInt(toRoundUp.z));
    // }


    public void UpdateGrid(TetrisBlockView tetrisBlock)
    {
        foreach (TetrisCubeView child in tetrisBlock.ChildCubes)
        {
            // Delete former locations of the cubes inside the grid
            var childTransform = child.transform;
            var childPosition = childTransform.position;
            Vector3 lastPosition = childPosition + Vector3.up;
            if (lastPosition.y < GridSizeY)
            {
                _theGrid[(int)lastPosition.x, (int)lastPosition.y, (int)lastPosition.z] = null;
            }

            if (childPosition.y <= GridSizeY)
            {
                _theGrid[(int) childPosition.x, (int)childPosition.y, (int)childPosition.z] = childTransform;
            }
            
            
        }
    }

    public bool IsPositionOccupied(Transform transform)
    {
        if (transform.position.y > GridSizeY)
        {
            return false;
        }

        Vector3 nextTransformPosition = transform.position + Vector3.down;
        if (nextTransformPosition.y >= GridSizeY)
        {
            return false;
        }

        Transform occupentTransform = _theGrid[(int) nextTransformPosition.x,
                                               (int) nextTransformPosition.y, 
                                               (int) nextTransformPosition.z];
        return occupentTransform != null && occupentTransform.parent != transform.parent;
    }
    
}
