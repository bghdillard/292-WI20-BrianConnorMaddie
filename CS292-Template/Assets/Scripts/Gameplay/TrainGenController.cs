using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.ParticleSystem;

public class TrainGenController : MonoBehaviour
{
    public int cooldown = 100;
    public TrainController trainPrefab;

    public GameController parent;
    public GameObject squirrel;
    public int direction;
    void Start()
    {
        cooldown = Random.Range(0, 70);
    }

    void FixedUpdate()
    {
        if(!parent.running) return;
        
        if(cooldown <= 0){
            TrainController tc = Instantiate(trainPrefab, transform.position, Quaternion.identity);
            tc.transform.parent = parent.terrainObject.transform;
            tc.parent = parent;
            tc.direction = direction;
            tc.squirrel = squirrel;
            tc.SetFlip();

            cooldown = 210 - Random.Range(0, 70);
        }
        cooldown -= 1;

        if(transform.position.x < -1){
            Destroy(gameObject);
        }
    }
}
