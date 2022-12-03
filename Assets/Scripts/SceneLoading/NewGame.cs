using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewGame : MonoBehaviour
{
    //Script to attach to first menu.
    public void loadStart()
    {
        GameObject.Find("GlobalGameState").GetComponent<GameState>().SaveGame();
        SceneManager.LoadScene("Level 0");
    }
}
