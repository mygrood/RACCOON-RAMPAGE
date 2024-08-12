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
    
    void Start()
    {
        highscore.text = "Highscore: "+HighScoreManager.GetHighScore();
    }
  
    public void StartGame()
    {
        Debug.Log("start");
        SceneManager.LoadScene("Game");
    }
   
}
