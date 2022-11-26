using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TrampolineEnemy : MonoBehaviour
{
    private bool squashed = false;
    private bool squashCooldown = false;
    private Vector3 squashedScale = new Vector3(1f, 0.5f, 1f);
    private Vector3 normalScale = new Vector3(1f, 1f, 1f);
    private NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponentInParent<NavMeshAgent>();
    }

    private void Update()
    {
        if (squashed)
        {
            agent.updatePosition = false;
            agent.updateRotation = false;
            agent.isStopped = true;
        }
        else
        {
            squashed = false;
            transform.parent.transform.localScale = normalScale;
            gameObject.tag = "Ground";
            agent.updatePosition = true;
            agent.updateRotation = true;
            agent.isStopped = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (!squashed)
            {
                squashed = true;
                transform.parent.transform.localScale = squashedScale;
                gameObject.tag = "JumpTag";
                StartCoroutine(SquashCooldown());
            }
            //else if (squashed && !squashCooldown)
            //{
            //    squashed = false;
            //    transform.parent.transform.localScale = normalScale;
            //    gameObject.tag = "Ground";
            //}
        }
    }

    private IEnumerator SquashCooldown()
    {
        squashed = true;
        yield return new WaitForSeconds(4);
        squashed = false;
    }
}
