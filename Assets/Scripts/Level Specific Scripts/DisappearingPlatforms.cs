using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearingPlatforms : Triggerable
{
    // platforms with this script are visible only when a button is pressed

    private MeshRenderer meshRenderer;

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.enabled = false;
    }

    public override void triggerAct()
    {
        meshRenderer.enabled = true;
    }

    public override void triggerUnAct()
    {
        meshRenderer.enabled = false;
    }
}
