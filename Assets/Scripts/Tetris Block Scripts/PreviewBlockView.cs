using UnityEngine;

/// <summary>
/// Class representing the blocks shown in the preview window
/// </summary>
public class PreviewBlockView : TetrisElement
{

    // The speed in which to rotate the block
    private float _rotationSpeed;
    
    private void Start()
    {
        _rotationSpeed = App.model.previewRotationSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(transform.position,
                               Vector3.up,
                               _rotationSpeed * Time.deltaTime);
    }
}
