using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelManager //Is started by Main Menu or when game is started
{
    public static LevelManager current; //Starts a global Level Manager

    public float time; // Stores level timer

    public bool isSceneBeingLoaded = false; //Checks if a scene is being loaded

    public LevelManager() {
        time = 0;
    }

    public TimeReset() {
        time = 0;
    }
}
