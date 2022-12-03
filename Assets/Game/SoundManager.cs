using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip musicClip;
    [SerializeField] private AudioSource musicSource, effectsSource;
    public static SoundManager instance = null;

    public void PlaySound(AudioClip clip, float pitch) {
        effectsSource.pitch = Random.Range(1-pitch,1+pitch);
        effectsSource.PlayOneShot(clip);
        effectsSource.pitch = 1;
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }
    }
}
