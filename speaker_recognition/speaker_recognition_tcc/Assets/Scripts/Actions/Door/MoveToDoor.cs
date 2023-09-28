using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveToDoor : MonoBehaviour
{
    private GameObject door;
    private NavMeshAgent agent;
    private Animator playerAnimator;
    private Animator doorAnimator;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        playerAnimator = GetComponent<Animator>();

        door = GameObject.FindGameObjectWithTag("door");  // Find object with the "door" tag
        if (door)
        {
            doorAnimator = door.GetComponent<Animator>();
        }
    }

    public void PlayerMoveToDoor()
    {
        if (agent && door)
        {
            agent.SetDestination(door.transform.position);
            agent.isStopped = false;
        }
    }

    // Call this using an Animation Event on the player's animation
    public void OpenDoor()
    {
        if (doorAnimator)
        {
            doorAnimator.SetTrigger("Open");
        }
    }

    private void Update()
    {
        // Adjust the speed in the animator based on the agent's speed
        if (playerAnimator && agent)
        {
            playerAnimator.SetFloat("Speed", agent.velocity.magnitude);
        }
    }
}
