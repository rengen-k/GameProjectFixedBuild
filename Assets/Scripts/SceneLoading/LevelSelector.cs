using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelSelector : MonoBehaviour
{
    // Script attached to a level button in the pausemenu, handles loading the level selected by clicking the UI button.
    public int level;
    private bool open = false;
    private GameObject levelText; 
    [Tooltip("Reference to pauseMenu in canvas")]
    [SerializeField] private PauseMenu pauseMenu;

    void Awake()
    {
        levelText = this.gameObject.transform.GetChild(0).gameObject;
        TMP_Text texty = levelText.GetComponent<TextMeshProUGUI>();
        texty.text = level.ToString();
    }

    public void OpenScene()
    {   
        if (open){
            pauseMenu.Resume();
            SceneManager.LoadScene("Level " + level.ToString());
        }
        
    }

    public void Open()
    {
        open = true;
    }
}
