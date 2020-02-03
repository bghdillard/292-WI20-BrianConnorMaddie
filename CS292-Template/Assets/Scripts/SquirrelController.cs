using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquirrelController : MonoBehaviour
{

    GameObject Controller;
    int row;
    int col;

    // Start is called before the first frame update
    void Start()
    {
        Controller = GameObject.Find("GameController");
        row = 3;
        col = 3;

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            int useable = Controller.GetComponent<GameController>().GetTerrain(row, col -1);
            print(useable);
            if(useable != -1 && useable != 1)
            {
                col--;
            }
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            int useable = Controller.GetComponent<GameController>().GetTerrain(row, col + 1);
            print(useable);
            if (useable != -1 && useable != 1)
           {
               col++;
           }
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            int useable = Controller.GetComponent<GameController>().GetTerrain(row - 1, col);
            print(useable);
            if (useable != -1 && useable != 1)
            {
                row++;
            }
        }
        if(Input.GetKeyDown(KeyCode.S))
        {
            int useable = Controller.GetComponent<GameController>().GetTerrain(row + 1, col);
            print(useable);
            if (useable != -1 && useable != 1)
            {
                row--;
            }
        }
        transform.position = new Vector3(col - Controller.GetComponent<GameController>().offset, row, 0);
    }
}
