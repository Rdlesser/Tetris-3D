using System;
using UnityEngine;

namespace Playfield_scripts
{
    public class BottomPlaneView : TetrisElement
    {

        private void OnDrawGizmos()
        {
            // Resize bottom plane
            Vector3 planeScale = new Vector3((float) App.model.gridSizeX / 10,
                                             1,
                                             (float) App.model.gridSizeZ / 10 );
            var planeTransform = transform;
            planeTransform.localScale = planeScale;
            
            // Reposition bottom plane
            var position = planeTransform.position;
            planeTransform.position = new Vector3((float) App.model.gridSizeX / 2,
                                                  position.y, 
                                                  (float) App.model.gridSizeZ / 2);
            
            // Retile Material
            GetComponent<MeshRenderer>().sharedMaterial.mainTextureScale = new Vector2(App.model.gridSizeX,
                                                                                       App.model.gridSizeZ);
        }
        
    }
}
