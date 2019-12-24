using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : TetrisElement
{

    public static UIHandler Instance;

    [SerializeField] private Text scoreText;
    [SerializeField] private Text levelText;
    [SerializeField] private Text layersText;
    
    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        scoreText.text = "Score: " + App.model.Score.ToString("D9");
        levelText.text = "Level: " + App.model.CurrentLevel.ToString("D4");
        layersText.text = "Layers: " + App.model.CompleteLayers.ToString("D9");
    }
}
