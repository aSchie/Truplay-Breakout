using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    [SerializeField] private int scoreValue;
    public int ScoreValue { get => scoreValue;}

    public delegate void DeadBrick(Brick brick);
    public static event DeadBrick BrickHasDied;

    [SerializeField] private GameObject brickParticleSystem;


    private void OnEnable()
    {
        GameManager.ResetTheScene += ResetScene;
    }

    private void OnDisable()
    {
        GameManager.ResetTheScene -= ResetScene;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("BreakoutBall"))
        {
            GameObject particles = Instantiate(brickParticleSystem, transform.position, Quaternion.identity);
            particles.GetComponent<ParticleSystem>().Play();

            BrickHasDied?.Invoke(this);
            if(gameObject != null)
                Destroy(gameObject);

            AudioManager.Instance.PlayAudioClip("BrickBreak");
        }

    }

    private void ResetScene(bool gameOver)
    {
        if (gameObject != null)
            Destroy(gameObject, .2f);
    }

    public void SetColor(Color color)
    {
        GetComponent<SpriteRenderer>().material.color = color;
    }

    public void SetScoreValue(int scoreMultiplier)
    {
        scoreValue *= scoreMultiplier;
    }
}
