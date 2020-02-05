using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menus : MonoBehaviour
{

    public GameObject title, settings, credits, gameUI, gameplay;


    void Start()
    {
        titleScreen();
    }

    void Update()
    {
        
    }

    void game() {
        title.SetActive(false);
        settings.SetActive(false);
        credits.SetActive(false);
        gameplay.SetActive(true);
        gameUI.SetActive(true);

    }

    void titleScreen() {
        title.SetActive(true);
        settings.SetActive(false);
        credits.SetActive(false);
        gameplay.SetActive(false);
        gameUI.SetActive(false);
    }

    void settingsScreen() {
        title.SetActive(false);
        settings.SetActive(true);
        credits.SetActive(false);
        gameplay.SetActive(false);
        gameUI.SetActive(false);

    }

    void creditsScreen() {
        title.SetActive(false);
        settings.SetActive(false);
        credits.SetActive(true);
        gameplay.SetActive(false);
        gameUI.SetActive(false);
    }


}
