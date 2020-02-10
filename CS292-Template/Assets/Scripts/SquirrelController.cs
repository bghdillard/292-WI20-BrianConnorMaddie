﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquirrelController : MonoBehaviour
{

    public GameObject Controller;
    GameController GC;
    Vector3 target;
    Animator anim;
    Rigidbody2D body;
    public GameObject squirrel;

    public float speed = 1.0f;
    public int row;
    public int col;
    public int health;
    public AudioSource audioSource;
    public AudioClip bump;
    bool dead;
    bool canMove;

    Rect top;
    Rect bottom;
    Rect left;
    Rect right;

    // Start is called before the first frame update
    void Start()
    {
        //Controller = GameObject.Find("/Everything/GameController");
        GC = Controller.GetComponent<GameController>();
        audioSource = gameObject.GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        dead = false;
        body = GetComponent<Rigidbody2D>();
        canMove = true;
        target = transform.position;
        top = new Rect(150, 300, 300, 150);
        bottom = new Rect(150, 0, 300, 150);
        left = new Rect(0, 0, 150, 400);
        right = new Rect(450, 0, 200, 600);
    }

    // Update is called once per frame
    void Update()
    {
        float step = speed * Time.deltaTime;

        bool moveL = false;
        bool moveR = false;
        bool moveU = false;
        bool moveD = false;

        canMove = Vector3.Distance(transform.position, target) <= 0.001f;

        if (!dead)
        {
            if (GC.GetTerrain(row, col) == '|')
            {
                offScreen();
            }
            //if (Input.touchCount > 0 || Input.GetMouseButtonDown(0))
            //{
            //Vector3 touch = Input.mousePosition;
            if (canMove)// && touch.phase == TouchPhase.Began)
            {
                int newRow = row;
                int newCol = col;
                if (Input.GetKeyDown(KeyCode.A))// || left.Contains(touch))
                {
                    newCol -= 1;
                    moveL = true;
                }
                if (Input.GetKeyDown(KeyCode.D))// || right.Contains(touch))
                {
                    newCol += 1;
                    moveR = true;
                }
                if (Input.GetKeyDown(KeyCode.W))// || top.Contains(touch))
                {
                    newRow += 1;
                    moveU = true;
                }
                if (Input.GetKeyDown(KeyCode.S))// || bottom.Contains(touch))
                {
                    newRow -= 1;
                    moveD = true;
                }

                char t = GC.GetTerrain(newRow, newCol);
                if (t == '-' || t == '=' || t == '<' || t == '>')
                {
                    row = newRow;
                    col = newCol;
                    if (moveL)
                    {
                        while (t == '=')
                        {
                            newCol--;
                            t = GC.GetTerrain(row, newCol);
                            if (t == '-' || t == '=' || t == '<' || t == '>')
                            {
                                col = newCol;
                                transform.position = Vector3.MoveTowards(transform.position,
                                    new Vector3(col - GC.offset, row, 0), step);
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
                                transform.position = Vector3.MoveTowards(transform.position,
                                    new Vector3(col - GC.offset, row, 0), step);
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
                                transform.position = Vector3.MoveTowards(transform.position,
                                    new Vector3(col - GC.offset, row, 0), step);
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
                                transform.position = Vector3.MoveTowards(transform.position,
                                    new Vector3(col - GC.offset, row, 0), step);
                            }
                            else
                            {
                                break;
                            }
                        }
                        anim.SetTrigger("MoveDown");
                    }
                }
                else
                {
                    audioSource.Play();
                }
                if (moveU || moveD || moveL || moveR)
                {
                    canMove = false;
                }
                //}
            }
        }
        target = new Vector3(col - GC.offset, row, 0);
            transform.position = Vector3.MoveTowards(transform.position, 
                target, step);   
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ChangeHealth(-1);
    }

    public void ChangeHealth(int change)
    {
        health += change;
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
            transform.position = new Vector3(transform.position.x + i, transform.position.y, 0);
            col = col + i;
        }
    }

    public bool isDead(){
        return dead;
    }

}
