using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class LevelManager //Is started by Main Menu or when game is started
{
    public static LevelManager current; //Starts a global Level Manager

    public float time; // Stores current level timer

    public int sceneID;

    public int lastCheckpoint = -1;

    public enum Difficulty {Easy, Normal, Hard}; // Stores game current difficulty

    public Difficulty difficulty;

    public float[] times = {240f, 150f, 60f};

    public bool isSceneBeingLoaded = false; //Checks if a scene is being loaded

    public LevelManager() {
        difficulty = (Difficulty)PlayerPrefs.GetInt("difficulty", 0);
        time = times[PlayerPrefs.GetInt("difficulty", 0)];
        sceneID = SceneManager.GetActiveScene().buildIndex;
    }
    
    public void TimeReset() {
        time = times[PlayerPrefs.GetInt("difficulty", 0)];
    }
}
