using System;
using Playfield_scripts;
using Tetris_Block_Scripts;
using UnityEditor;
using UnityEngine;

// Contains all views related to the app.
public class TetrisView : TetrisElement
{

    [SerializeField] public PlayfieldView playfieldView;
    
    public void OnGridSizeChanged()
    {
        // SceneView.RepaintAll();
    }

    private void Update()
    {
        // OnArrowKeyInput();
    }
    
    

    public bool IsPositionInsidePlayfield(Vector3 position)
    {
        if (playfieldView != null)
        {
            return playfieldView.IsPositionInsidePlayfield(position);
        }
        
        return false;
    }

    public void SpawnNewBlock(TetrisBlockView blockToInstantiate, GhostBlockView ghostBlock, Vector3 position)
    {
        playfieldView.SpawnNewBlock(blockToInstantiate, ghostBlock, position);
    }
    
    public void ShowPreview(PreviewBlockView preview)
    {
        PreviewView.Instance.ShowPreview(preview);
    }


    public void MoveCurrentBlockInDirection(Vector3 moveDirection)
    {
        playfieldView.MoveCurrentBlockInDirection(moveDirection);
    }

    public void RotateCurrentBlockInDirection(Vector3 rotationDirection)
    {
        playfieldView.RotateCurrentBlockInDirection(rotationDirection);
    }

    public TetrisBlockView GetCurrentMovingBlock()
    {
        return playfieldView.CurrentMovingBlock;
    }

    public void AttachInputToBlock()
    {
        playfieldView.AttachInputToBlock();
    }

    public void SwitchHudDisplay()
    {
        ButtonInputView.Instance.SwitchDisplay();
    }

    public void SpeedDropCurrentBlock()
    {
        playfieldView.SpeedDropCurrentBlock();
    }

    public Vector3 GetPlayfieldPosition()
    {
        return playfieldView.transform.position;
    }

    public void UpdateUI()
    {
        UIHandler.Instance.UpdateUI();
    }

    public void ShowGameOverWindow()
    {
        UIHandler.Instance.ShowGameOverWindow();
    }
}
