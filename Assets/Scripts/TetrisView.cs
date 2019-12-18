using Playfield_scripts;
using UnityEditor;
using UnityEngine;

// Contains all views related to the app.
public class TetrisView : TetrisElement
{
    
    public void OnGridSizeChanged()
    {
        SceneView.RepaintAll();
    }
}
