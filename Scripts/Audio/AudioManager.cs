using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource backgroundMusicSource;
    public AudioSource planetSoundSource;

    [Header("Settings")]
    public float fadeSpeed = 1.5f;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        backgroundMusicSource.loop = true;
        backgroundMusicSource.volume = 0.8f;
        backgroundMusicSource.Play();
    }

    public void PlayPlanetSound(AudioClip clip, float volume)
    {
        StartCoroutine(FadeMusicOut());
        planetSoundSource.clip = clip;
        planetSoundSource.volume = volume;
        planetSoundSource.loop = true;
        planetSoundSource.Play();
    }

    public void FadeBackgroundMusic()
    {
        planetSoundSource.Stop();
        StartCoroutine(FadeMusicIn());
    }

    IEnumerator FadeMusicOut()
    {
        while (backgroundMusicSource.volume > 0.05f)
        {
            backgroundMusicSource.volume -= fadeSpeed * Time.deltaTime;
            yield return null;
        }
        backgroundMusicSource.volume = 0;
    }

    IEnumerator FadeMusicIn()
    {
        while (backgroundMusicSource.volume < 0.8f)
        {
            backgroundMusicSource.volume += fadeSpeed * Time.deltaTime;
            yield return null;
        }
        backgroundMusicSource.volume = 0.8f;
    }
}