using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PausingScript : MonoBehaviour
{
    private GameObject optionsMenu;
    private GameObject pauseMenu;
    //private GameObject player;
    private PlayerMechanics playerMech;
    private PlayerInput cameraInput;

    private bool isCurrentlyPaused = false;

    void Awake() {
        pauseMenu = GameObject.Find("PauseMenu");
        optionsMenu = GameObject.Find("OptionsMenu");
    }

    void Start() {
        // Cursor.visible = false;
        Time.timeScale = 1;
        //player = GameObject.FindGameObjectWithTag("Player");
        playerMech = GameObject.FindWithTag("Player").GetComponent<PlayerMechanics>();
        cameraInput = GameObject.FindWithTag("Dolly").GetComponent<PlayerInput>();
        pauseMenu.SetActive(false);
    }

    // void Update() {
    //     // Toggle pause menu when pressing Escape
    //     if(Input.GetKeyDown(KeyCode.Tilde)) {
    //         // if (isCurrentlyPaused) // If pause menu is on
    //         //     ResumeGame();
    //         // else // If pause menu is off
    //         ResumeGame();
    //     }
    // }

    public void PauseGame() {
        Debug.Log("pauseed");
        pauseMenu.SetActive(true); // Show pause menu
        playerMech.OnDisable();
        cameraInput.actions.Disable();
        Cursor.lockState = CursorLockMode.None;
        isCurrentlyPaused = true;
        Time.timeScale = 0; // Stop time
    }

    public void ResumeGame() {
        playerMech.OnEnable();
        cameraInput.actions.Enable();
        Cursor.lockState = CursorLockMode.Locked;
        isCurrentlyPaused = false;
        Time.timeScale = 1; // Resume time
        pauseMenu.SetActive(false); // Hide pause menu
        optionsMenu.SetActive(false);
    }

    // Allows pausing game using external scripts
    public void PauseControl() {
        
    }

    // public void SaveGame()
    // {
    //     Scene scene = SceneManager.GetActiveScene();
    //     LevelManager.current.playerData.sceneID = scene.buildIndex;
    //     LevelManager.current.playerData.playerPosX = player.transform.position.x;
    //     LevelManager.current.playerData.playerPosY = player.transform.position.y;
    //     LevelManager.current.playerData.playerPosZ = player.transform.position.z;
    //     SaveLoad.WriteToDisk();
    // }

    // public void LoadGame()
    // {
    //     SaveLoad.LoadFromDisk();
    //     if (LevelManager.current.playerData.finishedGame)
    //     {
    //         //restartText.gameObject.SetActive(true);
    //         if (control.load)
    //         {
    //             goto loading;
    //         }
    //         else
    //         {
    //             return;
    //         }
    //     }
    //     loading:
    //         LevelManager.current.isSceneBeingLoaded = true;
    //         int whichScene = LevelManager.current.playerData.sceneID;
    //         SceneManager.LoadScene(whichScene);

    //         float t_x = LevelManager.current.playerData.playerPosX;
    //         float t_y = LevelManager.current.playerData.playerPosY;
    //         float t_z = LevelManager.current.playerData.playerPosZ;
    //         player.transform.position = new Vector3(t_x, t_y, t_z);
    // }

    public void OpenOptions() {
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }

    public void MainMenu() {
        SceneManager.LoadScene(0);
    }

    public void QuitGame() {
        Application.Quit();
    }
}
