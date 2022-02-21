using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakoutBall : MonoBehaviour
{
    [SerializeField] private float ballSpeed;
    [SerializeField] private Vector3 ballDirection;

    private bool isAlive;

    public delegate void DeadBall();
    public static event DeadBall BallHasDied;

    [SerializeField] private GameObject sparkParticlesPrefab;

    private void Awake()
    {
        isAlive = false;
    }

    private void OnEnable()
    {
        GameManager.ResetTheScene += ResetScene;
    }

    private void OnDisable()
    {
        GameManager.ResetTheScene -= ResetScene;
    }

    private void Update()
    {
        if (!isAlive)
            return;

        transform.Translate(ballDirection.normalized * ballSpeed * Time.deltaTime);
    }

    public void ShootBallFromPaddle()
    {
        if(!isAlive)
            AudioManager.Instance.PlayAudioClip("ShootBall");

        isAlive = true;
    }

    private void BounceOffCollider(Vector3 normal)
    {
        Vector3 newDirection = Vector3.Reflect(ballDirection, normal);

        ballDirection = newDirection;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 contact = collision.GetContact(0).point;
        CreateParticle(contact);

        Vector3 normal = collision.GetContact(0).normal;
        BounceOffCollider(normal);


        if (!collision.gameObject.CompareTag("Brick"))
        {
            AudioManager.Instance.PlayAudioClip("BallBounce");
        }
    }

    private void CreateParticle(Vector2 contactPoint)
    {
        Vector3 particlePosition = new Vector3(contactPoint.x, contactPoint.y, 1);

        GameObject particle = Instantiate(sparkParticlesPrefab, particlePosition, Quaternion.identity);
        Destroy(particle, 1f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "BottomBounds")
        {
            BallHasDied?.Invoke();
            if(gameObject != null)
                Destroy(gameObject, .5f);
        }
    }

    private void ResetScene(bool gameOver)
    {
        if (!gameOver)
        {
            isAlive = false;

            if(gameObject != null)
                Destroy(gameObject, .2f);
        }
    }
}
