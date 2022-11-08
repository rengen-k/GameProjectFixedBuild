using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewGame : MonoBehaviour
{
    //Script to attach to first menu.
    public void loadStart()
    {
        SceneManager.LoadScene("Level 0");
    }
}
