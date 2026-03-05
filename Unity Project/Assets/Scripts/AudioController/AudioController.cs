using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public static AudioController Instance;

    private AudioSource audioSource;
    public AudioClip ambience;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }


    public void PlaySound()
    {
        if (audioSource.clip != null)
        {
            audioSource.Play();
        }
    }

    public void StopSound()
    {
        audioSource.Stop();
    }

    public void PlaySoundClip(AudioClip clip, float volume = 0.5f)
    {
        if (clip != null)
        {
            audioSource.PlayOneShot(clip, volume);
        }
    }
}
