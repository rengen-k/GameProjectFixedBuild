using System.Collections;
using System.Collections.Generic;
using Ink.Parsed;
using UnityEngine;

public class PerspectivePlatforms : MonoBehaviour
{
    private Transform player;
    private CameraSwitchScript camScript;
    private Vector3 target;
    private float smoothTime = 0.2f;
    private Vector3 velocity = Vector3.zero;
    private Vector3 originalPos;
    [SerializeField] private float tower0;
    [SerializeField] private float tower1;
    [SerializeField] private float tower2;
    [SerializeField] private float tower3;
    private bool camChange;
    private float lastCamPos = 0;
    private Vector3 playerLocalPos;
    private static bool onBlock;

    private void Start()
    {
        player = GameObject.Find("Player").transform;
        camScript = GameObject.Find("StateDrivenCamera").GetComponent<CameraSwitchScript>();
        originalPos = transform.position;
    }

    private void Update()
    {
        if (lastCamPos != camScript.cameraPos)
        {
            camChange = true;
            lastCamPos = camScript.cameraPos;
        }
        else
        {
            camChange = false;
        }
        if (camChange)
        {
            StartCoroutine(moveBlocks());
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.transform.SetParent(transform);
            onBlock = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            onBlock = false;
        }
    }

    private IEnumerator moveBlocks()
    {
        if (camScript.cameraPos == 0 || camScript.cameraPos == 2)
        {
            if (camScript.cameraPos == 0 && onBlock)
            {
                target = new Vector3(originalPos.x, originalPos.y, tower0);
            }
            else if (camScript.cameraPos == 2 && onBlock)
            {
                target = new Vector3(originalPos.x, originalPos.y, tower2);
            }
            else
            {
                target = new Vector3(originalPos.x, originalPos.y, player.position.z);
            }
            playerLocalPos = player.localPosition;
            while (Vector3.Distance(transform.position, target) > 0.1)
            {
                transform.position = Vector3.SmoothDamp(transform.position, target, ref velocity, smoothTime);
                player.localPosition = playerLocalPos;
                yield return null;
            }
        }
        else if (camScript.cameraPos == 1 || camScript.cameraPos == 3)
        {
            if (camScript.cameraPos == 1 && onBlock)
            {
                target = new Vector3(tower1, originalPos.y, originalPos.z);
            }
            else if (camScript.cameraPos == 3 && onBlock)
            {
                target = new Vector3(tower3, originalPos.y, originalPos.z);
            }
            else
            {
                target = new Vector3(player.position.x, originalPos.y, originalPos.z);
            }
            playerLocalPos = player.localPosition;
            while (Vector3.Distance(transform.position, target) > 0.1)
            {
                transform.position = Vector3.SmoothDamp(transform.position, target, ref velocity, smoothTime);
                player.localPosition = playerLocalPos;
                yield return null;
            }
        }
    }
}
