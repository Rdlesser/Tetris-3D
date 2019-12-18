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
                Vector3 planeScale = new Vector3( App.model.GridSizeX / 10f, 1.0f, App.model.GridSizeY / 10f );
                var planeTransform = transform;
                planeTransform.localScale = planeScale;
            
                // Reposition north wall
                planeTransform.position = new Vector3(App.model.GridSizeX / 2.0f, 
                                                      App.model.GridSizeY / 2.0f, 
                                                      App.model.GridSizeZ);
            }

            // Retile Material
            GetComponent<MeshRenderer>().sharedMaterial.mainTextureScale = new Vector2(App.model.GridSizeX,
                                                                                       App.model.GridSizeY);
        }
        
    }
}
