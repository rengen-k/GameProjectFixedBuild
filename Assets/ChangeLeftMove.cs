using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeLeftMove : MonoBehaviour
{

    private PlayerActionsScript playerActionsScript;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeLeftMoveButton()
    {
        playerActionsScript = new PlayerActionsScript();
        //print(playerActionsScript.inputactions);
    }

}
