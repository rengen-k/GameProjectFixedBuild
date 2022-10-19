using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelSelector : MonoBehaviour
{

    public int level;
    private GameObject levelText; 
    [SerializeField] private PauseMenu pauseMenu;

    void Awake()
    {
        levelText = this.gameObject.transform.GetChild(0).gameObject;
        TMP_Text texty = levelText.GetComponent<TextMeshProUGUI>();
        texty.text = level.ToString();
    }

    public void OpenScene()
    {   
        pauseMenu.Resume();
        SceneManager.LoadScene("Level" + level.ToString());
    }
}
