using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarGenController : MonoBehaviour
{
    public int cooldown = 100;
    
    public GameController parent;
    public int direction;
    public CarController carPrefab;
    public GameObject squirrel;
    void Start()
    {
        cooldown = Random.Range(0, 70);
        //parent = parentObj.GetComponent<GameController>();
    }

    void FixedUpdate()
    {
        transform.position += new Vector3(-1 * parent.landSpeed * Time.deltaTime, 0, 0);
        if(cooldown <= 0){
            CarController tc = Instantiate(carPrefab, transform.position, Quaternion.identity);
            tc.parent = parent;
            tc.direction = direction;
            tc.squirrel = squirrel;
            tc.SetFlip();
            cooldown = 100 - Random.Range(0, 30);
        }
        cooldown -= 1;

        if(transform.position.x < -1){
            Destroy(gameObject);
        }
    }
}
