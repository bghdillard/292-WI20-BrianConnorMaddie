using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.ParticleSystem;

public class PedestrianGenController : MonoBehaviour
{
    public int minCooldown = 140;
    public int maxCooldown = 200;
    public PedestrianController pedestrianPrefab;

    public GameController parent;
    public GameObject squirrel;
    public int direction;
    int cooldown;

    void Start()
    {
        cooldown = Random.Range(0, minCooldown);
        //parent = parentObj.GetComponent<GameController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //transform.position += new Vector3(-1 * parent.landSpeed * Time.deltaTime, 0, 0);
        if(cooldown <= 0){
            PedestrianController tc = Instantiate(pedestrianPrefab, transform.position, Quaternion.identity);
            tc.transform.parent = parent.terrainObject.transform;
            tc.parent = parent;
            tc.direction = direction;
            tc.squirrel = squirrel;
            tc.SetFlip();

            cooldown = Random.Range(minCooldown, maxCooldown);
        }
        cooldown -= 1;

        if(transform.position.x < -1){
            Destroy(gameObject);
        }
    }
}
