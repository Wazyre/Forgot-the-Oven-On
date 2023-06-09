using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [Header("Main UI Elements")]
    [SerializeField] TextMeshProUGUI title;
    [SerializeField] Button newGameBtn;
    [SerializeField] Button continueBtn;
    [SerializeField] Button optionsBtn;
    [SerializeField] Button quitBtn;
    [SerializeField] Button backBtn;
    [SerializeField] GameObject gameCanvas;
    [SerializeField] GameObject player;

    [Header("Difficulty")]
    [SerializeField] bool easyOn = false;
    [SerializeField] bool normalOn = false;
    [SerializeField] bool hardOn = false;
    [SerializeField] Image easyImage;
    [SerializeField] TextMeshProUGUI easyText;
    [SerializeField] Image normalImage;
    [SerializeField] TextMeshProUGUI normalText;
    [SerializeField] Image hardImage;
    [SerializeField] TextMeshProUGUI hardText;
    [SerializeField] TextMeshProUGUI diffTitle;

    [Header("Game Menu UI Elements")]
    [SerializeField] Menu menu;
    [SerializeField] AudioManager audioMgr;
    // [SerializeField] AudioSource gameAudio1;
    [SerializeField] bool optionsMenuOpen = false;
    [SerializeField] TextMeshProUGUI controls;
    [SerializeField] TextMeshProUGUI timer;
    [SerializeField] TextMeshProUGUI jumpText;
    [SerializeField] TextMeshProUGUI swingText;
    [SerializeField] TextMeshProUGUI moveText;
    [SerializeField] TextMeshProUGUI checkpointText;

    void Awake() {
        diffTitle.gameObject.SetActive(false);
        easyImage.gameObject.SetActive(false);
        easyText.gameObject.SetActive(false);
        normalImage.gameObject.SetActive(false);
        normalText.gameObject.SetActive(false);
        hardImage.gameObject.SetActive(false);
        hardText.gameObject.SetActive(false);
        backBtn.gameObject.SetActive(false);
        timer.gameObject.SetActive(false);
    }

    void Start() {
        audioMgr.AudioSwitch(0);
    }

    public void NewGame() {
        title.gameObject.SetActive(false);
        newGameBtn.gameObject.SetActive(false);
        continueBtn.gameObject.SetActive(false);
        optionsBtn.gameObject.SetActive(false);
        quitBtn.gameObject.SetActive(false);
        player.SetActive(false);

        diffTitle.gameObject.SetActive(true);
        easyImage.gameObject.SetActive(true);
        normalImage.gameObject.SetActive(true);
        hardImage.gameObject.SetActive(true);
        backBtn.gameObject.SetActive(true);
    }

    public void ContinueGame() {

    }

    public void StartGame() {
        //fade.FadeImageToFullAlpha(1f, blackScreen);

        if (LevelManager.current == null) {
            SaveLoad.NewGame(0);
            LevelManager.current.isSceneBeingLoaded = true;
            
            // menu.BlkScreenFadeIn(1f);
            gameCanvas.SetActive(true);
            controls.gameObject.SetActive(true);
            timer.gameObject.SetActive(true);
            jumpText.gameObject.SetActive(true);
            swingText.gameObject.SetActive(true);
            moveText.gameObject.SetActive(true);
            checkpointText.gameObject.SetActive(true);
            // SceneManager.LoadScene(1); // First Level
            //audioMgr.AudioSwitch(1);
            GameManager.Singleton.newScene(1);
            // menu.BlkScreenFadeOut(1f);
            Debug.Log("ere");
        }
    }

    public void OptionsMenu() {
        optionsMenuOpen = true;
        title.gameObject.SetActive(false);
        newGameBtn.gameObject.SetActive(false);
        continueBtn.gameObject.SetActive(false);
        optionsBtn.gameObject.SetActive(false);
        quitBtn.gameObject.SetActive(false);
        backBtn.gameObject.SetActive(true);
        player.SetActive(false);
        menu.OptionsMenu();
    }

    public void ReturnToMain() {
        diffTitle.gameObject.SetActive(false);
        easyImage.gameObject.SetActive(false);
        easyText.gameObject.SetActive(false);
        normalImage.gameObject.SetActive(false);
        normalText.gameObject.SetActive(false);
        hardImage.gameObject.SetActive(false);
        hardText.gameObject.SetActive(false);

        if (optionsMenuOpen) {
            optionsMenuOpen = false;
            menu.OptionsMenu();
        }
        
        title.gameObject.SetActive(true);
        newGameBtn.gameObject.SetActive(true);
        continueBtn.gameObject.SetActive(true);
        optionsBtn.gameObject.SetActive(true);
        quitBtn.gameObject.SetActive(true);
        player.SetActive(true);
        backBtn.gameObject.SetActive(false);
    }

    public void ToggleEasy() {
        easyOn = !easyOn;
        easyImage.rectTransform.localScale = easyOn ? new Vector2(1.1f, 1.1f) : new Vector2(1f, 1f);
        easyText.gameObject.SetActive(easyOn);
    }

    public void ToggleNormal() {
        normalOn = !normalOn;
        normalImage.rectTransform.localScale = normalOn ? new Vector2(1.1f, 1.1f) : new Vector2(1f, 1f);
        normalText.gameObject.SetActive(normalOn);
    }

    public void ToggleHard() {
        hardOn = !hardOn;
        hardImage.rectTransform.localScale = hardOn ? new Vector2(1.1f, 1.1f) : new Vector2(1f, 1f);
        hardText.gameObject.SetActive(hardOn);
    }

    public void SetDifficulty() {
        if (easyOn) {
            PlayerPrefs.SetInt("difficulty", 0);
        }
        else if (normalOn) {
            PlayerPrefs.SetInt("difficulty", 1);
        }
        else if (hardOn) {
            PlayerPrefs.SetInt("difficulty", 2);
        }
        StartGame();
    }

    public void HoverButton(GameObject btn) {
        btn.transform.localScale = new Vector2(1.1f, 1.1f);
        btn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color(1f, 1f, 1f, 1f);
    }

    public void LeaveButton(GameObject btn) {
        btn.transform.localScale = new Vector2(1f, 1f);
        btn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color(0f, 0f, 0f, 1f); //new Color32(203, 203, 203, 255);
    }
}