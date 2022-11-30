using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentScript : MonoBehaviour
{

    public Animator animator;
    public Transform goal;
    NavMeshAgent agent; //this is a global variable

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>(); //asssigning the global variable with the navmesh agent component
    }

    // Update is called once per frame
    void Update()
    {
        if(gameObject.tag == "hostage")
        {
            animator.SetFloat("Speed_f", 0);
            animator.SetBool("Crouch_b", true);
            
        } else
        {
            agent.destination = goal.position;
            animator.SetFloat("Speed_f", agent.velocity.magnitude);
        }

    }
}
