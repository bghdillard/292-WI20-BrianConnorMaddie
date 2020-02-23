using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainController : MonoBehaviour
{
    public float speed = 3;
    public int direction = -1;
    bool running;

    public GameObject parentObj;
    public GameController parent;
    public List<Sprite> sprites;
    public GameObject squirrel;
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

        running = true;
    }

    public void SetFlip()
    {
        if(direction == -1){
            gameObject.transform.Rotate(0.0f, 0.0f, 180.0f, Space.Self);
        }
    }

    void Update()
    {
        if(parent.running != running){
            running = parent.running;
            GameObject smokeTrail = gameObject.transform.Find("SmokeTrail").gameObject;
            ParticleSystem particleSystem = smokeTrail.GetComponent<ParticleSystem>();
            if(running){
                particleSystem.Play();
            } else {
                particleSystem.Pause();
            }
        }
        if(!running) return;

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
            if(!parent.effectsMuted) audioSource.Play();
        }
    }

    private Sprite rc(List<Sprite> choices){
        return choices[Random.Range(0, choices.Count)];
    }
}
