using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Singleton = null;
    public AudioManager audioMgr;
    // public Image blackScreen;

    private void Awake() {
        if (Singleton) {
            gameObject.SetActive(false);
            Destroy(gameObject);
            return;
        } 
        else {
            Singleton = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Update() {
        if (LevelManager.current != null && !Menu.GamePaused) {
            if (LevelManager.current.time > 0) {
                LevelManager.current.time -= Time.deltaTime;
            }
        }
    }

    public void newScene(int sceneNumber) {
        //StartCoroutine(fade.FadeImageToFullAlpha(0.5f, blackScreen));
        SceneManager.LoadScene(sceneNumber);
 
        if (SceneManager.GetActiveScene().buildIndex != sceneNumber) {
            StartCoroutine("waitForSceneLoad", sceneNumber);
        }
        // else {
        //     StartCoroutine(fade.FadeImageToZeroAlpha(0.5f, blackScreen));
        // }
   }
 
    IEnumerator waitForSceneLoad(int sceneNumber) {
        while (SceneManager.GetActiveScene().buildIndex != sceneNumber) {
            //yield return fade.FadeImageToFullAlpha(0.5f, blackScreen);
            yield return null;
        }
 
        // After proper scene has been loaded
         if (SceneManager.GetActiveScene().buildIndex == sceneNumber) {
            // Debug.Log(SceneManager.GetActiveScene().buildIndex);
            audioMgr.AudioSwitch(sceneNumber);
            //yield return fade.FadeImageToZeroAlpha(0.5f, blackScreen);
            yield return null;
        }
    }
}
