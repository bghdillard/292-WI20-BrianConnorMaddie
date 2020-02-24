using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedestrianController : MonoBehaviour
{
    public float speed = 1;
    public int direction = -1;
    bool running;

    Animator anim;
    public GameObject parentObj;
    public GameController parent;
    public GameObject squirrel;
    public List<Sprite> sprites;
    public AudioSource audioSource;
    public AudioClip clip;
    bool passed;
    int spriteNum;

    void Start()
    {
        Sprite newSprite = rc(sprites);
        gameObject.GetComponent<SpriteRenderer>().sprite = newSprite;
        audioSource = gameObject.GetComponent<AudioSource>();
        
        passed = false;
        running = true;

        spriteNum = Random.Range(0, 4);

        anim = GetComponent<Animator>();
        if (direction == -1)
        {
            if(spriteNum == 0) anim.SetTrigger("MoveDown");
            if(spriteNum == 1) anim.SetTrigger("MoveDown2");
            if(spriteNum == 2) anim.SetTrigger("MoveDown3");
            if(spriteNum == 3) anim.SetTrigger("MoveDown4");
        }else{
            if(spriteNum == 0) anim.SetTrigger("MoveUp");
            if(spriteNum == 1) anim.SetTrigger("MoveUp2");
            if(spriteNum == 2) anim.SetTrigger("MoveUp3");
            if(spriteNum == 3) anim.SetTrigger("MoveUp4");
        }
    }

    public void SetFlip()
    {
        
    }

    void Update()
    {
        if(parent.running != running){
            running = parent.running;
            if(running){
                anim.enabled = true;
            } else {
                anim.enabled = false;
            }
        }
        if(!running) return;

        transform.position += new Vector3(0, direction * speed * Time.deltaTime, 0);

        if(transform.position.x < -3){
            Destroy(gameObject);
        }
        if(transform.position.y < -4){
            Destroy(gameObject);
        }
        if(transform.position.y > 12){
            Destroy(gameObject);
        }

        
        float dist = Vector3.Distance(squirrel.transform.position, transform.position);
        if(passed == false && dist < 4){
            passed = true;
            //audioSource.Play();
        }
    }

    private Sprite rc(List<Sprite> choices){
        return choices[Random.Range(0, choices.Count)];
    }
}
