using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainGenController : MonoBehaviour
{
    public int cooldown = 100;
    public TrainController trainPrefab;
    public GameObject parentObj;
    public GameController parent;
    public int direction;
    void Start()
    {
        cooldown = Random.Range(0, 70);
        //parent = parentObj.GetComponent<GameController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += new Vector3(-1 * parent.landSpeed * Time.deltaTime, 0, 0);
        if(cooldown <= 0){
            TrainController tc = Instantiate(trainPrefab, transform.position, Quaternion.identity);
            tc.parent = parent;
            tc.direction = direction;
            tc.SetFlip();
            cooldown = 210 - Random.Range(0, 70);
        }
        cooldown -= 1;

        if(transform.position.x < -1){
            Destroy(gameObject);
        }
    }
}
