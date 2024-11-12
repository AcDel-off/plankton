using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip windowOpen, systemStart, error, screamerBack, heartSound;
    [SerializeField] private AudioSource screamerBackObject;
    [SerializeField] private AudioSource cameraSourse;
    private void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
    }
    public void WindowOpenSound()
    {
        audioSource.PlayOneShot(windowOpen);
    }

    public void SystemStartSound()
    {
        audioSource.PlayOneShot(systemStart);
    }

    public void ErrorSound()
    {
        audioSource.PlayOneShot(error);
    }

    public void ScreamerBackSound()
    {
        screamerBackObject.PlayOneShot(screamerBack);
    }

    public void HeartSound()
    {
        cameraSourse.PlayOneShot(heartSound);
    }
}
