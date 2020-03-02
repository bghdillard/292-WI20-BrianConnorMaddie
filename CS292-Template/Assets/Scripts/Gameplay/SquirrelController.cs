using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquirrelController : MonoBehaviour
{

    public GameObject Controller;
    GameController GC;
    Vector3 target;
    Animator anim;
    Rigidbody2D body;

    public float speed = 1.0f;
    public int row;
    public int col;
    public int health = 3;
    public int hammers = 0;
    public AudioSource audioSource;
    public AudioClip bump;
    public Texture2D cu;
    public Texture2D cd;
    public Texture2D cr;
    public Texture2D cl;

    public float colInvinTime;
    public float hurtInvinTime;
    float invinTime;

    bool dead;
    bool canMove;
    bool invincible;
    int totalMoves;
    bool running;

    Rect top;
    Rect bottom;
    Rect left;
    Rect right;
    Texture2D controls;
    SpriteRenderer SR; 
    // Start is called before the first frame update
    void Start()
    {
        GC = Controller.GetComponent<GameController>();
        audioSource = gameObject.GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        dead = false;
        body = GetComponent<Rigidbody2D>();
        canMove = true;
        target = transform.position;
        totalMoves = 0;
        hammers = 0;

        top = new Rect(Screen.width / 3, Screen.height * 2 / 3, Screen.width / 3, Screen.height / 3);
        bottom = new Rect(Screen.width / 3, 0, Screen.width / 3, Screen.height / 3);
        left = new Rect(0, 0, Screen.width / 3, Screen.height);
        right = new Rect(Screen.width * 2 / 3, 0, Screen.width / 3, Screen.height * 5 / 6);

        invincible = false;
        controls = Texture2D.blackTexture;
        invinTime = 0.0f;
        running = false;

        print("Making Squirrel");
    }

    void Update()
    {
        if(GC.running != running){
            running = GC.running;
            if(running){
                anim.enabled = true;
            } else {
                anim.enabled = false;
            }
        }

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

            if (invincible)
            {
                invinTime -= Time.deltaTime;
                //print(invinTime);
                if(invinTime <= 0.0f)
                {
                    print("Ending Invincibility");
                    SpriteRenderer SR = GetComponent<SpriteRenderer>();
                    SR.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);    

                    invincible = false;
                }
                        
            }

            

            int newRow = row;
            int newCol = col;
            if (canMove)
            {
                int moveMode = 1;
                if(moveMode == 0){
                    if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) || Input.GetMouseButtonDown(0))
                    {
                        Vector2 touchPos = new Vector2(-100, -100);
                        if(Input.touchCount > 0) {
                            touchPos = Input.GetTouch(0).position;
                        } else {
                            touchPos = Input.mousePosition;
                        }

                        if (left.Contains(touchPos)) moveL = true;
                        if (right.Contains(touchPos)) moveR = true;
                        if (top.Contains(touchPos)) moveU = true;
                        if (bottom.Contains(touchPos)) moveD = true;
                    }else{
                        if (Input.GetKeyDown(KeyCode.A)) moveL = true;
                        if (Input.GetKeyDown(KeyCode.D)) moveR = true;
                        if (Input.GetKeyDown(KeyCode.W)) moveU = true;
                        if (Input.GetKeyDown(KeyCode.S)) moveD = true;
                    }   
                }else{
                    moveL = GC.moveL;
                    moveR = GC.moveR;
                    moveU = GC.moveU;
                    moveD = GC.moveD;

                    GC.moveL = false;
                    GC.moveR = false;
                    GC.moveU = false;
                    GC.moveD = false;
                }
            }

            if (moveL) newCol--;
            if (moveR) newCol++;
            if (moveU) newRow++;
            if (moveD) newRow--;

            if(moveL || moveR || moveU || moveD){
                char t = GC.GetTerrain(newRow, newCol);
                //print(t);
                if (t == '-' || t == '=' || t == '<' || t == '>' || (t == 'X' && hammers > 0))
                {
                    totalMoves += 1;
                    if(totalMoves > 3) GC.RemoveOverlay();
                    
                    if(t == 'X'){
                        hammers -= 1;
                        GC.BreakRock(newCol, newRow);
                    }
                    row = newRow;
                    col = newCol;
                    bool hitStop = false;
                    int test = 0;
                    while (t == '=' && hitStop == false && test < 6)
                    {
                        test += 1;
                        if (moveL) newCol--;
                        if (moveR) newCol++;
                        if (moveU) newRow++;
                        if (moveD) newRow--;
                        t = GC.GetTerrain(newRow, newCol);
                        if (t == '-' || t == '=' || t == '<' || t == '>')
                        {
                            row = newRow;
                            col = newCol;
                            transform.position = Vector3.MoveTowards(transform.position,
                                new Vector3(col - GC.offset, row, 0), step);
                        }
                        else
                        {
                            hitStop = true;
                        }
                    }
                    if (moveL) anim.SetTrigger("MoveLeft");
                    if (moveR) anim.SetTrigger("MoveRight");
                    if (moveU) anim.SetTrigger("MoveUp");
                    if (moveD) anim.SetTrigger("MoveDown");
                    if(GC.running == false) GC.UnpauseGame();
                }else{
                    if(!GC.effectsMuted) audioSource.Play();
                }
            }
            

            if (moveU || moveD || moveL || moveR)
            {
                canMove = false;
            }

        }

        target = new Vector3(col - GC.offset, row, 0);
        transform.position = Vector3.MoveTowards(transform.position, target, step);   
    }
    

    //void OnGUI()
    //{
    //    GUI.Box(top, cd);
    //    GUI.Box(bottom, cu);
    //    GUI.Box(right, cr);
    //    GUI.Box(left, cl);
    //}
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //print(collision.gameObject.layer);
        if(collision.gameObject.layer == 11){
            CollectibleController cc = collision.gameObject.GetComponent<CollectibleController>();
            print("Picked up: " + cc.type.ToString());
            if(cc.type == 0){
                GC.scoreVal += 100 * (int)(GC.offset);
            }else if(cc.type == 1){
                if(health < 3) ChangeHealth(1);
            }else if(cc.type == 2){
                triggerInvin(colInvinTime);
            }else{
                if(hammers < 3) hammers += 1;
            }
            Destroy(collision.gameObject);
        }else{
            if (!invincible)
            {
                print("Hurt");
                ChangeHealth(-1);
                if(!dead) triggerInvin(hurtInvinTime);
            }
        }
        
    }

    public void triggerInvin(float nextTime) 
    { 
        invincible = true; 
        print("Starting Invincibility");
        SpriteRenderer SR = GetComponent<SpriteRenderer>();
        SR.color = new Color(1.0f, 1.0f, 1.0f, 0.4f);  

        invinTime = nextTime; 
    } 

    public void ChangeHealth(int change)
    {
        health += change;
        if(health == 0)
        {
            anim.SetTrigger("IsDead");
            dead = true;
            GC.SaveScore(GC.scoreVal, GC.runTime);
        }
    }

    private void offScreen()
    {
        if (!dead)
        {   
            ChangeHealth(-1);
            row = 3;
            int i = 7;
            while (true && i < 14)
            {
                i += 1;
                if (GC.GetTerrain(row, col + i) == '=' || GC.GetTerrain(row, col + i) == '-') break;
            }
            if(i == 14){
                i = 7;
                while (true)
                {
                    i += 1;
                    if (GC.GetTerrain(row, col + 1) == '#' || GC.GetTerrain(row, col + i) == 'X') break;
                }
                GC.BreakRock(row, col + i);
            }

            col = col + i;
            transform.position = new Vector3(col - GC.offset, row, 0);
        }
    }

    public bool isDead(){
        return dead;
    }
}
