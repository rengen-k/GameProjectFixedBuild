using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewGame : MonoBehaviour
{
 
    public void loadStart()
    {
        SceneManager.LoadScene("Level 0");
    }
}
