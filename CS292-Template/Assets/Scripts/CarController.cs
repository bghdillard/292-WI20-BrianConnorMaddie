using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public float speed = 3;
    public int direction = -1;

    public GameObject parentObj;
    public GameController parent;
    public GameObject squirrel;
    public List<Sprite> sprites;
    public AudioSource audioSource;
    public AudioClip clip;
    bool passed;

    void Start()
    {
        Sprite newSprite = rc(sprites);
        gameObject.GetComponent<SpriteRenderer>().sprite = newSprite;
        audioSource = gameObject.GetComponent<AudioSource>();
        passed = false;

        GameObject smokeTrail = gameObject.transform.Find("SmokeTrail").gameObject;
        ParticleSystem particleSystem = smokeTrail.GetComponent<ParticleSystem>();

        var main = particleSystem.main;
        main.simulationSpace = ParticleSystemSimulationSpace.Custom;
        main.customSimulationSpace = parent.terrainObject.transform;
    }

    public void SetFlip()
    {
        if (direction == -1)
        {
            gameObject.transform.Rotate(0.0f, 0.0f, 180.0f, Space.Self);
            //transform.localScale.x *= 1;
            //gameObject.transform.Rotate(0, 180, 0);;
        }
    }

    void Update()
    {
        //transform.position += new Vector3(-1 * parent.landSpeed * Time.deltaTime, 0, 0);
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
        if(passed == false && dist < 4){
            passed = true;
            audioSource.Play();
        }
    }

    private Sprite rc(List<Sprite> choices){
        return choices[Random.Range(0, choices.Count)];
    }
}
