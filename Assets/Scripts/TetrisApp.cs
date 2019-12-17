using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Base class for all elements in this application.
public class TetrisElement : MonoBehaviour
{
    // Gives access to the application and all instances.
    public TetrisApp App => FindObjectOfType<TetrisApp>();
}

public class TetrisApp : MonoBehaviour
{
    // Reference to the root instances of the MVC.
    public TetrisModel model;
    public TetrisView view;
    public TetrisController controller;

    // Init things here
    void Start() { }
}
