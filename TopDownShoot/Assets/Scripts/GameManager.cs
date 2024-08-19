using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public AudioSource mainSound;
    public GameObject deathPanel; //экран смерти
    public GameObject pausePanel; //экран паузы

    public GameObject currentPoints; //текущие очки

    public TMP_Text endPoins; //итоговый результат
    public GameObject newRecord; //рекорд

    private PlayerController player;
    private GameObject weapon;

    public Image healthBar;
    public Image weaponBar;

    private bool isPause = false;

    void Start()
    {
        deathPanel.SetActive(false);
        pausePanel.SetActive(false);
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        weapon = GameObject.Find("Weapon");
    }

    void Update()
    {
        int score = player.score;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }

        if (player.isDead) //игра окончена
        {
            Time.timeScale = 0;
            endPoins.text = "Score:" + score;
            CheckHighScore(score); //проверка рекорда           
            deathPanel.SetActive(true);
            currentPoints.SetActive(false);
        }
        else if (!isPause)// игра не окончена, обновление счёта
        {
            Time.timeScale = 1;
            deathPanel.SetActive(false);
            currentPoints.SetActive(true);
            TMP_Text textP = currentPoints.GetComponent<TMP_Text>();
            textP.text = $"{player.score}";
            float health= (float)player.HP / (float)player.playerStats.HP;
            healthBar.fillAmount = health;
            Debug.Log(healthBar.fillAmount);
            weaponBar.sprite = weapon.GetComponent<SpriteRenderer>().sprite;


        }

    }

    //проверка и запись рекорда  
    bool CheckHighScore(int score)
    {
        int highScore = HighScoreManager.GetHighScore();
        if (highScore < score)
        {
            HighScoreManager.SaveHighScore(score);
            newRecord.SetActive(true);
            return true;
        }
        else
        {
            return false;
        }
    }

    //перезапуск игры
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    //выход из игры
    public void ExitGame()
    {
        SceneManager.LoadScene("Menu");
        Debug.Log("Exit");
    }

    public void PauseGame()
    {
        isPause = !isPause;
        if (isPause)
        {
            Time.timeScale = 0;
            Debug.Log($"pause {Time.timeScale}");
            pausePanel.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
            pausePanel.SetActive(false);
        }
    }

    public void TurnSound()
    {
        mainSound.mute = !mainSound.mute;
    }
}
