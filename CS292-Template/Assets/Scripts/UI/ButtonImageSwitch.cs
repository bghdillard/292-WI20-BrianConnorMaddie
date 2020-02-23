using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonImageSwitch : MonoBehaviour
{

    public Sprite SoundOn;
    public Sprite SoundOff;
    public Button but;
    public bool music;
    public GameController GC;

    void OnButtonClick()
    {
        ChangeImage();
    }

    public void ChangeImage() {
        if(music){
            GC.musicMuted = but.GetComponent<Image>().sprite == SoundOn;
        }else{
            GC.effectsMuted = but.GetComponent<Image>().sprite == SoundOn;
        }
        if (but.GetComponent<Image>().sprite == SoundOn)
        {
            but.GetComponent<Image>().sprite = SoundOff;
        }
        else
        {
            but.GetComponent<Image>().sprite = SoundOn;
        }
    }

}
