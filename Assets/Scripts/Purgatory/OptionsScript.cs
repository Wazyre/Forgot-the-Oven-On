using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionsScript : MonoBehaviour
{
    private float mainVolume;
    private GameObject optionsMenu;
    private GameObject pauseMenu;
    private Slider mainVolumeSlider;
    private TextMeshProUGUI mainVolumeText;
    private Slider sensitivitySlider;
    private TextMeshProUGUI sensitivityText;
    private Selectable invertYToggle;
    private CameraFollow camFollow;
    private AudioSource gameAudio;
     
    void Awake() {
        pauseMenu = GameObject.Find("PauseMenu");
        optionsMenu = GameObject.Find("OptionsMenu");
        mainVolumeSlider = GameObject.Find("MainVolumeSlider").GetComponent<Slider>();
        mainVolumeText = GameObject.Find("MainVolumeText").GetComponent<TextMeshProUGUI>();
        sensitivitySlider = GameObject.Find("LookSensitivitySlider").GetComponent<Slider>();
        sensitivityText = GameObject.Find("LookSensitivityText").GetComponent<TextMeshProUGUI>();
        //invertYToggle = GameObject.Find("InvertY").GetComponent<>();
        camFollow = GameObject.FindWithTag("Dolly").GetComponent<CameraFollow>();
        gameAudio = GameObject.Find("GameManager").GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start() {
        mainVolumeSlider.value = 0.6f; //PlayerPrefs.GetFloat("mainVolume", 0.6f);
        mainVolumeText.text = (Mathf.Round(mainVolumeSlider.value * 100)).ToString();
        sensitivitySlider.value = 0.3f; //PlayerPrefs.GetFloat("sensitivity", 0.3f);
        sensitivityText.text = (Mathf.Round(sensitivitySlider.value * 100)).ToString();
        optionsMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update() {
        mainVolumeText.text = (Mathf.Round(mainVolumeSlider.value * 100)).ToString();
        sensitivityText.text = (Mathf.Round(sensitivitySlider.value * 100)).ToString();
    }

    public void Volume() {
        PlayerPrefs.SetFloat("mainVolume", mainVolumeSlider.value);
        gameAudio.volume = mainVolumeSlider.value;
        
    }
    
    public void Sensitivity() {
        PlayerPrefs.SetFloat("sensitivity", sensitivitySlider.value);
        //camFollow.SetSensitivity(sensitivitySlider.value);
    }

    public void Resolution() {
        
    }
}
