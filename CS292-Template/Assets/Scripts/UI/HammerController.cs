using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerController : MonoBehaviour
{
    public GameController gameController;
    public GameObject hammer1, hammer2, hammer3;
    int hammers;
    void Start()
    {
        hammer1.SetActive(true);
        hammer2.SetActive(true);
        hammer3.SetActive(true);
    }

    void Update()
    {
        if(gameController.Squirrel == null) return;
        SquirrelController squirrel = gameController.Squirrel.GetComponent<SquirrelController>();
        if (squirrel == null) return;
        
        hammers = squirrel.hammers;

        hammer1.GetComponent<Animator>().SetBool("There", hammers > 0);
        hammer2.GetComponent<Animator>().SetBool("There", hammers > 1);
        hammer3.GetComponent<Animator>().SetBool("There", hammers > 2);

        
    }
}
