using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleController : MonoBehaviour
{
    public GameController parent;
    public List<Sprite> sprites;
    public int type = -1;

    void Start()
    {
        Sprite newSprite = sprites[type];
        gameObject.GetComponent<SpriteRenderer>().sprite = newSprite;
    }

    void Update()
    {
        
    }

    private Sprite rc(List<Sprite> choices){
        return choices[Random.Range(0, choices.Count)];
    }
}
