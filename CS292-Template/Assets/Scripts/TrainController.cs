using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainController : MonoBehaviour
{
    public float speed = 3;
    public int direction = -1;

    public GameObject parentObj;
    public GameController parent;
    public List<Sprite> sprites;
    public GameObject squirrel;
    public AudioSource audioSource;
    public AudioClip clip;
    bool passed;
    // Start is called before the first frame update
    void Start()
    {
        Sprite newSprite = rc(sprites);
        gameObject.GetComponent<SpriteRenderer>().sprite = newSprite;
        audioSource = gameObject.GetComponent<AudioSource>();
        passed = false;
    }

    public void SetFlip()
    {
        if(direction == -1){
            gameObject.GetComponent<SpriteRenderer>().flipY = true;
            //transform.localScale.x *= 1;
            //gameObject.transform.Rotate(0, 180, 0);;
        }
    }
    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(-1 * parent.landSpeed * Time.deltaTime, 0, 0);
        transform.position += new Vector3(0, direction * speed * Time.deltaTime, 0);

        if(transform.position.x < -1){
            Destroy(gameObject);
        }
        if(transform.position.y < -4){
            Destroy(gameObject);
        }
        if(transform.position.y > 12){
            Destroy(gameObject);
        }

        float dist = Vector3.Distance(squirrel.transform.position, transform.position);
        if(passed == false && dist < 3){
            passed = true;
            audioSource.Play();
        }
    }

    private Sprite rc(List<Sprite> choices){
        return choices[Random.Range(0, choices.Count)];
    }
}
