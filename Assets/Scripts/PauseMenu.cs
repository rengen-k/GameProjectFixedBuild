using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    
    private PlayerActionsScript playerActionsScript;

    public static bool GamePaused = false;
    public static bool helpToggle = false;
    public static bool levelsToggle = false;
    
    public GameObject PauseMenuUI;
    public GameObject helpPanel;
    public GameObject levelPanel;

    private Vector2 helpPosOn;
    private Vector2 helpPosOff;

    private Vector2 levelsPosOn;
    private Vector2 levelsPosOff;

    private bool pauseRequest;

    void Start(){
        playerActionsScript = new PlayerActionsScript();
        playerActionsScript.Player.Enable();
        playerActionsScript.Player.Menu.performed += TriggerPause;

        helpPosOn = helpPanel.transform.TransformPoint(new Vector2(0f, 550f));
        helpPosOff = helpPanel.transform.TransformPoint(new Vector2(0f, 0f));
        levelsPosOn = levelPanel.transform.TransformPoint(new Vector2(0f, -550f));
        levelsPosOff = levelPanel.transform.TransformPoint(new Vector2(0f, 0f));
    }

    private void OnDisable() {
        playerActionsScript.Player.Disable();
    }

    void Update()
    {
        if (pauseRequest) {
            pauseRequest = false;
            if (GamePaused){
                Resume();
            }        
            else{
                Pause();
            }
        }
            

        Vector2 vel = Vector2.zero;
        
        
        if (helpToggle){
            helpPanel.transform.position = Vector2.Lerp(helpPanel.transform.position, helpPosOn, Time.fixedDeltaTime);
        }
        else{
            helpPanel.transform.position = Vector2.Lerp(helpPanel.transform.position, helpPosOff, Time.fixedDeltaTime);
        }

        if (levelsToggle){
            levelPanel.transform.position = Vector2.Lerp(levelPanel.transform.position, levelsPosOn, Time.fixedDeltaTime);
        }
        else{
            levelPanel.transform.position = Vector2.Lerp(levelPanel.transform.position, levelsPosOff, Time.fixedDeltaTime);
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
}
