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
    public TetrisBlockView[] tetrisBlocks;
    
    private Transform[,,] _theGrid;
    
    private void Start()
    {
        // Create the grid representing the logical 3 dimensional array that is our playfield
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
        // When the grid size is changed by the inspector - notify the app that it has happened
        App.Notify(TetrisAppNotification.GridResized, this);
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
}
