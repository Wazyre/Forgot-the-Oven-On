using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Menu : MonoBehaviour
{
    [Header("UI Object")] 
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject optionsMenu;
    [SerializeField] Slider mainVolumeSlider;
    [SerializeField] TextMeshProUGUI mainVolumeText;
    [SerializeField] Slider sensitivitySlider;
    [SerializeField] TextMeshProUGUI sensitivityText;
    [SerializeField] Selectable invertYToggle;
    [SerializeField] Slider resolutionSlider;
    [SerializeField] TextMeshProUGUI resolutionText;

    [Header("UI Variables")] 
    [SerializeField] float mainVolume;
    [SerializeField] float sfxVolume;
    [SerializeField] int resolutionIndex;
    [SerializeField] Resolution[] resolutions;

    [Header("Trackers")]
    [SerializeField] bool optionsMenuOpen = false;
    [SerializeField] bool gamePaused = false;
     
    void Awake() {
        pauseMenu = GameObject.FindWithTag("PauseMenu");
        optionsMenu = GameObject.FindWithTag("OptionsMenu");
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(false);
    }

    // Start is called before the first frame update
    void Start() {
        mainVolumeSlider.value = PlayerPrefs.GetFloat("mainVolume", 0.4f);
        mainVolumeText.text = (Mathf.Round(mainVolumeSlider.value * 100)).ToString();
        sensitivitySlider.value = PlayerPrefs.GetFloat("sensitivity", 0.3f);
        sensitivityText.text = (Mathf.Round(sensitivitySlider.value * 100)).ToString();
        
        resolutions = Screen.resolutions;
        resolutionSlider.maxValue = resolutions.Length - 1;
        resolutionIndex = -1;

        for (int i = 0; i < resolutions.Length; i++) {
            if (resolutions[i].Equals(Screen.currentResolution)) {
                resolutionIndex = i;
                break;
            }
        }

        if (resolutionIndex > 0) {
            resolutionSlider.value = resolutionIndex;
            UpdateResolution();
        } 
        else {
            Debug.LogError("Current screen resolution not found in list of valid resolutions! How?!");
        }
    }

    public void PauseMenu() {
        gamePaused = !gamePaused;
        UpdateCursor();
        pauseMenu.SetActive(gamePaused);
        Time.timeScale = gamePaused ? 0 : 1;
    }

    public void OptionsMenu() {
        optionsMenuOpen = !optionsMenuOpen;
        pauseMenu.SetActive(!optionsMenuOpen);
        optionsMenu.SetActive(optionsMenuOpen);
        if (!optionsMenuOpen) {
            ApplySettings();
        }
    }

    public void ApplySettings() {
        UpdateVolume();
        UpdateSensitivity();
        UpdateResolution();
    }

    public void StartGame() {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(false);

        LoadGame();
    }

    public void LoadGame() {
        SaveLoad.Load();
    }

    public void SaveGame() {
        SaveLoad.Save();
    }

    void UpdateCursor () {
        if (gamePaused) {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        } 
        else {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void UpdateVolume() {
        PlayerPrefs.SetFloat("mainVolume", mainVolumeSlider.value);
        gameAudio.volume = mainVolumeSlider.value;
    }
    
    public void UpdateSensitivity() {
        PlayerPrefs.SetFloat("sensitivity", sensitivitySlider.value);
        camFollow.SetSensitivity(sensitivitySlider.value);
    }

    public void UpdateResolution() {
        string newRes = ResToString(resolutions[resolutionIndex]);
        PlayerPrefs.SetInt("resolution", resolutionIndex);
        resolutionText = newRes;
        Screen.SetResolution(resolutions[resolutionIndex].width, resolutions[resolutionIndex].height, true);
    }

    public string ResToString(Resolution res) {
        return "Resolution: " + res.ToString();
    }

    public void Quit() {
        Application.Quit();
    }
}
