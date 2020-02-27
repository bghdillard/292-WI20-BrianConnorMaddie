using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{

    public GameController gameController;
    public GameObject heart1, heart2, heart3, gameOver;
    int health;

    Animator anim;

    void Start()
    {
        //begining lives
        health = 3;
        gameOver.SetActive(false);
        heart1.SetActive(true);
        heart2.SetActive(true);
        heart3.SetActive(true);

        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(gameController.Squirrel == null) return;
        SquirrelController squirrel = gameController.Squirrel.GetComponent<SquirrelController>();
        if (squirrel == null) return; 

        health = squirrel.health;

        heart1.GetComponent<Animator>().SetBool("There", health > 0);
        heart2.GetComponent<Animator>().SetBool("There", health > 1);
        heart3.GetComponent<Animator>().SetBool("There", health > 2);

        if (health == 0) gameOver.SetActive(true);
    }
}
