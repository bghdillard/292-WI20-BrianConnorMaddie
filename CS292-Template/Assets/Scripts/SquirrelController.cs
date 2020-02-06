using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquirrelController : MonoBehaviour
{

    GameObject Controller;
    GameController GC;
    Animator anim;
    int row;
    int col;
    int health;
    bool dead;
    Rigidbody2D body;

    // Start is called before the first frame update
    void Start()
    {
        Controller = GameObject.Find("GameController");
        GC = Controller.GetComponent<GameController>();
        anim = GetComponent<Animator>();
        row = 3;
        col = 3;
        health = 3;
        dead = false;
        body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!dead)
        {
            int newRow = row;
            int newCol = col;
            bool moveL = false;
            bool moveR = false;
            bool moveU = false;
            bool moveD = false;

            if (Input.GetKeyDown(KeyCode.A))
            {
                newCol -= 1;
                moveL = true;
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                newCol += 1;
                moveR = true;
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                newRow += 1;
                moveU = true;
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                newRow -= 1;
                moveD = true;
            }

            if (GC.GetTerrain(newRow, newCol) == 0 || GC.GetTerrain(newRow, newCol) == 2)
            {
                row = newRow;
                col = newCol;
                if (moveL)
                {
                    anim.SetTrigger("MoveLeft");
                }
                else if (moveR)
                {
                    anim.SetTrigger("MoveRight");
                }
                else if (moveU)
                {
                    anim.SetTrigger("MoveUp");
                }
                else if (moveD)
                {
                    anim.SetTrigger("MoveDown");
                }
            }
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
            anim.SetTrigger("IsDead");
            dead = true;
        }
    }
}
