using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script to control the playing field view
public class PlayfieldView : TetrisElement
{

    [SerializeField] private GameObject bottomPlane;

    [SerializeField] private GameObject northWall;
    [SerializeField] private GameObject southWall;
    [SerializeField] private GameObject eastWall;
    [SerializeField] private GameObject westWall;

    [SerializeField] private int gridSizeX = 7;
    [SerializeField] private int gridSizeY = 10;
    [SerializeField] private int gridSizeZ = 7;
    
    [SerializeField] private Transform[] theGrid;


    private void OnDrawGizmos()
    {
        if (bottomPlane != null)
        {
            // Resize bottom plane
            Vector3 bottomPlaneScale = new Vector3((float) gridSizeX / 10, 1, (float) gridSizeZ / 10 );
            bottomPlane.transform.localScale = bottomPlaneScale;
            
            // Reposition bottom plane
            var position = transform.position;
            bottomPlane.transform.position = new Vector3(position.x + (float) gridSizeX / 2,
                                                         position.y, 
                                                         position.z + (float) gridSizeZ / 2);
            
            // Retile Material
            bottomPlane.GetComponent<MeshRenderer>().sharedMaterial.mainTextureScale =
                new Vector2(gridSizeX, gridSizeZ);
        }
    }


}
