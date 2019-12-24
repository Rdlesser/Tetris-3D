using System.Collections;
using System.Collections.Generic;
using Tetris_Block_Scripts;
using UnityEngine;

public class GhostBlockView : TetrisElement
{

    private TetrisBlockView _parentBlock;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(RepositionBlock());
    }

    public void SetParentBlock(TetrisBlockView parent)
    {
        _parentBlock = parent;
    }

    private void PositionGhost()
    {
        var parentTetrisTransform = _parentBlock.transform;
        transform.position = parentTetrisTransform.position;
        transform.rotation = parentTetrisTransform.rotation;
    }

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

    private void MoveDown()
    {
        int i = 0;
        while (IsValidMove(Vector3.down * i))
        {
            i++;
        }

        transform.position += Vector3.down * (i - 1);
    }
    
    private bool IsValidMove(Vector3 direction)
    {
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
