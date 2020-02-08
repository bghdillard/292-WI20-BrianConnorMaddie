using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public float speed = 3;
    public int direction = -1;

    public GameObject parentObj;
    public GameController parent;
    public List<Sprite> sprites;

    void Start()
    {
        Sprite newSprite = rc(sprites);
        gameObject.GetComponent<SpriteRenderer>().sprite = newSprite;
    }

    public void SetFlip()
    {
        if (direction == -1)
        {
            gameObject.GetComponent<SpriteRenderer>().flipY = true;
            //transform.localScale.x *= 1;
            //gameObject.transform.Rotate(0, 180, 0);;
        }
    }

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
    }

    private Sprite rc(List<Sprite> choices){
        return choices[Random.Range(0, choices.Count)];
    }
}
