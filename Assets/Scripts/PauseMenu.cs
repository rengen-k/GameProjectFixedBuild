using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


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
    private bool firstTime;

    void Awake(){
        playerActionsScript = new PlayerActionsScript();
        playerActionsScript.Player.Enable();
        playerActionsScript.Player.Menu.performed += TriggerPause;
        
        //Code meant to make the panals slide in from off screen, todo, figure out how to translate points 
        helpPosOn = helpPanel.transform.TransformPoint(new Vector2(0f, 1950f)); //Off: local y = -2900  On, local y = -950
        helpPosOff = helpPanel.transform.TransformPoint(new Vector2(0f, 0f));
        levelsPosOn = levelPanel.transform.TransformPoint(new Vector2(0f, -2000f));
        levelsPosOff = levelPanel.transform.TransformPoint(new Vector2(0f, 0f));

        
    }

    void Start(){
        
        firstTime = true;
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
            

        Vector2 vel = Vector2.zero;
        
        /*
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
        */

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
}
