using System;
using System.Collections;
using System.Collections.Generic;
using Playfield_scripts;
using Tetris_Block_Scripts;
using UnityEngine;
using UnityEngine.UI;

public class ButtonInputView : TetrisElement
{
    public static ButtonInputView Instance;

    [SerializeField] private GameObject[] rotationCanvases;
    [SerializeField] private GameObject movementCanvas;

    private Transform _attachedBlock;

    public Transform AttachedBlock
    {
        set => _attachedBlock = value;
    }

    private bool _moveCanvasDisplayed = true;

    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SetCanvasesActiveState();
    }

    void Update()
    {
        RepositionToActiveBlock();

    }

    /// <summary>
    /// Reposition the block control inputs to the Active block
    /// </summary>
    private void RepositionToActiveBlock()
    {
        if (_attachedBlock != null)
        {
            transform.position = _attachedBlock.transform.position;
        }
    }

    /// <summary>
    /// Method called by a click on one of the arrow buttons around the block
    /// </summary>
    /// <param name="direction">The direction in which the user wishes to move the block</param>
    public void MoveBlock(string direction)
    {
        if (_attachedBlock != null)
        {
            // Switch case for the given direction
            TetrisMoveDirections moveDirection = TetrisMoveDirections.Forward;
            switch (direction)
            {
                case "left":
                    moveDirection = TetrisMoveDirections.Left;
                    break;

                case "right":
                    moveDirection = TetrisMoveDirections.Right;
                    break;

                case "forward":
                    moveDirection = TetrisMoveDirections.Forward;
                    break;

                case "back":
                    moveDirection = TetrisMoveDirections.Back;
                    break;

            }
            // Register a click on one of the move buttons
            App.Notify(TetrisAppNotification.OnMoveBlockClicked,
                       this,
                       moveDirection);
        }
    }

    /// <summary>
    /// Method called by a click on one of the rotation arrows used to rotate the moving block
    /// </summary>
    /// <param name="rotation">The rotation in which to rotate the block</param>
    public void RotateBlock(string rotation)
    {
        if (_attachedBlock != null)
        {
            TetrisRotateDirection tetrisRotateDirection = TetrisRotateDirection.XNegative;
            switch (rotation)
            {
                case "XPositive":
                    tetrisRotateDirection = TetrisRotateDirection.XPositive;
                    break;

                case "XNegative":
                    tetrisRotateDirection = TetrisRotateDirection.XNegative;
                    break;

                case "YPositive":
                    tetrisRotateDirection = TetrisRotateDirection.YPositive;
                    break;

                case "YNegative":
                    tetrisRotateDirection = TetrisRotateDirection.YNegative;
                    break;

                case "ZPositive":
                    tetrisRotateDirection = TetrisRotateDirection.ZPositive;
                    break;

                case "ZNegative":
                    tetrisRotateDirection = TetrisRotateDirection.ZNegative;
                    break;
            }
            App.Notify(TetrisAppNotification.OnRotateBlockClicked,
                       this,
                       tetrisRotateDirection);
        }
    }

    /// <summary>
    /// Listener for the switch display button
    /// </summary>
    public void OnSwitchDisplayClicked()
    {
        App.Notify(TetrisAppNotification.OnSwitchDisplayClicked, this);
    }

    /// <summary>
    /// Method called by the controller to switch the display between movement arrows and rotation arrows
    /// </summary>
    public void SwitchDisplay()
    {
        _moveCanvasDisplayed = !_moveCanvasDisplayed;
        SetCanvasesActiveState();
    }

    /// <summary>
    /// Method called by the speed up button
    /// </summary>
    public void SpeedDropBlock()
    {
        App.Notify(TetrisAppNotification.OnDropClicked, this);
    }

    /// <summary>
    /// Alternate between the active state of the movement arrows and the rotation arrows
    /// </summary>
    private void SetCanvasesActiveState()
    {
        movementCanvas.SetActive(_moveCanvasDisplayed);
        foreach (var canvas in rotationCanvases)
        {
            canvas.SetActive( !_moveCanvasDisplayed);
        }
        
    }

}
