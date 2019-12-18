using System.Collections;
using System.Collections.Generic;
using Playfield_scripts;
using UnityEngine;

// Controls the app workflow.
public class TetrisController : TetrisElement
{

    public TetrisView TetrisView;

    // Handles the win condition
    public void OnGameOver()
    {
        Debug.Log("Game Over!!!");
    }

    public void OnGridSizeChanged()
    {
        TetrisView.OnGridSizeChanged();
    }
}