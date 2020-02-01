using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainController : MonoBehaviour
{
    public int speed = 3;
    public int direction = -1;

    public GameObject parentObj;
    public GameController parent;
    // Start is called before the first frame update
    void Start()
    {
        //parent = parentObj.GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(-1 * parent.landSpeed * Time.deltaTime, 0, 0);
        transform.position += new Vector3(0, direction * speed * Time.deltaTime, 0);

        if(transform.position.x < -1){
            Destroy(gameObject);
        }
        if(transform.position.y < -3){
            Destroy(gameObject);
        }
    }
}
