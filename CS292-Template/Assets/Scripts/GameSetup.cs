using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSetup : MonoBehaviour
{
    public GameObject GameUIPrefab;
    public GameObject GameOverPrefab;
    public GameObject TitleScreenPrefab;
    public GameObject CreditsScreenPrefab;
    public GameObject SettingsScreenPrefab;
    public GameObject GameControllerPrefab;

    GameObject GameUI;
    GameObject GameOver;
    GameObject TitleScreen;
    GameObject CreditsScreen;
    GameObject SettingsScreen;
    GameObject GameController;

    public GameObject getGameUI(){ return GameUI; }
    public GameObject getGameOver(){ return GameOver; }
    public GameObject getTitleScreen(){ return TitleScreen; }
    public GameObject getCreditsScreen(){ return CreditsScreen; }
    public GameObject getSettingsScreen(){ return SettingsScreen; }
    public GameObject getGameController(){ return GameController; }

    void Start()
    {
        //GameUI = Instantiate(GameUIPrefab, new Vector3(0, 0, 0), Quaternion.identity).gameObject;
        //GameOver = Instantiate(GameOverPrefab, new Vector3(0, 0, 0), Quaternion.identity).gameObject;
        //TitleScreen = Instantiate(TitleScreenPrefab, new Vector3(0, 0, 0), Quaternion.identity).gameObject;
        //CreditsScreen = Instantiate(CreditsScreenPrefab, new Vector3(0, 0, 0), Quaternion.identity).gameObject;
        //SettingsScreen = Instantiate(SettingsScreenPrefab, new Vector3(0, 0, 0), Quaternion.identity).gameObject;
        //GameController = Instantiate(GameControllerPrefab, new Vector3(0, 0, 0), Quaternion.identity).gameObject;

        //GameUI.transform.parent = transform;
        //GameOver.transform.parent = transform;
        //TitleScreen.transform.parent = transform;
        //CreditsScreen.transform.parent = transform;
        //SettingsScreen.transform.parent = transform;
        //GameController.transform.parent = transform;
    }
}
