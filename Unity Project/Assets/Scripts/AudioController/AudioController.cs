using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public static AudioController Instance;

    private AudioSource audioSource;
    public AudioSource clipSource;
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
            clipSource.Play();
        }
    }

    public void StopSound()
    {
        clipSource.Stop();
    }

    public void PlaySoundClip(AudioClip clip, float volume = 0.5f, int sourceIndex = 0)
    {
        if (sourceIndex == 0)
        {
            if (clip != null && !clipSource.isPlaying)
            {
                clipSource.PlayOneShot(clip, volume);
            }
        }

        else if (sourceIndex == 1)
        {
            if (clip != null)
            {
                clipSource.PlayOneShot(clip, volume);
            }
        }
    }
}
