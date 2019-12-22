using System;
using System.Collections;
using System.Collections.Generic;
using Playfield_scripts;
using Tetris_Block_Scripts;
using UnityEngine;

public class ButtonInputsView : TetrisElement
{
    public static ButtonInputsView Instance;

    [SerializeField] private GameObject[] rotationCanvases;
    [SerializeField] private GameObject movementCanvas;

   private TetrisBlockView _attachedBlock;

   public TetrisBlockView ActiveBlock
   {
       set => _attachedBlock = value;
   }
    // private TetrisBlockView _activeTetrisBlock;
    
    void Awake()
    {
        Instance = this;
    }
    
    void Update()
    {
        RepositionToActiveBlock();
        
    }

    private void RepositionToActiveBlock()
    {
        if (_attachedBlock != null)
        {
            transform.position = _attachedBlock.transform.position;
        }
    }

    public void MoveBlock(string direction)
    {
        if (_attachedBlock != null)
        {
            // Debug.LogError("Clicked on " + direction);
            switch (direction)
            {
                case "left":
                    _attachedBlock.MoveBlock(Vector3.left);
                break;
                
                case "right":
                    _attachedBlock.MoveBlock(Vector3.right);
                    break;
                
                case "forward":
                    _attachedBlock.MoveBlock(Vector3.forward);
                    break;
                
                case "back":
                    _attachedBlock.MoveBlock(Vector3.back);
                    break;
                
            }
        }
    }

    public void RotateBlock(string rotation)
    {
        if (_attachedBlock != null)
        {
            // Debug.LogError("Pressed " + rotation);
            switch (rotation)
            {
                case "XPositive":
                    _attachedBlock.RotateBlock(Vector3.right * 90.0f);
                    break;
                
                case "XNegative":
                    _attachedBlock.RotateBlock(Vector3.left * 90.0f);
                    break;
                
                case "YPositive":
                    _attachedBlock.RotateBlock(Vector3.up * 90.0f);
                    break;
                
                case "YNegative":
                    _attachedBlock.RotateBlock(Vector3.down * 90.0f);
                    break;
                
                case "ZPositive":
                    _attachedBlock.RotateBlock(Vector3.forward * 90.0f);
                    break;
                
                case "ZNegative":
                    _attachedBlock.RotateBlock(Vector3.back * 90.0f);
                    break;
            }
        }
    }

}
