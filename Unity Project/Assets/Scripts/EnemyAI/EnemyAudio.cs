using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAudio : MonoBehaviour
{
    private Enemy enemyBrain;
    
    [Range(0, 100)] public float chanceOfBabble = 15.0f;
    public AudioClip[] babbleSounds;
    public AudioClip[] huhSounds;
    public AudioClip wwtSound;
    public AudioClip scaredHuhSound;

    public float babbleTimerMax = 30.0f;
    private float babbleTimer = 0.0f;

    private bool isDistracted = false;
    private bool triggerHuh = false;

    void Awake()
    {
        // Get parent enemy script
        enemyBrain = this.GetComponentInParent<Enemy>();
    }
    
    // Set babble timer to random value up to set max
    void Start()
    {
        float setMax = babbleTimerMax;
        babbleTimerMax = Random.Range(10, setMax);
        babbleTimer = Random.Range(0, babbleTimerMax);
    }

    // Update is called once per frame
    void Update()
    {
        // Plays huh on enemy distract
        if (enemyBrain.shouldTriggerHuh)
        {
            PlayHuh();
            enemyBrain.shouldTriggerHuh = false;
        }
        
        // Try babble every max babble time
        if (babbleTimer < babbleTimerMax)
        {
            babbleTimer += Time.deltaTime;
        }
        else
        {
            TryBabble();
            babbleTimer = 0.0f;
        }
    }

    // Play random huh sound
    public void PlayHuh()
    {
        if (enemyBrain.shouldRunAway)
        {
            // 70% chance of playing the "What was that?" sound
            if (Random.Range(0, 10) <= 7)
                AudioController.Instance.PlaySoundClip(wwtSound);
            else
                AudioController.Instance.PlaySoundClip(scaredHuhSound);
        }
        else
        {
            AudioController.Instance.PlaySoundClip(huhSounds[Random.Range(0, huhSounds.Length)]);
        }
    }
    
    private void TryBabble()
    {
        // If enemy is not distracted, try babble
        if (enemyBrain.isDistracted)
        {
            return;
        }
        
        if (Random.Range(0, 100) < chanceOfBabble)
        {
            AudioController.Instance.PlaySoundClip(babbleSounds[Random.Range(0, babbleSounds.Length)], 1);
        }
    }
}
