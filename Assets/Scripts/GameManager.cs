using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int numberOfPlayerLivesToHave;
    private int playerLives;

    [SerializeField] private int numberOfBricksToHave;
    [SerializeField] private GameObject brickPrefab;
    [SerializeField] private Color[] brickColors;
    [SerializeField] private float brickYOffset;
    private Vector2 brickPrefabScale;
    private int brickCount;

    public delegate void PlayerLives(int num);
    public static event PlayerLives NumberOfPlayerLives;

    public delegate void SceneReset(bool resetLives);
    public static event SceneReset ResetTheScene;

    public delegate void GameIsOver();
    public static event GameIsOver GameOverMethod;

    private void Awake()
    {
        playerLives = numberOfPlayerLivesToHave;
    }

    private void OnEnable()
    {
        BreakoutBall.BallHasDied += BallHasDied;
        Brick.BrickHasDied += BrickHasDied;
    }

    private void OnDisable()
    {
        BreakoutBall.BallHasDied -= BallHasDied;
        Brick.BrickHasDied -= BrickHasDied;
    }

    private void Start()
    {
        NumberOfPlayerLives?.Invoke(playerLives);

        brickPrefabScale = brickPrefab.GetComponent<SpriteRenderer>().size;

        brickCount = numberOfBricksToHave;

        SetUpBricks();
    }

    private void BallHasDied()
    {
        playerLives -= 1;

        AudioManager.Instance.PlayAudioClip("PlayerDead");

        if (playerLives <= 0)
            GameOver();
    }

    private void GameOver()
    {
        AudioManager.Instance.PlayAudioClip("GameOver");

        brickCount = numberOfBricksToHave;
        playerLives = numberOfPlayerLivesToHave;

        GameOverMethod?.Invoke();
    }

    private void RestartGame()
    {
        AudioManager.Instance.PlayAudioClip("ButtonClick");

        StartCoroutine("ResetScene", true);
    }

    private void BrickHasDied(Brick brick)
    {
        brickCount -= 1;
        if (brickCount <= 0)
        {
            NextLevel();
        }
    }

    private void NextLevel()
    {
        AudioManager.Instance.PlayAudioClip("Congrats");

        StartCoroutine("ResetScene",false);
    }

    private void QuitGame()
    {
        AudioManager.Instance.PlayAudioClip("ButtonClick");

        SceneManager.LoadScene("Main Menu");
    }

    private IEnumerator ResetScene(bool gameOver)
    {
        ResetTheScene?.Invoke(gameOver);

        yield return new WaitForSecondsRealtime(.5f);

        SetUpBricks();
    }

    private void SetUpBricks()
    {
        if (brickCount <= 0)
            brickCount = numberOfBricksToHave;

        Vector2 screenBounds = GetScreenBoundsInWorldCoords();

        int howManyBricksPerRow = HowManyBricksPerRow(screenBounds.x * 2);

        float newPrefabWidth = (screenBounds.x * 2) / howManyBricksPerRow;
        float newPrefabHeight = brickPrefabScale.y / (brickPrefabScale.x / newPrefabWidth);

        ChangeBrickPrefabSizes(new Vector2(newPrefabWidth, newPrefabHeight));

        int numberInCurrentRow = 0, numberOfRowsCompleted = 0;
        Vector3 brickPosition = new Vector3();

        for (int i = 0; i < brickCount; i++)
        {
            brickPosition.x = (-screenBounds.x + (newPrefabWidth / 2) + (newPrefabWidth * (i - (howManyBricksPerRow * numberOfRowsCompleted))));
            brickPosition.y = (screenBounds.y * brickYOffset) + (newPrefabHeight * numberOfRowsCompleted);

            CreateBrick(brickPosition, i - (numberInCurrentRow + ((howManyBricksPerRow - 1) * numberOfRowsCompleted)));

            numberInCurrentRow++;

            if(numberInCurrentRow >= howManyBricksPerRow)
            {
                numberInCurrentRow = 0;
                numberOfRowsCompleted++;
            }
        }

        ResetBrickPrefabSizes();
    }

    private Vector2 GetScreenBoundsInWorldCoords()
    {
        return Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.nearClipPlane));
    }

    private int HowManyBricksPerRow(float screenBoundsInWorldWidth )
    {
        float totalWidthOfBricks = brickCount * brickPrefabScale.x;
        int howManyRowsTotal = (int)(totalWidthOfBricks / screenBoundsInWorldWidth);

        howManyRowsTotal = howManyRowsTotal == 0 ? 1 : howManyRowsTotal;

        return (int)(totalWidthOfBricks / howManyRowsTotal);
    }

    private void CreateBrick(Vector3 position, int brickColorIndex)
    {
        Brick tempBrick = Instantiate(brickPrefab, position, Quaternion.identity).GetComponent<Brick>();

        if(brickColorIndex >=0 && brickColorIndex < brickColors.Length)
            tempBrick.SetColor(brickColors[brickColorIndex]);

        tempBrick.SetScoreValue(brickColorIndex + 1);
    }

    private void ChangeBrickPrefabSizes(Vector2 newSize)
    {
        ChangePrefabRenderSize(newSize);
        ChangePrefabColliderSize(newSize);
    }

    private void ChangePrefabRenderSize(Vector2 newSize)
    {
        brickPrefab.GetComponent<SpriteRenderer>().size = newSize;
    }

    private void ChangePrefabColliderSize(Vector2 newSize)
    {
        brickPrefab.GetComponent<BoxCollider2D>().size = newSize;
    }

    private void ResetBrickPrefabSizes()
    {
        ResetBrickPrefabRenderSize();
        ResetBrickPrefabColliderSize();
    }

    private void ResetBrickPrefabRenderSize()
    {
        brickPrefab.GetComponent<SpriteRenderer>().size = brickPrefabScale;
    }

    private void ResetBrickPrefabColliderSize()
    {
        brickPrefab.GetComponent<BoxCollider2D>().size = brickPrefabScale;
    }
}
