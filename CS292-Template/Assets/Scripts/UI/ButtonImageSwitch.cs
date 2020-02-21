using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonImageSwitch : MonoBehaviour
{

    public Sprite SoundOn;
    public Sprite SoundOff;
    public Button but;
    public GameObject music;

    void OnButtonClick()
    {
        ChangeImage();
    }

    public void ChangeImage() {
        if (but.GetComponent<Image>().sprite == SoundOn)
        {
            but.GetComponent<Image>().sprite = SoundOff;
            music.SetActive(false);
        }
        else
        {
            but.GetComponent<Image>().sprite = SoundOn;
            music.SetActive(true);
        }
    }

}
