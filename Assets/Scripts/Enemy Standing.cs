using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStanding : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;
    public Vector3 walkPoint;
    public bool walkPointSet, alreadyAttacked, playerInSightRange, playerInAttackRange;
    public float walkPointRange, timeBetweenAttacks, sightRange, attackRange;
    private Animator _animator;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] public GameObject muzzleFlash;

    private void Chasing()
    {
        _animator.SetFloat("AControl", 0.5f);
        agent.SetDestination(player.position);
    }

    private void Attacking()
    {
        _animator.SetFloat("AControl", 0);
        agent.SetDestination(transform.position);
        transform.LookAt(new Vector3(player.position.x, player.position.y - 0.3f, player.position.z));

        if (!alreadyAttacked)
        {

            Rigidbody rb = Instantiate(_bulletPrefab, transform.GetChild(0).GetChild(2).GetChild(0).
                GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(5).position, Quaternion.identity).GetComponent<Rigidbody>();
            GameObject _muzzle = Instantiate(muzzleFlash, transform.GetChild(0).GetChild(2).GetChild(0).
                GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(5).position, Quaternion.identity);
            rb.AddForce(transform.forward * 32, ForceMode.Impulse);



            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);

            Destroy(rb.gameObject, 0.5f);
            Destroy(_muzzle, 0.1f);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }



    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        _animator = GetComponentInChildren<Animator>();

        _animator.SetFloat("AControl", 0f);
    }

    private void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (playerInSightRange && !playerInAttackRange)
            Chasing();
        if (playerInSightRange && playerInAttackRange)
            Attacking();

    }
}
