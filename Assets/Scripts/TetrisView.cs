using Playfield_scripts;
using UnityEditor;
using UnityEngine;

// Contains all views related to the app.
public class TetrisView : TetrisElement
{
    // Reference to the Playfield View
    [SerializeField] private PlayfieldView _playfieldView;

    public void OnGridSizeChanged()
    {
        SceneView.RepaintAll();
    }
}
