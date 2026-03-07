using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Music Clips")]
    public AudioClip dayAmbience;
    public AudioClip dayEnd;
    public AudioClip winTone;
    public AudioClip loseTone;
    public AudioClip alertTone;

    public float musicVolume = 0.5f;
    public float fadeDuration = 1.5f;

    private AudioSource musicSource;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.loop = true;
        musicSource.volume = musicVolume;
    }

    // called when the game starts
    public void PlayDayAmbience()
    {
        PlayMusic(dayAmbience, true);
    }

    // called when the day ends
    public void PlayDayEnd()
    {
        PlayMusic(dayEnd, false);
    }

    public void PlayWinMusic()
    {
        PlayMusic(winTone, false);
    }

    public void PlayLoseMusic()
    {
        PlayMusic(loseTone, false);
    }

    // alert plays on top of the music so it doesnt cut it off
    public void PlayAlertTone()
    {
        if (alertTone != null)
        {
            musicSource.PlayOneShot(alertTone, musicVolume);
        }
    }

    void PlayMusic(AudioClip clip, bool loop)
    {
        if (clip == null) return;

        // stop any fade thats already happening before starting a new one
        StopAllCoroutines();
        StartCoroutine(FadeAndSwitch(clip, loop));
    }

    // fades out the old music then fades in the new track
    IEnumerator FadeAndSwitch(AudioClip newClip, bool shouldLoop)
    {
        // fade out
        float startVolume = musicSource.volume;
        float t = 0;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(startVolume, 0f, t / fadeDuration);
            yield return null;
        }

        // swap to the new clip
        musicSource.Stop();
        musicSource.clip = newClip;
        musicSource.loop = shouldLoop;
        musicSource.Play();

        // fade back in
        t = 0;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(0f, musicVolume, t / fadeDuration);
            yield return null;
        }

        musicSource.volume = musicVolume;
    }

    public void StopMusic()
    {
        StopAllCoroutines();
        musicSource.Stop();
    }

    public void PauseMusic()
    {
        musicSource.Pause();
    }

    public void ResumeMusic()
    {
        musicSource.UnPause();
    }

    public void SetVolume(float vol)
    {
        musicVolume = vol;
        musicSource.volume = vol;
    }
}
