using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Contains all data related to the app.
public class TetrisModel : TetrisElement
{
    
    // Data
    public int GridSizeX = 7;
    public int GridSizeY = 10;
    public int GridSizeZ = 7;

    private void OnValidate()
    {
        App.controller.OnGridSizeChanged();
    }

}
