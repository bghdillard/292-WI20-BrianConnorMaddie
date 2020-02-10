using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour
{
    public static int scoreVal = 0;
    public GameController gameController;
    Text score;
    private int nextUpdate=1;
    // Start is called before the first frame update
    void Start()
    {
        score = GetComponent<Text> ();
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time>=nextUpdate){
             //Debug.Log(Time.time+">="+nextUpdate);
             // Change the next update (current second+1)
             nextUpdate=Mathf.FloorToInt(Time.time)+1;
             // Call your fonction
             UpdateEverySecond();
         }

        
    }

    void UpdateEverySecond(){
        SquirrelController squirrel = gameController.Squirrel.GetComponent<SquirrelController>();
        if(squirrel == null) return;

        score.text = "Score " + scoreVal;
        if(squirrel.isDead() == false){
            scoreVal += squirrel.col * 10;
        }
    }
}
