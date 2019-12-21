using System;
using UnityEngine;

namespace Playfield_scripts
{
    public class BottomPlaneView : TetrisElement
    {

        private void OnDrawGizmos()
        {
            // Resize bottom plane
            Vector3 planeScale = new Vector3(App.model.gridSizeX / 10.0f,
                                             1,
                                             App.model.gridSizeZ / 10.0f );
            var planeTransform = transform;
            planeTransform.localScale = planeScale;
            
            // Reposition bottom plane
            planeTransform.position = new Vector3(App.model.gridSizeX / 2.0f,
                                                  0, 
                                                  App.model.gridSizeZ / 2.0f);
            
            // Retile Material
            GetComponent<MeshRenderer>().sharedMaterial.mainTextureScale = new Vector2(App.model.gridSizeX,
                                                                                       App.model.gridSizeZ);
        }
        
    }
}
