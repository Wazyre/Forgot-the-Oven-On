using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
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
    [SerializeField] Slider resolutionSlider;
    [SerializeField] TextMeshProUGUI resolutionText;
    [SerializeField] Toggle invertYSelect;
    [SerializeField] Toggle freeAimSelect;
    [SerializeField] Image reticle;
    // [SerializeField] Image blackScreen;
    [SerializeField] TextMeshProUGUI jumpText;
    [SerializeField] TextMeshProUGUI swingText;
    [SerializeField] TextMeshProUGUI moveText;
    [SerializeField] TextMeshProUGUI cpText;
    [SerializeField] TextMeshProUGUI gameOverText;
    [SerializeField] TextMeshProUGUI winText;
    [SerializeField] TextMeshProUGUI finishGameText;
    [SerializeField] Button restartBtn;
    [SerializeField] Button nextBtn;

    [Header("UI Variables")] 
    [SerializeField] float mainVolume;
    [SerializeField] float sfxVolume;
    [SerializeField] int resolutionIndex;
    [SerializeField] Resolution[] resolutions;
    public float sensitivity;
    public bool freeAim = false;
    public bool invertY = false;

    [Header("Trackers")]
    [SerializeField] bool optionsMenuOpen = false;

    [Header("Other Objects")]
    //[SerializeField] AudioSource gameAudio;
    [SerializeField] Fade fade;
    [SerializeField] AudioManager audioMgr;
    //[SerializeField] CameraFollow camFollow;
    
    public static bool GamePaused = false;
     
    void Awake() {
        pauseMenu = GameObject.FindWithTag("PauseMenu");
        optionsMenu = GameObject.FindWithTag("OptionsMenu");
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(false);

        freeAim = (PlayerPrefs.GetInt("freeAim", 0) != 0);
        reticle.gameObject.SetActive(freeAim);
    }

    // Start is called before the first frame update
    void Start() {
        invertY = (PlayerPrefs.GetInt("invertY", 0) != 0);
        mainVolumeSlider.value = PlayerPrefs.GetFloat("mainVolume", 1f);
        mainVolumeText.text = (Mathf.Round(mainVolumeSlider.value * 100)).ToString();
        sensitivitySlider.value = PlayerPrefs.GetFloat("sensitivity", 0.3f);
        sensitivityText.text = (Mathf.Round(sensitivitySlider.value * 100)).ToString();
        sensitivity = sensitivitySlider.value;
        UpdateVolume();
        UpdateSensitivity();

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
            resolutionSlider.value = PlayerPrefs.GetInt("resolution", resolutionIndex);
            UpdateResolution();
        } 
        else {
            Debug.LogError("Current screen resolution not found in list of valid resolutions! How?!");
        }
    }

    void Update() {
        if (optionsMenuOpen) {
            UpdateVolume();
            UpdateSensitivity();
            UpdateResolution();
        }
    }

    public void PauseMenu() {
        if (optionsMenuOpen) {
            OptionsMenu();
            return;
        }
        GamePaused = !GamePaused;
        UpdateCursor();
        pauseMenu.SetActive(GamePaused);
        Time.timeScale = GamePaused ? 0 : 1;
    }

    public void OptionsMenu() {
        optionsMenuOpen = !optionsMenuOpen;
        if (Time.timeScale == 0) { // If not on Main Menu
            pauseMenu.SetActive(!optionsMenuOpen);
        }
        optionsMenu.SetActive(optionsMenuOpen);
        if (!optionsMenuOpen) {
            ApplySettings();
            //pauseMenu.SetActive(true);
        }
    }

    public void ApplySettings() {
        UpdateVolume();
        UpdateSensitivity();
        UpdateResolution();
        PlayerPrefs.Save();
    }

    void UpdateCursor () {
        if (GamePaused) {
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
        mainVolumeText.text = (Mathf.Round(mainVolumeSlider.value * 100)).ToString();
        audioMgr.SetVolume(mainVolumeSlider.value); 
    }
    
    public void UpdateSensitivity() {
        sensitivity = sensitivitySlider.value;
        sensitivityText.text = (Mathf.Round(sensitivitySlider.value * 100)).ToString();
        PlayerPrefs.SetFloat("sensitivity", sensitivitySlider.value);
        // if (camFollow) {
        //     camFollow.SetSensitivity(sensitivitySlider.value);
        // }
    }

    public void UpdateResolution() {
        string newRes = ResToString(resolutions[resolutionIndex]);
        PlayerPrefs.SetInt("resolution", resolutionIndex);
        resolutionText.text = newRes;
        Screen.SetResolution(resolutions[resolutionIndex].width, resolutions[resolutionIndex].height, true);
    }

    public void UpdateInvertY() {
        // if (camFollow) {
        //     camFollow.FlipInvertY();
        // }
        invertY = invertYSelect.isOn;
        PlayerPrefs.SetInt("invertY", (invertY ? 1 : 0));
    }
    
    public void UpdateFreeAim() {
        freeAim = freeAimSelect.isOn;
        PlayerPrefs.SetInt("freeAim", (freeAim ? 1 : 0));
        reticle.gameObject.SetActive(freeAim);
    }

    public string ResToString(Resolution res) {
        return "Resolution: " + res.ToString();
    }

    public void Restart() {
        if (LevelManager.current != null) {
            LevelManager.current.TimeReset();
        }
        //BlkScreenFadeInOut(0.5f);
        // BlkScreenFadeIn(0.5f);
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        GameManager.Singleton.newScene(SceneManager.GetActiveScene().buildIndex);
        gameOverText.gameObject.SetActive(false);
        restartBtn.gameObject.SetActive(false);
        PauseMenu();
        // BlkScreenFadeOut(0.5f);
    }

    public void NextLevel() {
        LevelManager.current.TimeReset();
        int index = SceneManager.GetActiveScene().buildIndex;
        SaveLoad.Save(index);
        if (index < 3) {
            //BlkScreenFadeIn(0.5f);
            GameManager.Singleton.newScene(index + 1);
            winText.gameObject.SetActive(false);
            nextBtn.gameObject.SetActive(false);
            //audioMgr.AudioSwitch(index + 1);
            //SceneManager.LoadScene(index + 1);
            PauseMenu();
            //BlkScreenFadeOut(0.5f);
        }
    }

    public void Quit() {
        Application.Quit();
    }

    public void GameOver() {
        Time.timeScale = 0;
        GamePaused = true;
        UpdateCursor();
        gameOverText.gameObject.SetActive(true);
        restartBtn.gameObject.SetActive(true);
    }

    public void Win() {
        Time.timeScale = 0;
        GamePaused = true;
        UpdateCursor();
        
        if (SceneManager.GetActiveScene().buildIndex != 3) {
            winText.gameObject.SetActive(true);
            nextBtn.gameObject.SetActive(true);
        }
        else {
            FinishGame();
        }
    }

    public void FinishGame() {
        Time.timeScale = 0;
        GamePaused = true;
        UpdateCursor();
        finishGameText.gameObject.SetActive(true);
    }

    public void HoverButton(GameObject btn) {
        btn.transform.localScale = new Vector2(1.1f, 1.1f);
        btn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color(1f, 1f, 1f, 1f);
        Vector3 pos = btn.transform.position;
        btn.transform.position = new Vector3(pos.x, pos.y, pos.z);
    }

    public void LeaveButton(GameObject btn) {
        btn.transform.localScale = new Vector2(1f, 1f);
        btn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color32(203, 203, 203, 255);
        Vector3 pos = btn.transform.position;
        btn.transform.position = new Vector3(pos.x, pos.y, pos.z);
    }

    public void ControlFadeIn(string control) {
        if (control == "Jump") {
            StartCoroutine(fade.FadeTextInOut(0.25f, 1f, jumpText, 2f));
        }
        else if (control == "Checkpoint") {
            StartCoroutine(fade.FadeTextInOut(0.25f, 0.5f, cpText, 1f));
        }
        else {
            StartCoroutine(fade.FadeTextInOut(0.25f, 1f, swingText, 2f));
        }
    }

    public void ControlFadeOut(string control) {
        // if (control == "Jump") {
        //     StartCoroutine(fade.FadeTextToZeroAlpha(1f, jumpText));
        // }
        // else {
        //     StartCoroutine(fade.FadeTextToZeroAlpha(1f, swingText));
        // }
        StartCoroutine(fade.FadeTextToZeroAlpha(1f, moveText));
    }

    // public void BlkScreenFadeInOut(float t) {
    //     StartCoroutine(fade.FadeImageInOut(0.5f, 0.5f, blackScreen, t));
    // }

    // public void BlkScreenFadeIn(float t) {
    //     StartCoroutine(fade.FadeImageToFullAlpha(t, blackScreen));
    // }

    // public void BlkScreenFadeOut(float t) {
    //     StartCoroutine(fade.FadeImageToZeroAlpha(t, blackScreen));
    // }
}
