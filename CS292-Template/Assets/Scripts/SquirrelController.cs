using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquirrelController : MonoBehaviour
{

    GameObject Controller;
    GameController GC;
    int row;
    int col;
    int health;

    // Start is called before the first frame update
    void Start()
    {
        Controller = GameObject.Find("GameController");
        GC = Controller.GetComponent<GameController>();
        row = 3;
        col = 3;
        health = 3;
    }

    // Update is called once per frame
    void Update()
    {
        int newRow = row;
        int newCol = col;

        if(Input.GetKeyDown(KeyCode.A))
        {
            newCol -= 1;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            newCol += 1;
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            newRow += 1;
        }
        if(Input.GetKeyDown(KeyCode.S))
        {
            newRow -= 1;
        }

        if(GC.GetTerrain(newRow, newCol) == 0 || GC.GetTerrain(newRow, newCol) == 2)
        {
            row = newRow;
            col = newCol;
        }

        transform.position = new Vector3(col - GC.offset, row, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ChangeHealth(-1);
    }

    private void ChangeHealth(int change)
    {
        health += change;
        print(health);
        if(health == 0)
        {
            Destroy(gameObject);
        }
    }
}
