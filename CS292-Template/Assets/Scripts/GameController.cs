using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public class GameController : MonoBehaviour
{
    public GameObject terrainObject;
    GameObject tlayers;
    GameObject TerrainTileset;
    GameObject ObjectTileset;
    GameObject AboveTileset;

    public GameObject TerrainPrefab;
    public GameObject TerrainLayers;
    public float landSpeed;

    public GameObject Squirrel;
    public GameObject SquirrelPrefab;
    public SquirrelController squirrelController;

    public float offset; //How far has camera(terrain) moved
    int front = 13; //Used for determine new layer placement
    int bot = -1;
    int top = 8;
    Tilemap terrain;
    Tilemap objects;
    Tilemap above;
    public Tile GrassTile;
    public Tile RoadTile;
    public Tile TrackTile;
    public Tile RockTile;
    public Tile IceTile;
    public Tile SnowTile;
    public Tile BushTile;
    public Tile TrunkTile;
    public Tile TreetopTile;
    public GameObject CollectiblePrefab;
    int genState; //Used for markov chains
    public bool running;
    public int scoreVal = 0;
    public TrainGenController tgen;
    public CarGenController cgen;
    public PedestrianGenController pgen;
    System.Random rnd = new System.Random();
    Dictionary<int,List<char>> T; //Terrain Array
    Dictionary<int,List<char>> B; //Access Array
    //| = Out of Bounds
    //- = Passable
    //# = Blocking
    //= = Ice
    //X = Lethal?
    int difficulty;
    int nextCollectible;
    

    public void ResetGame(){ //Maddie - Use this to reset Game
        running = false;
        DestroyImmediate(Squirrel);
        DestroyImmediate(terrainObject);
        DestroyImmediate(tlayers);

        SetupGame();
    }

    public char GetTerrain(int row, int col){ //Brian - Use this for movement
        if(row < 0) return '|';
        if(row >= 7) return '|';   
        if(T.ContainsKey(col) == false) return '|';
        return T[col][row];
    }

    void Start()
    {
        SetupGame();
    }

    void SetupGame(){
        offset = 0;
        scoreVal = 0;
        front = 13;
        difficulty = 0;
        nextCollectible = 2;
        
        terrainObject = Instantiate(TerrainPrefab, new Vector3(0, 0, 0), Quaternion.identity).gameObject;
        terrainObject.transform.position = new Vector3(-1 * offset, terrainObject.transform.position.y, 0);
        terrainObject.transform.parent = transform;

        tlayers = Instantiate(TerrainLayers, new Vector3(0, 0, 0), Quaternion.identity).gameObject;
        tlayers.transform.parent = terrainObject.transform;
            
        Squirrel = Instantiate(SquirrelPrefab, new Vector3(3, 3, 0), Quaternion.identity).gameObject;
        squirrelController = Squirrel.GetComponent<SquirrelController>();
        squirrelController.Controller = gameObject;

        
        TerrainTileset = tlayers.transform.Find("TerrainGrid/TerrainMap").gameObject;
        ObjectTileset = tlayers.transform.Find("ObjectGrid/ObjectMap").gameObject;
        AboveTileset = tlayers.transform.Find("AboveGrid/AboveMap").gameObject;

        terrain = TerrainTileset.GetComponent<Tilemap>();
        objects = ObjectTileset.GetComponent<Tilemap>();
        above = AboveTileset.GetComponent<Tilemap>();

        T = new Dictionary<int, List<char>>();
        B = new Dictionary<int, List<char>>();
        //Initialize Terrain
        for(int i = 0; i < 13; i += 1){
            List<int> L = new List<int>();
            T.Add(i, new List<char>(new char[]{'-','-','-','-','-','-','-'}));
            B.Add(i, new List<char>(new char[]{'-','-','-','-','-','-','-'}));
        }

        //Based on starting tiles
        int[] StartXs = new int[]{1, 2, 3, 4, 4, 5, 7, 8, 10, 10, 12};
        int[] StartYs = new int[]{2, 5, 1, 6, 0, 5, 6, 1, 0, 5, 4};

        for(int i = 0; i < StartXs.Length; i += 1){
            T[StartXs[i]][StartYs[i]] = '#';
            B[StartXs[i]][StartYs[i]] = '#';
        }

        genState = rc(new int[]{1, 2, 3});
        running = false;
        
    }

    

    void Update(){
        if(terrainObject == null) return;
        terrainObject.transform.position = new Vector3(-1 * offset - 0.5f, terrainObject.transform.position.y, 0);
        //if(running){
            
            //TerrainTileset.transform.position = new Vector3(-1 * offset - 0.5f, TerrainTileset.transform.position.y, 0);// = TerrainTileset.transform.position + new Vector3(-1 * landSpeed * Time.deltaTime, 0, 0);
            //ObjectTileset.transform.position = TerrainTileset.transform.position;
            //AboveTileset.transform.position = TerrainTileset.transform.position;
        //}
    }

    void FixedUpdate()
    {
        if(running){
            //landSpeed += Time.deltaTime / 10;
            landSpeed += ((1 / (float)(front + 20)) * Time.deltaTime); //Acceleration Function integrates to Log

            offset += landSpeed * Time.deltaTime;
            if(offset > 25 && difficulty == 0){
                print("Medium");
                difficulty = 1;
            }
            if(offset > 50 && difficulty == 1){
                print("Hard");
                difficulty = 2;
            } 
            if(offset + 24 > front){
                MakeNewLayer();
            }
        }

        if(Time.time >= nextUpdate && running){
             nextUpdate=Mathf.FloorToInt(Time.time)+1;
             UpdateEverySecond();
        }

        
    }

    private int nextUpdate=1;
    void UpdateEverySecond(){
        if(squirrelController.isDead() == false){
            scoreVal += squirrelController.col * 10;
        }
    }

    void PauseGame(){
        running = false;
    }

    void UnpauseGame(){
        running = true;
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
            above.SetTile(new Vector3Int(front - 25, i, 0), null);
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

        int r = 0;// = Random.Range(0, 24);
        if(genState == 20 || genState == 21 || genState == 22 || genState == 23 || genState == 24 || genState == 25 || genState == 26){ //End of Passable
            int dir;
            if(genState == 20){
                dir = rc(new int[]{1, -1});
            }else if(genState == 22 || genState == 24 || genState == 26){
                dir = 1;
            }else {
                dir = -1;
            }

            if(difficulty == 0) r = rc(new int[]{0});
            if(difficulty == 1) r = rc(new int[]{0, 1});
            if(difficulty == 2) r = rc(new int[]{0, 1, 2});
            if(r == 0){
                MakeTracks(dir);
            } else if(r == 0) {
                MakeRoad(dir);
            } else {
                MakeSidewalk(dir);
            }

            if(genState == 20 || genState == 21 || genState == 24){
                if(difficulty == 0) genState = rc(new int[]{4, 3, 14, 13});
                if(difficulty == 1) genState = rc(new int[]{4, 3, 2, 14, 13, 12});
                if(difficulty == 2) genState = rc(new int[]{3, 2, 13, 12});
            } else {
                genState -= 1;
            }
        } else if(genState == 5 || genState == 4 || genState == 3 || genState == 2 || genState == 1){ //More Passable
            MakeRocks();
            genState -= 1;
            if(genState == 0){
                if(difficulty == 0) genState = rc(new int[]{20, 20, 22, 24});
                if(difficulty == 1) genState = rc(new int[]{20, 20, 22, 24});
                if(difficulty == 2) genState = rc(new int[]{20, 20, 22, 24});
            }
        }else if(genState == 15 || genState == 14 || genState == 13 || genState == 12 || genState == 11){ //More Ice Field
            MakeIceField();
            genState -= 1;
            if(genState == 10){
                if(difficulty == 0) genState = rc(new int[]{20});
                if(difficulty == 1) genState = rc(new int[]{20, 20, 22, 24});
                if(difficulty == 2) genState = rc(new int[]{22, 23, 25, 26});
            }
        }

        UpdateBlocking();

        MakeCollectibles();

        front += 1;
    }

    void MakeCollectibles(){
        if(nextCollectible != 0){
            nextCollectible -= 1;
            return;
        }
        List<int> open = Enumerable.Range(0, 7).Where(i => B[front][i] == '-').ToList();
        if(open.Count > 0){
            int pos = rc(open.ToArray());

            GameObject Collectible = Instantiate(CollectiblePrefab, new Vector3(front - offset, pos, 0), Quaternion.identity).gameObject;
            CollectibleController collectibleController = Collectible.GetComponent<CollectibleController>();
            Collectible.transform.parent = terrainObject.transform;
            collectibleController.parent = this;
            int colType = -1;
            if(difficulty == 0) colType = rc(new int[]{0, 0, 3});
            if(difficulty == 1) colType = rc(new int[]{0, 0, 0, 0, 3, 3, 1});
            if(difficulty == 2) colType = rc(new int[]{0, 0, 0, 0, 3, 3, 2, 1});
            collectibleController.type = colType;

            nextCollectible = 5;
        }
    }

    void MakeRoad(int dir){
        for(int i = bot; i < top; i+=1){
            terrain.SetTile(new Vector3Int(front, i, 0), RoadTile);
        }

        for(int i = 0; i < 7; i += 1){
            if(dir == 1){T[front][i] = '>';}
            else{T[front][i] = '<';}
        }
        
        CarGenController tg;
        if(dir == 1){
            tg = Instantiate(cgen, new Vector3(front - offset, -3, 0), Quaternion.identity);
            tg.direction = 1;
        }else{
            tg = Instantiate(cgen, new Vector3(front - offset, 12, 0), Quaternion.identity);
            tg.direction = -1;
        }
        tg.transform.parent = terrainObject.transform;
        tg.parent = this;
        tg.squirrel = Squirrel;
    }

    void MakeSidewalk(int dir){
        for(int i = bot; i < top; i+=1){
            terrain.SetTile(new Vector3Int(front, i, 0), RoadTile);
        }

        for(int i = 0; i < 7; i += 1){
            if(dir == 1){T[front][i] = '>';}
            else{T[front][i] = '<';}
        }
        
        PedestrianGenController tg;
        if(dir == 1){
            tg = Instantiate(pgen, new Vector3(front - offset, -3, 0), Quaternion.identity);
            tg.direction = 1;
        }else{
            tg = Instantiate(pgen, new Vector3(front - offset, 12, 0), Quaternion.identity);
            tg.direction = -1;
        }
        tg.transform.parent = terrainObject.transform;
        tg.parent = this;
        tg.squirrel = Squirrel;
    }

    void MakeTracks(int dir){
        for(int i = bot; i < top; i+=1){
            objects.SetTile(new Vector3Int(front, i, 0), TrackTile);
        }

        for(int i = 0; i < 7; i += 1){
            if(dir == 1){T[front][i] = '>';}
            else{T[front][i] = '<';}
        }

        TrainGenController tg;
        if(dir == 1){
            tg = Instantiate(tgen, new Vector3(front - offset, -3, 0), Quaternion.identity);
            tg.direction = 1;
            
        }else{
            tg = Instantiate(tgen, new Vector3(front - offset, 12, 0), Quaternion.identity);
            tg.direction = -1;
        }
        tg.transform.parent = terrainObject.transform;
        tg.parent = this;
        tg.squirrel = Squirrel;
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
        List<int> open = Enumerable.Range(0, 7).Where(i => B[front - 1][i] == '-').ToList();
        List<int> paths = new List<int>();
        if(difficulty == 0) paths = open.OrderBy(x => rnd.Next()).Take(2).ToList();
        if(difficulty > 0) paths = open.OrderBy(x => rnd.Next()).Take(1).ToList();
        List<int> rocks = new List<int>();
        if(difficulty == 0) rocks = Enumerable.Range(0, 7).OrderBy(x => rnd.Next()).Take(2).ToList();
        if(difficulty == 1) rocks = Enumerable.Range(0, 7).OrderBy(x => rnd.Next()).Take(3).ToList();
        if(difficulty > 1) rocks = Enumerable.Range(0, 7).OrderBy(x => rnd.Next()).Take(4).ToList();

        for(int i = 0; i < 7; i += 1){
            if(paths.Contains(i) || !rocks.Contains(i)){
                T[front][i] = '-';

                
            }else{
                T[front][i] = '#';
                int r = Random.Range(0, 3);
                if(r == 0){
                    objects.SetTile(new Vector3Int(front, i, 0), BushTile);
                } else if (r == 1) {
                    objects.SetTile(new Vector3Int(front, i, 0), RockTile);
                } else {
                    objects.SetTile(new Vector3Int(front, i, 0), TrunkTile);
                    above.SetTile(new Vector3Int(front, i + 1, 0), TreetopTile);
                }
            }
        }
    }

    void MakeIceField(){
        List<int> open = Enumerable.Range(0, 7).Where(i => B[front - 1][i] == '-').ToList();
        List<int> paths = new List<int>();;
        if(difficulty == 0) paths = open.OrderBy(x => rnd.Next()).Take(2).ToList();
        if(difficulty > 0) paths = open.OrderBy(x => rnd.Next()).Take(1).ToList();
        List<int> rocks = new List<int>();;
        List<int> patches = new List<int>();;
        if(difficulty == 0) rocks = Enumerable.Range(0, 7).OrderBy(x => rnd.Next()).Take(1).ToList();
        if(difficulty == 1) rocks = Enumerable.Range(0, 7).OrderBy(x => rnd.Next()).Take(2).ToList();
        if(difficulty == 2) rocks = Enumerable.Range(0, 7).OrderBy(x => rnd.Next()).Take(3).ToList();
        if(difficulty == 0) patches = Enumerable.Range(0, 7).OrderBy(x => rnd.Next()).Take(2).ToList();
        if(difficulty == 1) patches = Enumerable.Range(0, 7).OrderBy(x => rnd.Next()).Take(3).ToList();
        if(difficulty == 2) patches = Enumerable.Range(0, 7).OrderBy(x => rnd.Next()).Take(4).ToList();

        for(int i = 0; i < 7; i += 1){
            if(paths.Contains(i) || (!rocks.Contains(i) && !patches.Contains(i))){
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


