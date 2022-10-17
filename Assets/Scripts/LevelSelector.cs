using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelSelector : MonoBehaviour
{

    public int level;
    private GameObject levelText; 
    void Awake()
    {
        levelText = this.gameObject.transform.GetChild(0).gameObject;
        TMP_Text texty = levelText.GetComponent<TextMeshProUGUI>();
        texty.text = level.ToString();
    }

    public void OpenScene()
    {
        SceneManager.LoadScene("Level" + level.ToString());
    }
}
