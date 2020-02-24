using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class ClickSound : MonoBehaviour
{

    public AudioSource audioSource;
    public AudioClip audioClip;

    public void playClip()
    {
        audioSource.clip = audioClip;
        audioSource.Play();
    }

}