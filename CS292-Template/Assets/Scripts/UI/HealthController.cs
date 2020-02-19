using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{

    public GameController gameController;
    public GameObject heart1, heart2, heart3, gameOver;
    int health;

    // Start is called before the first frame update
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

        if(gameController.Squirrel == null) return;//print("hello1");
        SquirrelController squirrel = gameController.Squirrel.GetComponent<SquirrelController>();
        if (squirrel == null) return;//print("hello2");    
        

        health = squirrel.health;
        //print("health" + health);

        if (health == 3)
        {
            heart1.SetActive(true);
            heart2.SetActive(true);
            heart3.SetActive(true);
            
        }
        else if (health == 2)
        {
            heart1.SetActive(true);
            heart2.SetActive(true);
            heart3.SetActive(false);
        }
        else if (health == 1)
        {
            heart1.SetActive(true);
            heart2.SetActive(false);
            heart3.SetActive(false);
        }
        else { //if health == 0
            heart1.SetActive(false);
            heart2.SetActive(false);
            heart3.SetActive(false);
            gameOver.SetActive(true);
            
        }
    }
}
