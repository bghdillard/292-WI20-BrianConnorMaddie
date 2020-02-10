using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menus : MonoBehaviour
{

    public GameObject title, settings, credits, gameUI, gameplay, GameOver;


    void Start()
    {
        titleScreen();
    }

    void Update()
    {
        /*GameObject HP = GameObject.Find("HealthController");
        HealthController h = HP.GetComponent<HealthController>();
        if (h.health == 0)
        {
            gameover();
        }*/
    }

    void game() {
        title.SetActive(false);
        settings.SetActive(false);
        credits.SetActive(false);
        gameplay.SetActive(true);
        gameUI.SetActive(true);
        GameOver.SetActive(false);

    }

    void titleScreen() {
        title.SetActive(true);
        settings.SetActive(false);
        credits.SetActive(false);
        gameplay.SetActive(false);
        gameUI.SetActive(false);
        GameOver.SetActive(false);
    }

    void settingsScreen() {
        title.SetActive(false);
        settings.SetActive(true);
        credits.SetActive(false);
        gameplay.SetActive(false);
        gameUI.SetActive(false);
        GameOver.SetActive(false);

    }

    void creditsScreen() {
        title.SetActive(false);
        settings.SetActive(false);
        credits.SetActive(true);
        gameplay.SetActive(false);
        gameUI.SetActive(false);
        GameOver.SetActive(false);
    }

    void gameover() {
        

        title.SetActive(false);
        settings.SetActive(false);
        credits.SetActive(false);
        gameplay.SetActive(true);
        gameUI.SetActive(true);
        GameOver.SetActive(true);
    }


}
