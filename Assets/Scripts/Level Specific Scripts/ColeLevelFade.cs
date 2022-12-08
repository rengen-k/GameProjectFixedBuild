using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ColeLevelFade : MonoBehaviour
{
    /* this script is only used for level 20 because the normal object fader script only fades objects directly on the
    path from the player to the camera which doesn't account for the large complicated blocks of level 20 that inhibit
    vision on certain sides of other blocks */

    private Transform player;
    private CameraSwitchScript cameraSwitchScript;
    public bool faded;

    private void Start()
    {
        player = GameObject.Find("Player").transform;
        cameraSwitchScript = GameObject.Find("StateDrivenCamera").GetComponent<CameraSwitchScript>();
    }

    private void Update()
    {
        // fades one entire block of the level depending on where the player is and what camera is active
        if (player.position.x < 34 && player.position.y > 8 && cameraSwitchScript.cameraPos == 1)
        {
            if (transform.position.x > 34 && transform.position.y > 8)
            {
                Fade();
            }
        }
        else if (player.position.x < 34 && player.position.y < 8 && cameraSwitchScript.cameraPos == 1)
        {
            if (transform.position.x > 34 && transform.position.y < 8)
            {
                Fade();
            }
        }
        else if (player.position.x > 34 && player.position.y > 8 && cameraSwitchScript.cameraPos == 3)
        {
            if (transform.position.x < 34 && transform.position.y > 8)
            {
                Fade();
            }
        }
        else if (player.position.x > 34 && player.position.y < 8 && cameraSwitchScript.cameraPos == 3)
        {
            if (transform.position.x < 34 && transform.position.y < 8)
            {
                Fade();
            }
        }
        if (cameraSwitchScript.cameraPos != 1 && cameraSwitchScript.cameraPos != 3)
        {
            Unfade();
        }
    }

    // same as the method in ObjectFader.cs
    public void Fade()
    {
        faded = true;

        Renderer objectRenderer = GetComponent<Renderer>();

        foreach (Material material in objectRenderer.materials)
        {
            MaterialObjectFade.MakeFade(material);

            Color newColor = material.color;
            newColor.a = 0.2f;
            material.color = newColor;
        }
    }

    // same as the method in ObjectFader.cs
    private void Unfade()
    {
        Renderer objectRenderer = GetComponent<Renderer>();
        foreach (Material material in objectRenderer.materials)
        {
            MaterialObjectFade.MakeOpaque(material);

            Color newColor = material.color;
            newColor.a = 1f;
            material.color = newColor;
        }
    }
}
