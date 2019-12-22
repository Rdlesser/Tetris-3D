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

    // Notify the controller and delegate the notification data
    // This method can easily be found because every class is a “TetrisElement” and has an “app” 
    // instance.
    public void Notify(TetrisAppNotifications eventString, Object target, params object[] data)
    {
        controller.OnNotification(eventString, target, data);
        
    }
}
