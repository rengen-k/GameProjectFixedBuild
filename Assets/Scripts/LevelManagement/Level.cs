using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Level : MonoBehaviour
{
    // Attached to level button, decides if open or not, colours and loads select scene.
    [Tooltip("Number Level loads to.")]
    [SerializeField]private int num;
    private bool done;
     private bool collected;

    private GameObject levelText; 
    private LevelSet parentSet;

    [Tooltip("Reference to pauseMenu in canvas")]
    [SerializeField] private PauseMenu pauseMenu;
    
    void Awake()
    {
        levelText = this.gameObject.transform.GetChild(0).gameObject;
        TMP_Text texty = levelText.GetComponent<TextMeshProUGUI>();
        texty.text = num.ToString();
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
        if (done)
        {
            gameObject.GetComponent<Image>().color = Color.green;
        }
        if (collected)
        {
            gameObject.GetComponent<Image>().color = Color.yellow;
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
        Debug.Log("This is " + transform.name + " setting values as " + d + ", " + c);
        done = d;
        collected = c;
        updateVisuals();
    }

    public void Finish()
    {
        done = true;
    }

    public int GetLevel()
    {
        return num;
    }

    public bool GetDone()
    {
        return done;
    }
}
