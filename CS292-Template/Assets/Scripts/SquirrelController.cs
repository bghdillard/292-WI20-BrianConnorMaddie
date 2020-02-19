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


    Rect top;
    Rect bottom;
    Rect left;
    Rect right;
    Texture2D controls;
    SpriteRenderer SR; 
    // Start is called before the first frame update
    void Start()
    {
        SR = gameObject.GetComponent<SpriteRenderer>();
        SR.color = Color.blue;
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
        invincible = false;
        controls = Texture2D.blackTexture;
        invinTime = 0.0f;

        /// print("Making Squirrel");
    }

    void OnAwake(){
        SR.color = Color.blue;
    }
    // Update is called once per frame
    void Update()
    {
        
        //print(SR.color);
        SR.color = Color.blue;//new Color(0f, 0f, 0f, 1f);    

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
                    

                    triggerInvin(-1);
                }
                //SpriteRenderer SR = GetComponent<SpriteRenderer>();
                //SR.color = new Color(1.0f, 1.0f, 1.0f, 0.1f);            
            } else {
                //SpriteRenderer SR = GetComponent<SpriteRenderer>();
                //SR.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);  
            }

            int newRow = row;
            int newCol = col;
            if (Input.touchCount > 0 || Input.GetMouseButtonDown(0))
            {
                Touch touch = Input.GetTouch(0);
                if (canMove && touch.phase == TouchPhase.Began)
                {

                    if (Input.GetKeyDown(KeyCode.A) || left.Contains(touch.position))
                    {
                        newCol -= 1;
                        moveL = true;
                    }
                    if (Input.GetKeyDown(KeyCode.D) || right.Contains(touch.position))
                    {
                        newCol += 1;
                        moveR = true;
                    }
                    if (Input.GetKeyDown(KeyCode.W) || top.Contains(touch.position))
                    {
                        newRow += 1;
                        moveU = true;
                    }
                    if (Input.GetKeyDown(KeyCode.S) || bottom.Contains(touch.position))
                    {
                        newRow -= 1;
                        moveD = true;
                    }
                }
            }else{
                if (canMove)
                {
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
                }
            }

            if(moveL || moveR || moveU || moveD){
                char t = GC.GetTerrain(newRow, newCol);
                //print(t);
                if (t == '-' || t == '=' || t == '<' || t == '>' || (t == 'X' && hammers > 0))
                {
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
                    if(GC.running == false) GC.running = true;
                }else{
                    audioSource.Play();
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
    

    void OnGUI()
    {
        GUI.Box(top, cd);
        GUI.Box(bottom, cu);
        GUI.Box(right, cr);
        GUI.Box(left, cl);
    }
    

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
                triggerInvin(hurtInvinTime);
            }
        }
        
    }

    public void triggerInvin(float nextTime) 
    { 
        invincible = !invincible; 
        if (invincible) 
        { 
            print("Starting Invincibility");
            //SpriteRenderer SR = GetComponent<SpriteRenderer>();
            //SR.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);

            invinTime = nextTime; 
        } 
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
