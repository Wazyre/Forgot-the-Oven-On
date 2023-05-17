using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] float storedTime;
    [SerializeField] Menu menu;
    [SerializeField] TextMeshProUGUI timer;

    void Start() {
        storedTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        int min = Mathf.FloorToInt(LevelManager.current.time / 60);
        int sec = Mathf.FloorToInt((LevelManager.current.time % 60));
        timer.text = string.Format("Timer {0:00}:{1:00}", min, sec);
        storedTime += Time.deltaTime;

        if (LevelManager.current.time > 0 && !Menu.GamePaused && storedTime >= 60f) {
            
            LevelManager.current.time -= 1f;
            storedTime = 0;
        }
        else if ((Menu.GamePaused || storedTime < 60f) && LevelManager.current.time > 0) {}
        else {
            LevelManager.current.time = 0;
            menu.GameOver();
        }
        
    }
}
