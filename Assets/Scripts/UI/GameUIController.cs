using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUIController : MonoBehaviour
{
    private int playerScore;
    [SerializeField] private Text scoreText;
    [SerializeField] private int scoreTextOffsetX;
    [SerializeField] private int scoreTextOffsetY;

    private int playerLives;
    private int defaultPlayerLives;
    [SerializeField] private Text livesText;
    [SerializeField] private int livesTextOffsetX;
    [SerializeField] private int livesTextOffsetY;

    [SerializeField] private GameObject gameOverPanel;

    private void Awake()
    {
        gameOverPanel.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        BreakoutBall.BallHasDied += BallHasDied;
        GameManager.NumberOfPlayerLives += SetNumberOfPlayerLives;
        Brick.BrickHasDied += BrickHasDied;
        GameManager.ResetTheScene += ResetScene;
        GameManager.GameOverMethod += GameIsOver;
    }

    private void OnDisable()
    {
        BreakoutBall.BallHasDied -= BallHasDied;
        GameManager.NumberOfPlayerLives -= SetNumberOfPlayerLives;
        Brick.BrickHasDied -= BrickHasDied;
        GameManager.ResetTheScene -= ResetScene;
        GameManager.GameOverMethod -= GameIsOver;
    }

    private void Start()
    {
        playerScore = 0;
        playerLives = 0;
    }

    private void SetNumberOfPlayerLives(int num)
    {
        playerLives = num;
        defaultPlayerLives = num;
    }

    private void Update()
    {
        HandleDisplay();
    }

    private void HandleDisplay()
    {
        DisplayPlayerScore();
        DisplayPlayerLives();
    }

    private void DisplayPlayerScore()
    {
        scoreText.text = "Score: " + playerScore;
    }

    private void DisplayPlayerLives()
    {
        livesText.text = "Lives: " + playerLives;
    }

    private void BallHasDied()
    {
        playerLives -= 1;
    }

    private void BrickHasDied(Brick brick)
    {
        UpdateScore(brick.ScoreValue);
    }

    private void GameIsOver()
    {
        gameOverPanel.gameObject.SetActive(true);
    }

    private void ResetScene(bool gameOver)
    {
        if(gameOver)
        {
            playerLives = defaultPlayerLives;
            playerScore = 0;
        }

        gameOverPanel.gameObject.SetActive(false);
    }

    private void UpdateScore(int score)
    {
        playerScore += score;
    }
}
