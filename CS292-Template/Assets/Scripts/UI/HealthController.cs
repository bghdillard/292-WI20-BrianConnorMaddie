using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{

    public GameController gameController;
    public GameObject heart1, heart2, heart3, gameOver;
    int health;

    void Start()
    {
        //begining lives
        //health = 3;
        gameOver.SetActive(false);
        heart1.SetActive(true);
        heart2.SetActive(true);
        heart3.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if(gameController.Squirrel == null) return;
        SquirrelController squirrel = gameController.Squirrel.GetComponent<SquirrelController>();
        if (squirrel == null) return; 

        health = squirrel.health;

        if (health < 3) heart3.SetActive(false); else heart3.SetActive(true);
        if (health < 2) heart2.SetActive(false); else heart2.SetActive(true);
        if (health < 1) heart1.SetActive(false); else heart1.SetActive(true);

        if (health == 0) gameOver.SetActive(true);
    }
}
