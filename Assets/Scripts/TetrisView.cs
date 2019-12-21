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
}
