﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void OnPlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void OnQuitGame()
    {
        Application.Quit();
    }
}
