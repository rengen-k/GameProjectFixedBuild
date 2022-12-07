using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ColeLevelFade : MonoBehaviour
{
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
        //else
        //{
        //    Unfade();
        //}
    }

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
