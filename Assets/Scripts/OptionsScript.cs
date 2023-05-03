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
     
    void Awake() {
        pauseMenu = GameObject.Find("PauseMenu");
        optionsMenu = GameObject.Find("OptionsMenu");
        mainVolumeSlider = GameObject.Find("MainVolumeSlider").GetComponent<Slider>();
        mainVolumeText = GameObject.Find("MainVolumeText").GetComponent<TextMeshProUGUI>();
    }

    // Start is called before the first frame update
    void Start() {
        mainVolumeSlider.value = PlayerPrefs.GetFloat("mainVolume", 1f);
        mainVolumeText.text = (Mathf.Round(mainVolumeSlider.value * 100)).ToString();
        optionsMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update() {
        mainVolumeText.text = (Mathf.Round(mainVolumeSlider.value * 100)).ToString();
    }

    public void Volume() {
        PlayerPrefs.SetFloat("mainVolume", mainVolumeSlider.value);
    }
}
