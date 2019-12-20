using Playfield_scripts;
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

    public bool IsPositionInsidePlayfield(Vector3 position)
    {
        if (playfieldView != null)
        {
            return playfieldView.IsPositionInsidePlayfield(position);
        }
        
        Debug.LogError("No playfield set in TetrisView");
        return false;
    }
}
