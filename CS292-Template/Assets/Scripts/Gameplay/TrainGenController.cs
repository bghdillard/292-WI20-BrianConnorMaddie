using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.ParticleSystem;

public class TrainGenController : MonoBehaviour
{
    public int cooldown = 100;
    public TrainController trainPrefab;
    //public SmokeController smokePrefab;

    public GameController parent;
    public GameObject squirrel;
    public int direction;
    void Start()
    {
        //GameObject smokeTrail = gameObject.find("SmokeTrail");
        //ParticleSystem PS = smokeTrail.GetComponent<ParticleSystem>();

        //PS.simulationSpace = parent.gameObject.transform;

        cooldown = Random.Range(0, 70);
        //parent = parentObj.GetComponent<GameController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //transform.position += new Vector3(-1 * parent.landSpeed * Time.deltaTime, 0, 0);
        if(cooldown <= 0){
            TrainController tc = Instantiate(trainPrefab, transform.position, Quaternion.identity);
            tc.transform.parent = parent.terrainObject.transform;
            tc.parent = parent;
            tc.direction = direction;
            tc.squirrel = squirrel;
            tc.SetFlip();

            //SmokeController sc = Instantiate(smokePrefab, transform.position, Quaternion.identity);
            //sc.transform.parent = parent.terrainObject.transform;
            //sc.parent = parent;
            //sc.direction = direction;

            cooldown = 210 - Random.Range(0, 70);
        }
        cooldown -= 1;

        if(transform.position.x < -1){
            Destroy(gameObject);
        }
    }
}
