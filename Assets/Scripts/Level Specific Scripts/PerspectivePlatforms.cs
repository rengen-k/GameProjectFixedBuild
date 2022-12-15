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
    [SerializeField] private bool tower;
    [SerializeField] private float tower0;
    [SerializeField] private float tower1;
    [SerializeField] private float tower2;
    [SerializeField] private float tower3;

    private void Start()
    {
        player = GameObject.Find("Player").transform;
        camScript = GameObject.Find("StateDrivenCamera").GetComponent<CameraSwitchScript>();
        originalPos = transform.position;
    }

    private void FixedUpdate()
    {
        if (!tower)
        {
            if (camScript.cameraPos == 0 || camScript.cameraPos == 2)
            {
                target = new Vector3(originalPos.x, originalPos.y, player.position.z);
                transform.position = Vector3.SmoothDamp(transform.position, target, ref velocity, smoothTime);
            }
            else if (camScript.cameraPos == 1 || camScript.cameraPos == 3)
            {
                target = new Vector3(player.position.x, originalPos.y, originalPos.z);
                transform.position = Vector3.SmoothDamp(transform.position, target, ref velocity, smoothTime);
            }
        }

        if (tower)
        {
            if (camScript.cameraPos == 0)
            {
                target = new Vector3(originalPos.x, originalPos.y, tower0);
                transform.position = Vector3.SmoothDamp(transform.position, target, ref velocity, smoothTime);
            }
            else if (camScript.cameraPos == 1)
            {
                target = new Vector3(tower1, originalPos.y, originalPos.z);
                transform.position = Vector3.SmoothDamp(transform.position, target, ref velocity, smoothTime);
            }
            else if (camScript.cameraPos == 2)
            {
                target = new Vector3(originalPos.x, originalPos.y, tower2);
                transform.position = Vector3.SmoothDamp(transform.position, target, ref velocity, smoothTime);
            }
            else if (camScript.cameraPos == 3)
            {
                target = new Vector3(tower3, originalPos.y, originalPos.z);
                transform.position = Vector3.SmoothDamp(transform.position, target, ref velocity, smoothTime);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.transform.SetParent(transform);
        }
    }
}
