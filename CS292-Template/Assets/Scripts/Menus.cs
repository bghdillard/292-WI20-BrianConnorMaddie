using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menus : MonoBehaviour
{

    public GameObject title, settings, credits;

    void Start()
    {
        titleScreen();
    }

    void Update()
    {
        
    }

    void titleScreen() {
        title.SetActive(true);
        settings.SetActive(false);
        credits.SetActive(false);
    }

    void settingsScreen() {
        title.SetActive(false);
        settings.SetActive(true);
        credits.SetActive(false);
    }

    void creditsScreen() {
        title.SetActive(false);
        settings.SetActive(false);
        credits.SetActive(true);
    }

}
