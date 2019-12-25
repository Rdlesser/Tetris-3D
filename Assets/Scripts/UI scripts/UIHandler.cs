using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A handler for the HUD (score, level etc...)
/// </summary>
public class UIHandler : TetrisElement
{

    public static UIHandler Instance;

    [SerializeField] private Text scoreText;
    [SerializeField] private Text levelText;
    [SerializeField] private Text layersText;
    [SerializeField] private GameObject gameOverWindow;
    
    
    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UpdateUI();
        gameOverWindow.SetActive(false);
    }

    /// <summary>
    /// Update the UI by reading the model's data
    /// </summary>
    public void UpdateUI()
    {
        scoreText.text = "Score: " + App.model.Score.ToString("D9");
        levelText.text = "Level: " + App.model.CurrentLevel.ToString("D4");
        layersText.text = "Layers: " + App.model.CompleteLayers.ToString("D9");
    }

    public void ShowGameOverWindow()
    {
        gameOverWindow.SetActive(true);
    }
}
