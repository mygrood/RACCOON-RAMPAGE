using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{        
    public TMP_Text highscore;
    public GameObject levelSelectPanel;
    
    void Start()
    {
        highscore.text = "Highscore: "+HighScoreManager.GetHighScore();
        levelSelectPanel.SetActive(false);
    }
  
    public void StartGame()
    {
        levelSelectPanel.SetActive(true);
    }
   
    public void LoadLevel(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void BackToMenu()
    {
        levelSelectPanel.SetActive(false);
    }

    public void Exit()
    {
        Application.Quit();
        Debug.Log("EXIT");
    }

}
