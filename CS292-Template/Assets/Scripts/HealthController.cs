using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{

    public GameObject heart1, heart2, heart3;
    public int health;

    // Start is called before the first frame update
    void Start()
    {
        //begining lives
        health = 3;
        heart1.SetActive(true);
        heart2.SetActive(true);
        heart3.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        //updating health
        /* (health > 3)
        {
            health = 3;
            heart1.SetActive(true);
            heart2.SetActive(true);
            heart3.SetActive(true);
        }
        else */
        GameObject thePlayer = GameObject.Find("SquirrelController");
        SquirrelController squirrel = thePlayer.GetComponent<SquirrelController>();

        GameObject menus = GameObject.Find("Menus");
        Menus gameOver = menus.GetComponent<Menus>();


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
            
            
        }

        if (squirrel.isDead() == true)
        {
            health -= 1;
        }

    }
}
