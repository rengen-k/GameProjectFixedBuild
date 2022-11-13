using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSet : MonoBehaviour
{
    // Check all internal levels, which is done? if quota amount is done, return should Open next set
    // level set starts with levels.
    private Level levels;
    [Tooltip("The amount of levels inside a set that needs to be completed before the next set opens.")]
    [SerializeField] private int quota;
    
    private bool open;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool isOpen()
    {
        return open;
    }

    public bool isDone()
    {
        int dones = 0;
        foreach (Transform child in transform)
        {
            if (child.GetComponent<Level>() != null)
            {
                if (child.GetComponent<Level>().GetDone())
                {
                    dones++;
                }
            }
        }
        return dones >= quota;
    }

    public void setLevel(bool done, bool collected, int index)
    {
        transform.GetChild(index).GetComponent<Level>().SetValues(done, collected);
    }

    public void ForceOpen()
    {
        open = true;
    }

    
}
