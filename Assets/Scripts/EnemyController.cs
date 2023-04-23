using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;

    private Transform player;

    [SerializeField]
    private LayerMask playerLayer;

    [SerializeField]
    private Transform lookPoint;
    
    private Camera shootingRaycastArea;

    [Header("Guarding var")]
    [SerializeField]
    private GameObject[] walkPoints;
    
    private int currentPointIndex;
    
    [SerializeField]
    private int enemySpeed;

    private const float walkingPointRadius = 2;

    [Header("Enemy mood/situation")]
    [SerializeField]
    private float defaultVisionRadius;
    [SerializeField]
    private float defaultShootingRadius;
    
    [SerializeField]
    private float pursueVisionRadius;
    [SerializeField]
    private float pursueShootingRadius;

    private float visionRadius;
    private float shootingRadius;
    
    private bool isInVisionRadius;
    private bool isInShootingRadius;

    private void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        navMeshAgent = GetComponent<NavMeshAgent>();

        visionRadius = defaultVisionRadius;
        shootingRadius = defaultShootingRadius;
    }

    private void Update()
    {
        isInVisionRadius = Physics.CheckSphere(transform.position, visionRadius, playerLayer);
        isInShootingRadius = Physics.CheckSphere(transform.position, shootingRadius, playerLayer);

        if (!isInVisionRadius && !isInShootingRadius)
        {
            Guard();
            
            // visionRadius = defaultVisionRadius;
            // shootingRadius = defaultShootingRadius;
        }
        // else
        // {
        //     visionRadius = pursueVisionRadius;
        //     shootingRadius = pursueShootingRadius;
        // }
        // else if (isInVisionRadius && !isInShootingRadius)
        // {
        //     PursuePlayer();
        // }
        
        if (isInShootingRadius)
        {
            ShootPlayer();
        }
        else if (isInVisionRadius)
        {
            PursuePlayer();
        }
    }

    private void Guard()
    {
        if (Vector3.Distance(walkPoints[currentPointIndex].transform.position, transform.position) < walkingPointRadius)
        {
            currentPointIndex = Random.Range(0, walkPoints.Length);
        }

        transform.position = Vector3.MoveTowards(transform.position, walkPoints[currentPointIndex].transform.position,
            enemySpeed * Time.deltaTime);
        transform.LookAt(walkPoints[currentPointIndex].transform.position);
    }

    private void ShootPlayer()
    {
        if (navMeshAgent.SetDestination(transform.position))
        {
            
        }
    }

    private void PursuePlayer()
    {
        if (navMeshAgent.SetDestination(player.position))
        {
            // visionRadius = pursueVisionRadius;
            // shootingRadius = pursueShootingRadius;
        }
    }
}
