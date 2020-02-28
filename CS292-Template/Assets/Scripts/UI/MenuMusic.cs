using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuMusic : MonoBehaviour
{

    public AudioSource music;
    public ButtonImageSwitch button;


    void Start()
    {
        music.Play();
    }

    void Update()
    {
        if (button.musicBool == true) {
            music.mute = false;
        }
        if (button.musicBool == false) {
            music.mute = true;
        }
    }
}
