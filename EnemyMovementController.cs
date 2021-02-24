using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyMovementController : MonoBehaviour
{
    public float lookRadius = 10f;
    public Transform Player;
    NavMeshAgent agent;
    Animator anim;
    bool isDead;

    public Transform[] patrolPoints;
    private int destPoint = 0;
    private NPCState state = NPCState.patrol; // By default agents should attempt to Patrol

    public GameObject bulletProjectile;
    public GameObject agentGunBarrel;
    private GameObject muzzleflash;
    private float attackDelay = 0f;

    public Text score;

    // Start is called before the first frame update
    void Start()
    {
        if (Player == null)
        {
            Debug.LogError("Player not defined for Enemy");
            return;
        }
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;
        agent.speed = 3f;

        anim = GetComponent<Animator>();

        if (agentGunBarrel != null)
        {
           muzzleflash = agentGunBarrel.transform.GetChild(0).gameObject;
        }
        
        // Upon game starting we want the AI to begin patrolling rather than standing still waiting for player.
        goToNextPoint();
    }

    // Update is called once per frame
    void Update()
    {
        isDead = GetComponent<DestroyableObject>().isDestroyed;
        if (isDead)
        {
            return;
        }

        float distanceFromPlayer = UnityEngine.Vector3.Distance(Player.position, transform.position);

        bool playerIsDead = Player.gameObject.GetComponent<PlayerHealth>().isDead;

        switch (state) {
            case NPCState.patrol:
                if (!agent.pathPending && agent.remainingDistance < 4f)
                {
                    goToNextPoint();
                }
                else if (distanceFromPlayer <= lookRadius && !playerIsDead)
                {
                    state = NPCState.attack;
                }
                break;

            case NPCState.attack:
                agent.SetDestination(Player.position);
                if (distanceFromPlayer <= agent.stoppingDistance && !playerIsDead)
                {
                    FaceTarget(Player);
                    AttackPlayer();
                }
                else
                {
                    state = NPCState.patrol;
                }
                break;

            default:
                state = NPCState.patrol;
                break;
        }

        // PRE FSM Implementation
        /*if (distanceFromPlayer <= lookRadius && !playerIsDead) // i.e. if we're within the sphere
        {
            agent.SetDestination(Player.position); // lock onto Player

            if (distanceFromPlayer <= agent.stoppingDistance)
            {
                // Attack the target TO BE IMPLEMENTED
                FaceTarget(Player);
                AttackPlayer();
            }
        }
        else
        {
            // Continue Patrolling
            if (!agent.pathPending && agent.remainingDistance < 4f)
            {
                goToNextPoint();
            }
        }*/
    }

    void AttackPlayer()
    {
        if (attackDelay <= 0f && bulletProjectile != null && agentGunBarrel != null)
        {
            GameObject bulletObject = Instantiate(bulletProjectile);
            anim.SetBool("Attack", true);
            anim.SetFloat("Speed", 0f, 0.1f, Time.deltaTime);
            muzzleflash.SetActive(true);
            bulletObject.transform.position = agentGunBarrel.transform.position;
            bulletObject.transform.forward = transform.forward;

            attackDelay = 1.2f;
            
        }
        else
        {
            muzzleflash.SetActive(false);
            anim.SetBool("Attack", false);
            attackDelay -= Time.deltaTime;
        }
    }

    void goToNextPoint()
    {
        if  (patrolPoints.Length == 0)
        {
            Debug.LogError("Patrol Points not set for enemy.");
            return;
        }

        anim.SetFloat("Speed", 1f, 0f, Time.deltaTime);
        anim.SetFloat("wOffset", Random.Range(0, 1));
        float agentSpeed = Random.Range(0.9f, 1);
        anim.SetFloat("speedMultiplyer", agentSpeed);
        agent.speed *= agentSpeed;

        agent.SetDestination(patrolPoints[destPoint].position);
        FaceTarget(patrolPoints[destPoint]);
        destPoint = (destPoint + 1) % patrolPoints.Length; // Allows us to loop Patrol points.
    }

    void FaceTarget(Transform target)
    {
        // Get the direction to the Player from the agent's current position
        UnityEngine.Vector3 direction = (target.position - transform.position).normalized;

        //Calculate a rotation for agent to look at the Player
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));

        // Set agents current roation to that of the lookRotation that was just calculated
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 3f);
    }

    void OnDrawGizmosSelected()
    {
        // Used for seeing the targeting sphere of the Enemy 
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }

    public enum NPCState
    {
        // 2 Simple states for FSM Agents.
        patrol,
        attack
    }
}
