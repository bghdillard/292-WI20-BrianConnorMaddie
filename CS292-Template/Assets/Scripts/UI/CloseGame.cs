﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseGame : MonoBehaviour
{
    // Start is called before the first frame update
    public void CloseApplication()
    {
        Application.Quit();
        //UnityEditor.EditorApplication.isPlaying = false;
    }


    public void Start()
    {

    }
}
