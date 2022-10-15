using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    
    public static bool GamePaused = false;
    public static bool helpToggle = false;
    public static bool levelsToggle = false;
    
    public GameObject PauseMenuUI;
    public GameObject helpPanal;
    public GameObject levelPanel;

    private Vector2 helpPosOn;
    private Vector2 helpPosOff;

    private Vector2 levelsPosOn;
    private Vector2 levelsPosOff;

    // Update is called once per frame

    void Start(){
        helpPosOn = helpPanal.transform.TransformPoint(new Vector2(0f, 550f));
        helpPosOff = helpPanal.transform.TransformPoint(new Vector2(0f, 0f));
        levelsPosOn = levelPanel.transform.TransformPoint(new Vector2(0f, -550f));
        levelsPosOff = levelPanel.transform.TransformPoint(new Vector2(0f, 0f));
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)){
            if (GamePaused){
                Resume();
            }        
            else{
                Pause();
            }
        }
        Vector2 vel = Vector2.zero;
        
        
        if (helpToggle){
            helpPanal.transform.position = Vector2.Lerp(helpPanal.transform.position, helpPosOn, Time.fixedDeltaTime);
        }
        else{
            helpPanal.transform.position = Vector2.Lerp(helpPanal.transform.position, helpPosOff, Time.fixedDeltaTime);
        }

        if (levelsToggle){
            levelPanel.transform.position = Vector2.Lerp(levelPanel.transform.position, levelsPosOn, Time.fixedDeltaTime);
        }
        else{
            levelPanel.transform.position = Vector2.Lerp(levelPanel.transform.position, levelsPosOff, Time.fixedDeltaTime);
        }
        

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
        
    }
}
