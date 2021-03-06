﻿using UnityEngine;

namespace Playfield_scripts
{
    public class EastWallView : TetrisElement
    {
    
        private void OnDrawGizmos()
        {
            // Resize plane
            if (App != null)
            {
                Vector3 planeScale = new Vector3(App.model.gridSizeZ / 10.0f,
                                                 1.0f,
                                                 App.model.gridSizeY / 10.0f );
                var planeTransform = transform;
                planeTransform.localScale = planeScale;
            
                // Reposition  plane
                planeTransform.position = new Vector3(App.model.gridSizeX,
                                                      App.model.gridSizeY / 2.0f, 
                                                      App.model.gridSizeZ / 2.0f);
            }

            // Retile Material
            GetComponent<MeshRenderer>().sharedMaterial.mainTextureScale = new Vector2(App.model.gridSizeZ,
                                                                                       App.model.gridSizeY);
        }
        
    }
}
