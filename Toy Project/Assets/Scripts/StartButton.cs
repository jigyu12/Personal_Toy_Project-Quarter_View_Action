using System;
using UnityEngine;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    private SceneManager sceneManager;
    private Button startButton;
    void Start()
    {
        sceneManager = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneManager>();
        startButton = GetComponent<Button>();
        
        startButton.onClick.AddListener(sceneManager.ChangeToGameScene);
    }
}
