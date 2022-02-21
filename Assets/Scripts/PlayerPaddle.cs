using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPaddle : MonoBehaviour
{
    [SerializeField] private float playerPaddleSpeed;
    [SerializeField] private GameObject breakoutBallPrefab;
    [SerializeField] private Vector3 ballPositionOnPaddle;
    private BreakoutBall breakoutBallCreated;

    private void OnEnable()
    {
        BreakoutBall.BallHasDied += BallHasDied;
        GameManager.ResetTheScene += ResetScene;
    }

    private void OnDisable()
    {
        BreakoutBall.BallHasDied -= BallHasDied;
        GameManager.ResetTheScene -= ResetScene;
    }

    private void Start()
    {
        CreateBall();
    }

    private void CreateBall()
    {
        Vector3 tempBallPositionOnPaddle = ballPositionOnPaddle;
        tempBallPositionOnPaddle += transform.position;
        GameObject tempBall = Instantiate(breakoutBallPrefab, tempBallPositionOnPaddle, Quaternion.identity, transform);

        breakoutBallCreated = tempBall.GetComponent<BreakoutBall>();
    }

    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        DoMovement(Input.GetAxis("Horizontal"));

        if (Input.GetButtonDown("Jump"))
            ShootBall();
    }

    private void ShootBall()
    {
        breakoutBallCreated.transform.parent = null;

        breakoutBallCreated.ShootBallFromPaddle();
    }

    private void DoMovement(float direction)
    {
        if (direction < 0)
            DoMovementLeft();
        else if (direction > 0)
            DoMovementRight();
    }

    private void DoMovementRight()
    {
        transform.Translate(Vector3.right * playerPaddleSpeed);
    }

    private void DoMovementLeft()
    {
        transform.Translate(Vector3.left * playerPaddleSpeed);
    }

    private void BallHasDied()
    {
        if(breakoutBallCreated?.transform.parent == null)
            CreateBall();
    }

    private void ResetScene(bool gameOver)
    {
        if(!gameOver)
            CreateBall();
    }
}
