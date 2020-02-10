using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquirrelController : MonoBehaviour
{

    GameObject Controller;
    GameController GC;
    Animator anim;
    public GameObject squirrel;

    public int row;
    public int col;
    public int health;
    bool dead;

    Rigidbody2D body;

    // Start is called before the first frame update
    void Start()
    {
        Controller = GameObject.Find("GameController");
        GC = Controller.GetComponent<GameController>();
        anim = GetComponent<Animator>();
        dead = false;
        body = GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    void Update()
    {


        if (!dead)
        {
            if (GC.GetTerrain(row,col) == '|')
            {
                offScreen();
            }
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

            char t = GC.GetTerrain(newRow, newCol);
            if(t == '-' || t == '=' || t == '<' || t == '>')
            {
                row = newRow;
                col = newCol;
                if (moveL)
                {
                    while(t == '=')
                    {
                        newCol--;
                        t = GC.GetTerrain(row, newCol);
                        if (t == '-' || t == '=' || t == '<' || t == '>')
                        {
                            col = newCol;
                            transform.position = new Vector3(col - GC.offset, row, 0);
                        }
                        else
                        {
                            break;
                        }
                    }
                    anim.SetTrigger("MoveLeft");
                }
                else if (moveR)
                {
                    while (t == '=')
                    {
                        newCol++;
                        t = GC.GetTerrain(row, newCol);
                        if (t == '-' || t == '=' || t == '<' || t == '>')
                        {
                            col = newCol;
                            transform.position = new Vector3(col - GC.offset, row, 0);
                        }
                        else
                        {
                            break;
                        }
                    }
                    anim.SetTrigger("MoveRight");
                }
                else if (moveU)
                {
                    while (t == '=')
                    {
                        newRow++;
                        t = GC.GetTerrain(newRow, col);
                        if (t == '-' || t == '=' || t == '<' || t == '>')
                        {
                            row = newRow;
                            transform.position = new Vector3(col - GC.offset, row, 0);
                        }
                        else
                        {
                            break;
                        }
                    }
                    anim.SetTrigger("MoveUp");
                }
                else if (moveD)
                {
                    while (t == '=')
                    {
                        newRow--;
                        t = GC.GetTerrain(newRow, col);
                        if (t == '-' || t == '=' || t == '<' || t == '>')
                        {
                            row = newRow;
                            transform.position = new Vector3(col - GC.offset, row, 0);
                        }
                        else
                        {
                            break;
                        }
                    }
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

    public void ChangeHealth(int change)
    {
        health += change;
        print(health);
        if(health == 0)
        {
            anim.SetTrigger("IsDead");
            dead = true;
        }
    }

    private void offScreen()
    {
        ChangeHealth(-1);
        if (!dead)
        {
            int i = 3;
            while (true)
            {
                if (GC.GetTerrain(row, col + i) == '=' || GC.GetTerrain(row, col + i) == '-')
                {
                    break;
                }
                i++;
            }
            GameObject other = Instantiate(squirrel, new Vector3(transform.position.x + i,
                transform.position.y, 0), Quaternion.identity).gameObject;
            SquirrelController s = other.GetComponent<SquirrelController>();
            s.col = col + i;
            s.row = row;
            s.health = health;
            Destroy(gameObject);
        }
    }

    public bool isDead(){
        return dead;
    }

}
