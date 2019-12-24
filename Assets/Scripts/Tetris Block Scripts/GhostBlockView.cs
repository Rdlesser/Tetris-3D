using System.Collections;
using System.Collections.Generic;
using Tetris_Block_Scripts;
using UnityEngine;

/// <summary>
/// A Class representing the view of the ghost block (the shadow)
/// </summary>
public class GhostBlockView : TetrisElement
{

    private TetrisBlockView _parentBlock;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(RepositionBlock());
    }

    /// <summary>
    /// Set the shadow block's image (the block creating the shadow)
    /// </summary>
    /// <param name="parent">The block creating the shadow</param>
    public void SetParentBlock(TetrisBlockView parent)
    {
        _parentBlock = parent;
    }

    /// <summary>
    /// Position the ghost block below the currently falling block
    /// </summary>
    private void PositionGhost()
    {
        var parentTetrisTransform = _parentBlock.transform;
        transform.position = parentTetrisTransform.position;
        transform.rotation = parentTetrisTransform.rotation;
    }

    /// <summary>
    /// A Coroutine to reposition the ghost block, used to reduce payload insted of Update() 
    /// </summary>
    /// <returns></returns>
    IEnumerator RepositionBlock()
    {
        while (_parentBlock.enabled)
        {
            PositionGhost();

            MoveDown();

            yield return new WaitForSeconds(0.1f);
        }
        Destroy(gameObject);
        yield return null;
    }

    /// <summary>
    /// Move the ghost block all the way to the bottom of the current position
    /// </summary>
    private void MoveDown()
    {
        int i = 0;
        while (IsValidMove(Vector3.down * i))
        {
            i++;
        }

        transform.position += Vector3.down * (i - 1);
    }
    
    /// <summary>
    /// Check if a move direction is valid
    /// </summary>
    /// <param name="direction"></param>
    /// <returns>
    /// True - when the move is valid and legal
    /// False - Otherwise
    /// </returns>
    private bool IsValidMove(Vector3 direction)
    {
        // For each child of the block (the cubes) - check if the move is valid
        foreach (TetrisCubeView child in _parentBlock.ChildCubes)
        {
            if (!child.IsValidMove(direction))
            {
                return false;
            }
        }
        return true;
    }
}
