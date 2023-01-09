using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
public class NewGame : MonoBehaviour
{
    //Script to attach to first menu.

    private GameObject confPanal;

    void Start()
    {
        confPanal = GameObject.Find("Confirmation");
        confPanal.SetActive(false);
    }

    public void buttonPress()
    {
        if (File.Exists(Application.persistentDataPath + "/Save.dat"))
        {
            confPanal.SetActive(true);
        }
        else
        {
            GameObject.Find("GlobalGameState").GetComponent<GameState>().SaveGame();
            loadStart();
        }
    }

    public void loadStart()
    {
        SceneManager.LoadScene("LevelSelector");
    }

    public void closePanal()
    {
        confPanal.SetActive(false);
    }

}
