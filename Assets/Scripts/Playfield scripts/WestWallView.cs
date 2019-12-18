using UnityEngine;

namespace Playfield_scripts
{
    public class WestWallView : TetrisElement
    {
    
        private void OnDrawGizmos()
        {
            // Resize bottom plane
            if (App != null)
            {
                Vector3 planeScale = new Vector3(App.model.GridSizeZ / 10.0f,
                                                 1.0f,
                                                 App.model.GridSizeY / 10.0f );
                var planeTransform = transform;
                planeTransform.localScale = planeScale;
            
                // Reposition bottom plane
                planeTransform.position = new Vector3(0.0f,
                                                      App.model.GridSizeY / 2.0f, 
                                                      App.model.GridSizeZ / 2.0f);
            }

            // Retile Material
            GetComponent<MeshRenderer>().sharedMaterial.mainTextureScale = new Vector2(App.model.GridSizeX,
                                                                                       App.model.GridSizeY);
        }
        
    }
}
