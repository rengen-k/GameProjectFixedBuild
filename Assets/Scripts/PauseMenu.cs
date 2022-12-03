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

    private GameObject PauseMenuUI;
    private GameObject helpPanel;
    private GameObject levelPanel;

    private Vector2 helpPosOn;
    private Vector2 helpPosOff;

    private Vector2 levelsPosOn;
    private Vector2 levelsPosOff;

    private bool pauseRequest;
    private bool firstTime;

    void Awake(){

        PauseMenuUI = transform.Find("PauseMenu").gameObject;
        helpPanel = transform.Find("PauseMenu/HelpScreen").gameObject;
        levelPanel = transform.Find("PauseMenu/LevelScreen").gameObject;

        playerActionsScript = new PlayerActionsScript();
        playerActionsScript.Player.Enable();
        playerActionsScript.Player.Menu.performed += TriggerPause;

        int diff = GameObject.Find("GlobalGameState").GetComponent<GameState>().GetDifficulty();
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

    void Start(){
        
        firstTime = true;
        GameObject title = GameObject.Find("Level Title");
        TMP_Text texty = title.GetComponent<TextMeshProUGUI>();
        texty.text = "Level " + Int32.Parse(SceneManager.GetActiveScene().name.Split(" ")[1]);
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
        

    }

    public void TriggerPause(InputAction.CallbackContext context) {
        pauseRequest = true;
    }


    public void Resume()
    {
        PauseMenuUI.SetActive(false);
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
        Time.timeScale = 0f;
        GamePaused = true;
        
    }

    
    public void LevelsToggle()
    {
        helpToggle = false;
        levelsToggle = !levelsToggle;

    }

    public void Help()
    {   
        levelsToggle = false;
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
}
