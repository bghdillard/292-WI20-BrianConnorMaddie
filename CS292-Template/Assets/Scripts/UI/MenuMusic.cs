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
        music.mute = false;
        music.loop = true;
        
    }


}
