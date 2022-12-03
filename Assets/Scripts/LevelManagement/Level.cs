using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Level : MonoBehaviour
{
    // Attached to level button, decides if open or not, colours and loads select scene.
    // Mark open and closed levels
    [Tooltip("Number Level loads to.")]
    [SerializeField]private int num;

    public enum stat
    {
        closed,
        open,
        done,
        collected
    }

    public stat status = stat.closed;

    private GameObject levelText; 
    private LevelSet parentSet;

    [Tooltip("Reference to pauseMenu in canvas")]
    [SerializeField] private PauseMenu pauseMenu;
    
    void Awake()
    {
        levelText = this.gameObject.transform.GetChild(0).gameObject;
        TMP_Text texty = levelText.GetComponent<TextMeshProUGUI>();
        texty.text = num.ToString();
        parentSet = this.transform.parent.GetComponent<LevelSet>();
    }

    void Start()
    {
        parentSet = this.transform.parent.GetComponent<LevelSet>();
    }
    //ORGANISE
    void Update()
    {
    }

    public void updateVisuals()
    {
        switch (status)
        {
            case stat.closed:
            gameObject.GetComponent<Image>().color = Color.grey;
            break; 
            case stat.done: 
            gameObject.GetComponent<Image>().color = Color.green;
            break;
            case stat.collected: 
            gameObject.GetComponent<Image>().color = Color.yellow;
            break;
            default:
            gameObject.GetComponent<Image>().color = Color.white;
            break;
        }

    }

    public void OpenScene()
    {   
        if (parentSet.isOpen()){
            pauseMenu.Resume();
            SceneManager.LoadScene("Level " + num.ToString());
        }
    }

    public void SetValues(bool d, bool c)
    {
        
        if (parentSet.isOpen())
        {

            status = stat.open;
        }
        if (d)
        {
            status = stat.done;
        }
        if (c)
        {
            status = stat.collected;
        }
        updateVisuals();
    }

    public int GetLevel()
    {
        return num;
    }

    public bool GetDone()
    {
        return status == stat.done || status == stat.collected; 
    }
}
