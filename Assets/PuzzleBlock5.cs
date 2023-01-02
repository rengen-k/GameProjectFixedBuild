using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//-----------------------------------------//
// PuzzleBlock3
//-----------------------------------------//

public class PuzzleBlock5 : Triggerable
{
    [SerializeField] private GameObject cube;
    [SerializeField] private float rotation;
    [SerializeField] private GameObject[] oldButtons;
    [SerializeField] private GameObject[] newButtons;
    private bool isTrigger = true;
    private bool rotate = false;
    private Quaternion nextRotation;
    private int degree = 30;
    public string tag;
    private int turns = 30;
    public GameObject triggerCube;
    float magnitude = 6f;


    void Start()
    {
        nextRotation = transform.rotation * Quaternion.Euler(0, 0, degree);
    }

    void Update()
    {

        if (turns >= 360)
        {
            turns -= 360;
        } else if (turns <= -360)
        {
            turns += 360;
        }

        if (turns == 270)
        {
            triggerCube.GetComponent<Animator>().SetTrigger("Animate");
            cube.SetActive(true);
            cube.GetComponent<Rigidbody>().AddForce(Vector3.back * magnitude, ForceMode.Impulse);
            StartCoroutine(Wait());
            foreach (GameObject i in oldButtons)
            {
                i.SetActive(false);
            }

            foreach (GameObject j in newButtons)
            {
                j.SetActive(true);
            }
        }
        if (rotate) {
            transform.rotation = Quaternion.Slerp(transform.rotation, nextRotation, Time.deltaTime * 8);
            //transform.rotation = Quaternion.Slerp(transform.rotation, nextRotation, 1.0f);
            if (transform.rotation == nextRotation) {
                rotate = false;
            }
        }
    }

    public override void triggerAct()
    {
        if (isTrigger) {
            print(turns);
            isTrigger = false;
            Debug.Log("rotate");
            if (tag == "IncreaseButton")
            {
                degree = 24;
                turns += 24;
            } else if (tag == "DecreaseButton")
            {
                degree = -24;
                turns -= 24;
            }
            print(turns);
            if (rotate) {
                nextRotation = nextRotation * Quaternion.Euler(0, 0, degree);
            } else {
                nextRotation = transform.rotation * Quaternion.Euler(0, 0, degree);
            }
            rotate = true;
        }
    }

    public override void triggerUnAct()
    {
        isTrigger = true;
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(3);
        cube.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
    }
}