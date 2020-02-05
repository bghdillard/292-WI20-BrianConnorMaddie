using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menus : MonoBehaviour
{

    public GameObject title, settings, credits, gameUI;


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
        gameUI.SetActive(true);
    }

    void titleScreen() {
        title.SetActive(true);
        settings.SetActive(false);
        credits.SetActive(false);
        gameUI.SetActive(false);
    }

    void settingsScreen() {
        title.SetActive(false);
        settings.SetActive(true);
        credits.SetActive(false);
        gameUI.SetActive(false);

    }

    void creditsScreen() {
        title.SetActive(false);
        settings.SetActive(false);
        credits.SetActive(true);
        gameUI.SetActive(false);
    }

}
