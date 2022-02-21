using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
    public static AudioManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<AudioManager>();
                if(_instance == null)
                {
                    GameObject gameObject = new GameObject();
                    gameObject.name = "AudioManager";
                    _instance = gameObject.AddComponent<AudioManager>();
                }
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(_instance.gameObject);
        }
        else if (_instance != this)
            Destroy(gameObject);
    }

    [SerializeField] private AudioClip[] soundClipsToPlay;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource clipSource;
    [SerializeField] private AudioSource clipSource2;

    private Hashtable audioClips;

    private void Start()
    {
        audioClips = new Hashtable();

        foreach (AudioClip clip in soundClipsToPlay)
        {
            audioClips.Add(clip.name, clip);
        }

        PlayMusicClip();
    }

    public void PlayMusicClip()
    {
        if (musicSource != null)
        {
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    public void PlayAudioClip(string clipName)
    {
        if (clipName == "Congrats" || clipName == "NextLevel")
        {
            if (clipSource2 != null)
            {
                clipSource2.clip = (AudioClip)audioClips[clipName];

                if (clipSource2.clip != null)
                    clipSource2.Play();

                return;
            }
        }

        if(clipSource != null)
        {
            if (clipSource.isPlaying)
                clipSource.Stop();

            clipSource.clip = (AudioClip)audioClips[clipName];

            if(clipSource.clip != null)
                clipSource.Play();
        }
    }
}
