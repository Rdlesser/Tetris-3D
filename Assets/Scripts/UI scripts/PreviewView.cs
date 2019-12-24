using UnityEngine;

public class PreviewView : TetrisElement
{
    public static PreviewView Instance;
    private PreviewBlockView _currentActive;

    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// Show a preview of the next block to appear
    /// </summary>
    /// <param name="previewBlock">The block to show in the preview window</param>
    public void ShowPreview(PreviewBlockView previewBlock)
    {
        if (_currentActive != null)
        {
            Destroy(_currentActive.gameObject);
        }

        _currentActive = Instantiate(previewBlock, transform.position, Quaternion.identity);
    }
}
