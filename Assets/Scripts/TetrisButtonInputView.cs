using System;
using System.Collections;
using System.Collections.Generic;
using Playfield_scripts;
using Tetris_Block_Scripts;
using UnityEngine;

public class TetrisButtonInputView : TetrisElement
{
    public static TetrisButtonInputView Instance;

    [SerializeField] private GameObject[] rotationCanvases;
    [SerializeField] private GameObject movementCanvas;

    private Transform _attachedBlock;

    public Transform AttachedBlock
    {
        set => _attachedBlock = value;
    }

    private bool _moveCanvasDisplayed = true;

    // private TetrisBlockView _activeTetrisBlock;

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
                    App.Notify(TetrisAppNotification.OnMoveBlockClicked,
                               this,
                               TetrisMoveDirections.Left);
                    // _attachedBlock.MoveBlock(Vector3.left);
                    break;

                case "right":
                    App.Notify(TetrisAppNotification.OnMoveBlockClicked,
                               this,
                               TetrisMoveDirections.Right);
                    // _attachedBlock.MoveBlock(Vector3.right);
                    break;

                case "forward":
                    App.Notify(TetrisAppNotification.OnMoveBlockClicked,
                               this,
                               TetrisMoveDirections.Forward);
                    // _attachedBlock.MoveBlock(Vector3.forward);
                    break;

                case "back":
                    App.Notify(TetrisAppNotification.OnMoveBlockClicked,
                               this,
                               TetrisMoveDirections.Back);
                    // _attachedBlock.MoveBlock(Vector3.back);
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
                    App.Notify(TetrisAppNotification.OnRotateBlockClicked,
                               this,
                               TetrisRotateDirection.XPositive);
                    // _attachedBlock.RotateBlock(Vector3.right * 90.0f);
                    break;

                case "XNegative":
                    App.Notify(TetrisAppNotification.OnRotateBlockClicked,
                               this,
                               TetrisRotateDirection.XNegative);
                    // _attachedBlock.RotateBlock(Vector3.left * 90.0f);
                    break;

                case "YPositive":
                    App.Notify(TetrisAppNotification.OnRotateBlockClicked,
                               this,
                               TetrisRotateDirection.YPositive);
                    // _attachedBlock.RotateBlock(Vector3.up * 90.0f);
                    break;

                case "YNegative":
                    App.Notify(TetrisAppNotification.OnRotateBlockClicked,
                               this,
                               TetrisRotateDirection.YNegative);
                    // _attachedBlock.RotateBlock(Vector3.down * 90.0f);
                    break;

                case "ZPositive":
                    App.Notify(TetrisAppNotification.OnRotateBlockClicked,
                               this,
                               TetrisRotateDirection.ZPositive);
                    // _attachedBlock.RotateBlock(Vector3.forward * 90.0f);
                    break;

                case "ZNegative":
                    App.Notify(TetrisAppNotification.OnRotateBlockClicked,
                               this,
                               TetrisRotateDirection.ZNegative);
                    // _attachedBlock.RotateBlock(Vector3.back * 90.0f);
                    break;
            }
        }
    }

    public void OnSwitchDisplayClicked()
    {
        App.Notify(TetrisAppNotification.OnSwitchDisplayClicked, this);
    }

    public void SwitchDisplay()
    {
        _moveCanvasDisplayed = !_moveCanvasDisplayed;
        SetCanvasesActiveState();
    }

    public void SpeedDropBlock()
    {
        App.Notify(TetrisAppNotification.OnDropClicked, this);
    }

    private void SetCanvasesActiveState()
    {
        movementCanvas.SetActive(_moveCanvasDisplayed);
        foreach (var canvas in rotationCanvases)
        {
            canvas.SetActive( !_moveCanvasDisplayed);
        }
    }

}
