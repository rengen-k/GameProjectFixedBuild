using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    // PauseMenu handler. Attached to canvas.

    private PlayerActionsScript playerActionsScript;
    [Tooltip("Fields representing status of pausemenu.")]
    public static bool GamePaused = false;
    [Tooltip("Fields representing status of pausemenu.")]
    public static bool helpToggle = false;
    [Tooltip("Fields representing status of pausemenu.")]
    public static bool levelsToggle = false;
    [Tooltip("Fields representing status of pausemenu.")]
    public static bool settingsToggle = false;
    [Tooltip("Fields representing status of pausemenu.")]
    public static bool graphicsToggle = false;
    [Tooltip("Fields representing status of pausemenu.")]
    public static bool soundToggle = false;

    private GameObject PauseMenuUI;
    private GameObject helpPanel;
    private GameObject levelPanel;
    private GameObject KeyPanel;
    private GameObject DialoguePanel;
    private GameObject settingsPanel;
    private GameObject graphicsPanel;
    private GameObject audioPanel;
    private GameObject CollectiblePanel;

    private Vector2 helpPosOn;
    private Vector2 helpPosOff;

    private Vector2 levelsPosOn;
    private Vector2 levelsPosOff;

    private bool pauseRequest;
    private bool firstTime;

    private DialogueManager dialogueManager;

    void Awake(){
        try {
        dialogueManager = GameObject.Find("DialogueManager").GetComponent<DialogueManager>();
        PauseMenuUI = transform.Find("PauseMenu").gameObject;
        helpPanel = transform.Find("PauseMenu/HelpScreen").gameObject;
        levelPanel = transform.Find("PauseMenu/LevelScreen").gameObject;
        KeyPanel = transform.Find("KeyPanel").gameObject;
        DialoguePanel = transform.Find("DialoguePanel").gameObject;
        settingsPanel = transform.Find("PauseMenu/SettingsScreen").gameObject;
        graphicsPanel = transform.Find("PauseMenu/GraphicsScreen").gameObject;
        audioPanel = transform.Find("PauseMenu/SoundScreen").gameObject;
        CollectiblePanel = transform.Find("CollectiblePanel").gameObject;
        }
        catch {

        }



        playerActionsScript = new PlayerActionsScript();
        playerActionsScript.Player.Enable();
        playerActionsScript.Player.Menu.performed += TriggerPause;

        int diff = GameObject.Find("GlobalGameState").GetComponent<GameState>().GetDifficulty();
        GameObject diffText = GameObject.Find("Diff Text");
        try {
            TMP_Text texty = diffText.GetComponent<TextMeshProUGUI>();
        

        
        if (diff == 0)
        {
            texty.text = "Diff: Easy";
        }
        else if (diff == 1){
            texty.text = "Diff: Normal";
        }
        else if (diff == 2)
        {
            texty.text = "Diff: Hard";
        }

        }
        catch {}
        
    }

    void Start(){
        
        firstTime = true;
        GameObject title = GameObject.Find("Level Title");
        TMP_Text texty = title.GetComponent<TextMeshProUGUI>();
        texty.text = "Level " + Int32.Parse(SceneManager.GetActiveScene().name.Split(" ")[1]);
    }

    private void OnEnable() {
        playerActionsScript.Player.Enable();
    }

    private void OnDisable() {
        playerActionsScript.Player.Disable();
    }

    void Update()
    {
        if (firstTime)
        {
            firstTime = false;
            PauseMenuUI.SetActive(false);
            Time.timeScale = 1f;
            GamePaused = false;
        }

        if (pauseRequest) {
            //Debug.Log("Trigger Pause menu");
            pauseRequest = false;
            if (GamePaused){
                Resume();
            }        
            else{
                Pause();
            }
        }
            
        if (helpToggle){
            helpPanel.SetActive(true);
        }
        else{
            helpPanel.SetActive(false);
        }

        if (levelsToggle){
            levelPanel.SetActive(true);
        }
        else{
            levelPanel.SetActive(false);
        }

        if(settingsToggle){
            settingsPanel.SetActive(true);
        }
        else{
            settingsPanel.SetActive(false);
        }

        if(graphicsToggle){
            graphicsPanel.SetActive(true);
        }
        else{
            graphicsPanel.SetActive(false);
        }

        if(soundToggle){
            audioPanel.SetActive(true);
        }
        else{
            audioPanel.SetActive(false);
        }

        

    }

    public void TriggerPause(InputAction.CallbackContext context) {
        pauseRequest = true;
    }


    public void Resume()
    {
        PauseMenuUI.SetActive(false);
        KeyPanel.SetActive(true);
        CollectiblePanel.SetActive(true);
        graphicsPanel.SetActive(false);
        settingsPanel.SetActive(false);
        audioPanel.SetActive(false);
        if (dialogueManager.dialogueIsPlaying)
        {
            DialoguePanel.SetActive(true);
        }
        

        Time.timeScale = 1f;
        GamePaused = false;
        GameObject.Find("Player").GetComponent<PlayerController>().MenuIncreaseHealth();
    }

    public void ResetLevel()
    {
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GamePaused = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void Pause()
    {
        PauseMenuUI.SetActive(true);
        KeyPanel.SetActive(false);
        CollectiblePanel.SetActive(false);
        graphicsPanel.SetActive(false);
        settingsPanel.SetActive(false);
        audioPanel.SetActive(false);
        if (dialogueManager.dialogueIsPlaying)
        {
            DialoguePanel.SetActive(false);
        }


        Time.timeScale = 0f;
        GamePaused = true;
        
    }

    
    public void LevelsToggle()
    {
        helpToggle = false;
        settingsToggle = false;
        levelsToggle = !levelsToggle;

    }

    public void Help()
    {   
        levelsToggle = false;
        settingsToggle = false;
        helpToggle = !helpToggle;
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Diff()
    {
        int diff = GameObject.Find("GlobalGameState").GetComponent<GameState>().IncrementDifficulty();
        GameObject diffText = GameObject.Find("Diff Text");
        TMP_Text texty = diffText.GetComponent<TextMeshProUGUI>();
        if (diff == 0)
        {
            texty.text = "Diff: Easy";
        }
        else if (diff == 1){
            texty.text = "Diff: Normal";
        }
        else if (diff == 2)
        {
            texty.text = "Diff: Hard";
        }

    }

    public void Settings()
    {
        levelsToggle = false;
        helpToggle = false;
        soundToggle = false;
        graphicsToggle = false;
        settingsToggle = true;
    }

    public void Graphics()
    {
        levelsToggle = false;
        helpToggle = false;
        settingsToggle = false;
        soundToggle = false;
        graphicsToggle = true;
    }

    public void Sound()
    {
        levelsToggle = false;
        helpToggle = false;
        settingsToggle = false;
        graphicsToggle = false;
        soundToggle = true;
    }
}
