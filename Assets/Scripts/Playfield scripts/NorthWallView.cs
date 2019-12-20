using UnityEngine;

namespace Playfield_scripts
{
    public class NorthWallView : TetrisElement
    {
    
        private void OnDrawGizmos()
        {
            // Resize north wall
            if (App != null)
            {
                Vector3 planeScale = new Vector3( App.model.gridSizeX / 10f, 1.0f, App.model.gridSizeY / 10f );
                var planeTransform = transform;
                planeTransform.localScale = planeScale;
            
                // Reposition north wall
                planeTransform.position = new Vector3(App.model.gridSizeX / 2.0f, 
                                                      App.model.gridSizeY / 2.0f, 
                                                      App.model.gridSizeZ);
            }

            // Retile Material
            GetComponent<MeshRenderer>().sharedMaterial.mainTextureScale = new Vector2(App.model.gridSizeX,
                                                                                       App.model.gridSizeY);
        }
        
    }
}
