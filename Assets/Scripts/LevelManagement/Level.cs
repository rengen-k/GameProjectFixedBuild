using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level
{
    // Start is called before the first frame update
    private int num;
    private bool done;
    
    public Level (int level)
    {
        num = level;
        done = false;
    }

    public Level (int level, bool d)
    {
        num = level;
        done = true;
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
