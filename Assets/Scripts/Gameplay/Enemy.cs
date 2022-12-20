using Player;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public enum ItState { Wander, Chase, Flee }

public class Enemy : MonoBehaviour
{
    #region variables

    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask wallLayer;

    [SerializeField] public int life = 3;

    [SerializeField] private float maxChaseTime;
    [SerializeField] private float invincibilityTime;

    [SerializeField] private float maxDistanceAroundPlayer = 10f;
    
    public ItState state = ItState.Wander;
    
    private Transform player;
    private NavMeshAgent agent;

    private float timerChase;
    private float timerInvincibility;

    #endregion
    
    #region built-in functions

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }
    
    void Update()
    {
        if (life == 0) Die();
        
        // State machine
        switch (state)
        {
            case ItState.Wander:
                RandomWander();
                break;
            case ItState.Chase:
                Chase();
                break;
            case ItState.Flee:
                Flee();
                break;
            default:
                RandomWander();
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Rip
        if (other.gameObject.CompareTag("Player") && state != ItState.Flee)
        {
            other.GetComponent<PlayerController>().Die();
        }
    }

    #endregion

    #region Custom functions

    private void RandomWander()
    {
        // Player spotted
        if (LookForPlayer())
        {
            state = ItState.Chase;
            return;
        }
        
        // Goes somewhere around the player
        if (agent.remainingDistance < 1f)
        {
            float offset = Random.Range(-(maxDistanceAroundPlayer / 2f), maxDistanceAroundPlayer / 2f);

            Vector3 point = player.position + new Vector3(offset, 0, offset);
        
            agent.destination = point;
        }
    }

    private void Chase()
    {
        // Chase, even when player wasn't seen for some time
        if (LookForPlayer())
        {
            timerChase = 0;
            agent.destination = player.position;
        }
        else
        {
            timerChase += Time.deltaTime;

            // Stop chasing after some time
            if (timerChase > maxChaseTime)
            {
                state = ItState.Wander;
            }
        }
    }

    private void Flee()
    {
        // When hit
        timerInvincibility += Time.deltaTime;

        if (timerInvincibility > invincibilityTime)
        {
            state = ItState.Wander;
        }
        else
        {
            agent.destination = GetFarFromPlayer();
        }
    }

    public void Hit()
    {
        life--;
        timerChase = 0;
        timerInvincibility = 0;
        state = ItState.Flee;
    }

    private void Die()
    {
        Destroy(gameObject);
        FindObjectOfType<EndScreenManager>().ShowMenu();
    }
    
    private bool LookForPlayer()
    {
        // Might be bad but works
        
        Vector3 raycastOrigin = transform.position + Vector3.up;
        
        bool seePlayerNorth = Physics.Raycast(raycastOrigin, Vector3.forward,40f, playerLayer) && 
                              !Physics.Raycast(raycastOrigin, Vector3.forward, 40f, wallLayer);
        
        bool seePlayerEast = Physics.Raycast(raycastOrigin, Vector3.right,40f, playerLayer) && 
                              !Physics.Raycast(raycastOrigin, Vector3.right, 40f, wallLayer);
        
        bool seePlayerWest = Physics.Raycast(raycastOrigin, Vector3.left,40f, playerLayer) && 
                              !Physics.Raycast(raycastOrigin, Vector3.left, 40f, wallLayer);
        
        bool seePlayerSouth = Physics.Raycast(raycastOrigin, Vector3.back,40f, playerLayer) && 
                              !Physics.Raycast(raycastOrigin, Vector3.back, 40f, wallLayer);

        return seePlayerNorth || seePlayerEast || seePlayerWest || seePlayerSouth;
    }

    private Vector3 GetFarFromPlayer()
    {
        Vector3 transformToPlayer = transform.position - player.position;
        
        return transformToPlayer * 3;
    }

    #endregion
}
