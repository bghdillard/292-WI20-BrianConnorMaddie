using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class GameController : MonoBehaviour
{
    public GameObject TerrainTileset;
    public GameObject ObjectTileset;
    public int landSpeed;

    float offset; //How far has camera(terrain) moved
    int front = 13; //Used for determine new layer placement
    int bot = -1;
    int top = 8;
    Tilemap terrain;
    Tilemap objects;
    public Tile GrassTile;
    public Tile RoadTile;
    public Tile TrackTile;
    public Tile RockTile;
    public Tile IceTile;
    public Tile SnowTile;
    public Tile BushTile;
    int genState; //Used for markov chains
    Dictionary<int,List<int>> T; //Terrain Array
    //-1 = Out of Bounds
    //0 = Passable
    //1 = Blocking
    //2 = Ice
    //3 = Lethal?

    public int GetTerrain(int row, int col){ //Brian - Use this for movement
        if(row < 0) return -1;
        if(row >= 7) return -1;   
        if(T.ContainsKey(col) == false) return -1;
        return T[col][row];
    }

    void Start()
    {
        terrain = TerrainTileset.GetComponent<Tilemap>();
        objects = ObjectTileset.GetComponent<Tilemap>();

        T = new Dictionary<int, List<int>>();
        //Initialize Terrain
        for(int i = 0; i < 13; i += 1){
            List<int> L = new List<int>();
            for(int j = 0; j < 7; j += 1){
                L.Add(0);
            }
            T.Add(i, L);
        }
        //Based on starting tiles
        T[0][4] = 1;
        T[1][1] = 1;
        T[2][5] = 1;
        T[3][6] = 1;
        T[3][0] = 1;
        T[4][1] = 1;
        T[6][0] = 1;
        T[7][5] = 1;
        T[9][6] = 1;
        T[10][1] = 1;
        T[11][2] = 1;
    }

    void FixedUpdate()
    {
        TerrainTileset.transform.position = TerrainTileset.transform.position + new Vector3(-1 * landSpeed * Time.deltaTime, 0, 0);
        ObjectTileset.transform.position = TerrainTileset.transform.position;
        offset += landSpeed * Time.deltaTime;
        if(offset + 13 > front){
            MakeNewLayer();
        }
    }

    void MakeNewLayer()
    {
        List<int> L = new List<int>();
        for(int i = 0; i < 7; i += 1){
            L.Add(0);
        }
        T.Remove(front - 14);
        T.Add(front, L);

        for(int i = bot; i < top; i+=1){
            terrain.SetTile(new Vector3Int(front - 14, i, 0), null);
            objects.SetTile(new Vector3Int(front - 14, i, 0), null);
            
        }

        int r = Random.Range(0, 3);

        if(r + genState > 2){
            int r2 = Random.Range(0, 2);
            if(r2 == 0){
                MakeRoad();
            }else{
                MakeTracks();
            }
            genState = 0;
        }else{
            MakeRocks();
            genState += 1;
        } 
    }

    void MakeRoad(){
        for(int i = bot; i < top; i+=1){
            terrain.SetTile(new Vector3Int(front, i, 0), RoadTile);
        }
        front += 1;
    }

    void MakeTracks(){
        for(int i = bot; i < top; i+=1){
            terrain.SetTile(new Vector3Int(front, i, 0), TrackTile);
        }
        MakePassable();
        front += 1;
    }

    void MakePassable()
    {
        int r = Random.Range(1, 6);
        for(int i = bot; i < bot + r; i+=1){
            terrain.SetTile(new Vector3Int(front, i, 0), SnowTile);
        }
        for(int i = bot + r; i < top; i+=1){
            terrain.SetTile(new Vector3Int(front, i, 0), GrassTile);
        }
    }
    void MakeRocks(){
        for(int i = 0; i < 7; i+=1){
            int r2 = Random.Range(0, 7);
            if(r2 % 7 == 0){
                objects.SetTile(new Vector3Int(front, i, 0), BushTile);
                T[front][i] = 1;
            } else if(r2 % 7 == 1){
                T[front][i] = 1;
                objects.SetTile(new Vector3Int(front, i, 0), RockTile);
            }    
        }
        MakePassable();
        front += 1;
    }
}
