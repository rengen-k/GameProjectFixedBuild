using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Level : MonoBehaviour
{
    // Attached to level button, decides if open or not, colours and loads select scene.
    private int num;
    private bool open = false;
    private bool done;
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

    public void Finish()
    {
        done = true;
    }

    public int getLevel()
    {
        return num;
    }

    public bool isDone()
    {
        return done;
    }
}
