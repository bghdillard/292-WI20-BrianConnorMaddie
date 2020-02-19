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

        if (hammers < 3) hammer3.SetActive(false); else hammer3.SetActive(true);
        if (hammers < 2) hammer2.SetActive(false); else hammer2.SetActive(true);
        if (hammers < 1) hammer1.SetActive(false); else hammer1.SetActive(true);
    }
}
