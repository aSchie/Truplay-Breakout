using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUIController : MonoBehaviour
{ 
    void startButtonPressed()
    {
        AudioManager.Instance.PlayAudioClip("ButtonClick");
        SceneManager.LoadScene("GameScene");
    }
}
