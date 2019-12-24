using UnityEngine;

public class PreviewView : TetrisElement
{
    public static PreviewView Instance;
    private PreviewBlockView _currentActive;

    private void Awake()
    {
        Instance = this;
    }

    public void ShowPreview(PreviewBlockView previewBlock)
    {
        if (_currentActive != null)
        {
            Destroy(_currentActive.gameObject);
        }

        _currentActive = Instantiate(previewBlock, transform.position, Quaternion.identity);
    }
}
