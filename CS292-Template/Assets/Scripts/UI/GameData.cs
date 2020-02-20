using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class DataLoader
{
    public GameData data;

    public DataLoader(){
        data = new GameData(new int[0], 0, 0f);
    }

    public void SaveFile()
    {
        string destination = Application.persistentDataPath + "/save.dat";
        FileStream file;

        if(File.Exists(destination)) file = File.OpenWrite(destination);
        else file = File.Create(destination);

        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, data);
        file.Close();
    }

    public void LoadFile()
    {
        string destination = Application.persistentDataPath + "/save.dat";
        FileStream file;

        if(File.Exists(destination)) file = File.OpenRead(destination);
        else
        {
            Debug.LogError("File not found");
            
            SaveFile();
            return;
        }

        BinaryFormatter bf = new BinaryFormatter();
        GameData newData = (GameData) bf.Deserialize(file);
        file.Close();

        data = newData;
    }
}

[System.Serializable]
public class GameData
{
    public int[] scores;
    public int runs;
    public float timePlayed;

    public GameData(int[] scores, int runs, float timePlayed)
    {
        this.scores = scores;
        this.runs = runs;
        this.timePlayed = timePlayed;
    }

    public GameData()
    {
        scores = new int[0];
    }
}