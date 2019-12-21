using System;
using Playfield_scripts;
using Tetris_Block_Scripts;
using UnityEditor;
using UnityEngine;

// Contains all views related to the app.
public class TetrisView : TetrisElement
{

    [SerializeField] private PlayfieldView playfieldView;
    
    public void OnGridSizeChanged()
    {
        SceneView.RepaintAll();
    }

    private void Update()
    {
        OnArrowKeyInput();
    }
    
    private void OnArrowKeyInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            App.Notify(TetrisNotifications.OnArrowKeyPressed, this, Vector3.left);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            App.Notify(TetrisNotifications.OnArrowKeyPressed, this, Vector3.right);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            App.Notify(TetrisNotifications.OnArrowKeyPressed, this, Vector3.forward);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            App.Notify(TetrisNotifications.OnArrowKeyPressed, this, Vector3.back);
        }
    }

    public bool IsPositionInsidePlayfield(Vector3 position)
    {
        if (playfieldView != null)
        {
            return playfieldView.IsPositionInsidePlayfield(position);
        }
        
        Debug.LogError("No playfield set in TetrisView");
        return false;
    }

    public void SpawnNewBlock(TetrisBlockView blockToInstantiate, Vector3 position)
    {
        playfieldView.SpawnNewBlock(blockToInstantiate, position);
    }


    public void MoveCurrentBlockInDirection(Vector3 moveDirection)
    {
        playfieldView.MoveCurrentBlockInDirection(moveDirection);
    }
}
