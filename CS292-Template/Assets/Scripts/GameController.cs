using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public class GameController : MonoBehaviour
{
    public GameObject TerrainTileset;
    public GameObject ObjectTileset;

    public GameObject TerrainLayers;
    public float landSpeed;

    public GameObject Squirrel;

    public float offset; //How far has camera(terrain) moved
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
    public bool running;
    public TrainGenController tgen;
    public CarGenController cgen;
    System.Random rnd = new System.Random();
    Dictionary<int,List<char>> T; //Terrain Array
    Dictionary<int,List<char>> B; //Access Array
    //| = Out of Bounds
    //- = Passable
    //# = Blocking
    //= = Ice
    //X = Lethal?

    public char GetTerrain(int row, int col){ //Brian - Use this for movement
        if(row < 0) return '|';
        if(row >= 7) return '|';   
        if(T.ContainsKey(col) == false) return '|';
        return T[col][row];
    }

    void Start()
    {
        GameObject tlayers = Instantiate(TerrainLayers, new Vector3(0, 0, 0), Quaternion.identity).gameObject;
        GameObject squirrel = Instantiate(Squirrel, new Vector3(0, 0, 0), Quaternion.identity).gameObject;

        TerrainTileset = tlayers.transform.Find("TerrainGrid/TerrainMap").gameObject;
        ObjectTileset = tlayers.transform.Find("ObjectGrid/ObjectMap").gameObject;


        terrain = TerrainTileset.GetComponent<Tilemap>();
        objects = ObjectTileset.GetComponent<Tilemap>();

        T = new Dictionary<int, List<char>>();
        B = new Dictionary<int, List<char>>();
        //Initialize Terrain
        for(int i = 0; i < 13; i += 1){
            List<int> L = new List<int>();
            T.Add(i, new List<char>(new char[]{'-','-','-','-','-','-','-'}));
            B.Add(i, new List<char>(new char[]{'-','-','-','-','-','-','-'}));
        }

        //Based on starting tiles
        int[] StartXs = new int[]{1, 2, 3, 4, 4, 5, 7, 8, 10, 11, 12};
        int[] StartYs = new int[]{2, 5, 1, 6, 0, 5, 6, 1, 0, 5, 4};

        for(int i = 0; i < StartXs.Length; i += 1){
            T[StartXs[i]][StartYs[i]] = '#';
            B[StartXs[i]][StartYs[i]] = '#';
        }

        genState = rc(new int[]{1, 2, 3});
        running = true;
    }

    void Update(){
        if(running){
            TerrainTileset.transform.position = new Vector3(-1 * offset - 0.5f, TerrainTileset.transform.position.y, 0);// = TerrainTileset.transform.position + new Vector3(-1 * landSpeed * Time.deltaTime, 0, 0);
            ObjectTileset.transform.position = TerrainTileset.transform.position;
        }
    }
    void FixedUpdate()
    {
        if(running){
            //landSpeed += Time.deltaTime / 10;
            landSpeed += ((1 / (float)(front + 20)) * Time.deltaTime); //Acceleration Function integrates to Log

            offset += landSpeed * Time.deltaTime;
            if(offset + 24 > front){
                MakeNewLayer();
            }
        }
    }

    void LayerSetup(){
        List<char> L = new List<char>();
        for(int i = 0; i < 7; i += 1){
            L.Add('-');
        }
        T.Remove(front - 25);
        B.Remove(front - 25);
        T.Add(front, L);

        for(int i = bot; i < top; i+=1){
            terrain.SetTile(new Vector3Int(front - 25, i, 0), null);
            objects.SetTile(new Vector3Int(front - 25, i, 0), null);
        }
    }

    void UpdateBlocking(){
        List<char> Last = B[front - 1];
        List<char> Terrain = T[front];
        List<char> NewBlocking = new List<char>();
        for(int i = 0; i < 7; i += 1){
            if(Terrain[i] == '#'){
                NewBlocking.Add('#');
            }else{
                NewBlocking.Add(Last[i]);
            }
        }
        B.Add(front, NewBlocking);

        for(int j = 0; j < 7; j += 1){
            for(int i = 1; i < 5; i += 1){
                if(NewBlocking[i-1] == '-' && (Terrain[i] == '-' || Terrain[i] == '=' || Terrain[i] == '>')) NewBlocking[i] = '-';
                if(NewBlocking[i+1] == '-' && (Terrain[i] == '-' || Terrain[i] == '=' || Terrain[i] == '<')) NewBlocking[i] = '-';
            }
        }
        if(NewBlocking[1] == '-' && Terrain[0] == '-') NewBlocking[0] = '-';
        if(NewBlocking[5] == '-' && Terrain[6] == '-') NewBlocking[6] = '-';
    }

    void MakeNewLayer()
    {
        LayerSetup();
        MakePassable();

        int r = Random.Range(0, 24);
        if(genState == 20 || genState == 21 || genState == 22 || genState == 23 || genState == 24){ //End of Passable
            int dir;
            if(genState == 20){
                dir = rc(new int[]{1, -2});
            }else if(genState == 22 || genState == 23){
                dir = 1;
            }else {
                dir = -1;
            }

            if(r % 2 == 0){
                MakeRoad(dir);
            } else {
                MakeTracks(dir);
            }

            if(genState == 20 || genState == 21 || genState == 23){
                genState = rc(new int[]{2, 3, 12, 13});
            } else {
                genState -= 1;
            }
        } else if(genState == 3 || genState == 2 || genState == 1){ //More Passable
            MakeRocks();
            genState -= 1;
            if(genState == 0) genState = rc(new int[]{20, 20, 22, 24});
        }else if(genState == 13 || genState == 12 || genState == 11){ //More Ice Field
            MakeIceField();
            genState -= 1;
            if(genState == 10) genState = rc(new int[]{20, 20, 22, 24});
        }

        UpdateBlocking();
        front += 1;
    }

    void MakeRoad(int dir){
        for(int i = bot; i < top; i+=1){
            terrain.SetTile(new Vector3Int(front, i, 0), RoadTile);
            
        }

        for(int i = 0; i < 7; i += 1){
            if(dir == 1){T[front][i] = '>';}
            else{T[front][i] = '<';}
        }
        

        if(dir == 1){
            CarGenController tg = Instantiate(cgen, new Vector3(front - offset, -3, 0), Quaternion.identity);
            tg.direction = 1;
            tg.parent = this;
        }else{
            CarGenController tg = Instantiate(cgen, new Vector3(front - offset, 12, 0), Quaternion.identity);
            tg.direction = -1;
            tg.parent = this;
        }
    }

    void MakeTracks(int dir){
        for(int i = bot; i < top; i+=1){
            objects.SetTile(new Vector3Int(front, i, 0), TrackTile);
        }

        for(int i = 0; i < 7; i += 1){
            if(dir == 1){T[front][i] = '>';}
            else{T[front][i] = '<';}
        }

        if(dir == 1){
            TrainGenController tg = Instantiate(tgen, new Vector3(front - offset, -3, 0), Quaternion.identity);
            tg.direction = 1;
            tg.parent = this;
        }else{
            TrainGenController tg = Instantiate(tgen, new Vector3(front - offset, 12, 0), Quaternion.identity);
            tg.direction = -1;
            tg.parent = this;
        }
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
        List<int> paths = Enumerable.Range(0, 7).Where(i => B[front - 1][i] == '-').ToList();
        int path = rc(paths.OrderBy(x => rnd.Next()).ToArray());
        List<int> rocks = Enumerable.Range(0, 7).OrderBy(x => rnd.Next()).Take(4).ToList();

        for(int i = 0; i < 7; i += 1){
            if(path == i || !rocks.Contains(i)){
                T[front][i] = '-';
            }else{
                T[front][i] = '#';
                if(Random.Range(0, 2) == 0){
                    objects.SetTile(new Vector3Int(front, i, 0), BushTile);
                } else {
                    objects.SetTile(new Vector3Int(front, i, 0), RockTile);
                }  
            }
        }
    }

    void MakeIceField(){
        List<int> paths = Enumerable.Range(0, 7).Where(i => B[front - 1][i] == '-').ToList();
        int path = rc(paths.OrderBy(x => rnd.Next()).ToArray());
        List<int> rocks = Enumerable.Range(0, 7).OrderBy(x => rnd.Next()).Take(3).ToList();
        List<int> patches = Enumerable.Range(0, 7).OrderBy(x => rnd.Next()).Take(4).ToList();

        for(int i = 0; i < 7; i += 1){
            if(path == i || (!rocks.Contains(i) && !patches.Contains(i))){
                T[front][i] = '-';
            }else if(patches.Contains(i)){
                T[front][i] = '=';
                terrain.SetTile(new Vector3Int(front, i, 0), IceTile);
            }else{
                T[front][i] = '#';
                objects.SetTile(new Vector3Int(front, i, 0), RockTile);
            }
        }
    }

    private int WeightedChoice(int[] choices, float[] weights){
        float sum= 0;
        for(int i = 0; i < weights.Length; i++){
            sum += weights[i];
        }

        float r = Random.Range(0, sum);
        int j = 0;
        r -= weights[j];
        while(r > 0){
            j += 1;
            r -= weights[j]; 
        }
        return choices[j];
    }

    private int rc(int[] choices){
        int r = Random.Range(0, choices.Length);
        return choices[r];
    }
}


