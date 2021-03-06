﻿using UnityEngine;

namespace Playfield_scripts
{
    public class SouthWallView : TetrisElement
    {
    
        private void OnDrawGizmos()
        {
            // Resize plane
            if (App != null)
            {
                Vector3 planeScale = new Vector3(App.model.gridSizeX / 10.0f, 1.0f, App.model.gridSizeY / 10.0f );
                var planeTransform = transform;
                planeTransform.localScale = planeScale;
            
                // Reposition plane
                planeTransform.position = new Vector3(App.model.gridSizeX / 2.0f, 
                                                      App.model.gridSizeY / 2.0f, 
                                                      0.0f);
            }

            // Retile Material
            GetComponent<MeshRenderer>().sharedMaterial.mainTextureScale = new Vector2(App.model.gridSizeX,
                                                                                       App.model.gridSizeY);
        }
        
    }
}
