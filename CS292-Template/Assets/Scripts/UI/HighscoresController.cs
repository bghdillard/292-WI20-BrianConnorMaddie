using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class HighscoresController : MonoBehaviour
{
    public GameObject HighScoreText;
    Text txt;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Delete)){
            DataLoader DL = new DataLoader();
            DL.SaveFile();
            OnEnable();
        }
    }

    void OnEnable(){
        txt = HighScoreText.GetComponent<Text>();
        
        DataLoader DL = new DataLoader();
        DL.LoadFile();

        List<int> scores = DL.data.scores.ToList().OrderByDescending(x => x).ToList();

        string s = "";    
        for(int i = 0; i < 5; i += 1){
            if(i < scores.Count){
                s += (i+1).ToString() + "\t- " + scores[i].ToString("#,##0") + "\n";
            } else {
                s += (i+1).ToString() + "\t- 0\n";
            }
        }
        txt.text = s;
    }
}

