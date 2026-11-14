using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

public class SoundLib : MonoBehaviour
{
    public List<AudioClip> soundClips;
    [SerializeField] private AudioSource audioSource;

    public void PlaySound(string soundName)
    {
        AudioClip clip = soundClips?.Find(soundClip => soundClip != null && soundClip.name == soundName);
        if (clip == null)
        {
            Debug.LogWarning($"Sound effect '{soundName}' was not found.", this);
            return;
        }

        if (!audioSource)
            audioSource = GetComponent<AudioSource>();

        if (!audioSource)
        {
            Debug.LogWarning("SoundLib requires an AudioSource to play sound effects.", this);
            return;
        }

        audioSource.PlayOneShot(clip);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
