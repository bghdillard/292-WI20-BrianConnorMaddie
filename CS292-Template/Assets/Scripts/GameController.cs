using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class GameController : MonoBehaviour
{
    public GameObject TileSet;
    public int landSpeed;

    float offset;
    int front = 9;
    public Tile GrassTile;
    public Tile RoadTile;
    int genState;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        TileSet.transform.position = TileSet.transform.position + new Vector3(-1 * landSpeed * Time.deltaTime, 0, 0);
        offset += landSpeed * Time.deltaTime;
        if(offset + 9 > front){
            MakeNewLayer();
        }
        Debug.Log(offset);
    }

    void MakeNewLayer()
    {
        Tilemap tilemap = TileSet.GetComponent<Tilemap>();
        int r = Random.Range(0, 3);

        if(r + genState > 2){
            for(int i = -6; i < 5; i+=1){
                tilemap.SetTile(new Vector3Int(front, i, 0), RoadTile);
            } 
            genState = 0;
        }else{
            for(int i = -6; i < 5; i+=1){
                tilemap.SetTile(new Vector3Int(front, i, 0), GrassTile);
            } 
            genState += 1;
        }

        for(int i = -6; i < 5; i+=1){
            tilemap.SetTile(new Vector3Int(front - 18, i, 0), null);
        }
        front += 1;
    }
}
