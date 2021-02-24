using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CrowdAgent : MonoBehaviour
{

    public GameObject[] agentGoals;
    NavMeshAgent agent;
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        agentGoals = GameObject.FindGameObjectsWithTag("CrowdGoal");
        agent = this.GetComponent<NavMeshAgent>();
        agent.SetDestination(agentGoals[Random.Range(0,agentGoals.Length)].transform.position);

        anim = GetComponent<Animator>();
        anim.SetFloat("Speed", 1f, 0f, Time.deltaTime);
        anim.SetFloat("wOffset", Random.Range(0, 1));
       float agentSpeed = Random.Range(0.9f, 1);
        anim.SetFloat("speedMultiplyer", agentSpeed);
        agent.speed *= agentSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if(agent.remainingDistance < 1.0f)
        {
            agent.SetDestination(agentGoals[Random.Range(0, agentGoals.Length)].transform.position);
        }
    }
}
