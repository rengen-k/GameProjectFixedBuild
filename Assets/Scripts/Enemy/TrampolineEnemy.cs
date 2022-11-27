using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TrampolineEnemy : MonoBehaviour
{
    private NavMeshAgent agent;
    public Transform respawnPoint;
    private Vector3 squashedScale = new Vector3(1f, 0.5f, 1f);
    private Vector3 normalScale = new Vector3(1f, 1f, 1f);
    private Vector3 scale;
    private bool squashed = false;

    private void Start()
    {
        agent = GetComponentInParent<NavMeshAgent>();
    }

    private void FixedUpdate()
    {
        // pause the NavMeshAgent when squashed
        if (squashed)
        {
            //StartCoroutine(smoothScaling());
            scale = normalScale;
            agent.updatePosition = false;
            agent.updateRotation = false;
            agent.isStopped = true;
        }
        // resume the NavMeshAgent and reset the scale and tag
        else
        {
            scale = squashedScale;
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
            // if the player jumps on the trampoline enemy it gets squashed and its head becomes a trampoline
            if (!squashed)
            {
                //transform.parent.transform.localScale = squashedScale;
                StartCoroutine(smoothScaling());
                squashed = true;
                gameObject.tag = "JumpTag";
                StartCoroutine(SquashCooldown());
            }
            else if (collision.gameObject.name == "KillPlane")
            {
                Respawn();
            }
        }
    }

    public void Respawn()
    {
        transform.position = respawnPoint.position;
    }

    private IEnumerator smoothScaling()
    {
        while (Vector3.Distance(transform.parent.transform.localScale, scale) > 0.05f)
        {
            yield return transform.parent.transform.localScale = Vector3.Lerp(transform.parent.transform.localScale, scale, 10f * Time.fixedDeltaTime);
        }
    }

    // keeps the enemy squashed for a set amount of time
    private IEnumerator SquashCooldown()
    {
        squashed = true;
        yield return new WaitForSeconds(4);
        squashed = false;
    }
}
