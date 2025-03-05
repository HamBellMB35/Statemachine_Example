using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 4;                                                                                     // Enemy Movement speed
    public float aggroRadius = 5;                                                                                   // Distance at which the enemy will attack
    public float returnToOriginRadius = 15;                                                                         // Distance at which the enemy disengages and
                                                                                                                    // returns to its start point

    public Material aggroMat;                                                                                       // Diiferent materials the enemy can have
    public Material idleMat;
    public Material returningMat;

    private float _distanceToPlayer;
    private Renderer _renderer;
    private Vector3 _originalPos;                                                                                   // Position where the enemy starts
    private float _distanceToOrigin;

    private enum EnemyState                                                                                         // Enum create to hold the different states the enemy
    {                                                                                                               // can be in
        Idle, 
        Aggro,
        ReturnToOrigin
    }

    [SerializeField]
    private EnemyState _currentState;                                                                               // Current state of the enemy


    // Initial setup

    void Awake()
    {
        _renderer = GetComponent<Renderer>();                                                                       // Used to change the enemy's material depending on the state
        _originalPos = transform.position;                                                                          // Stores the enemy starting point
        _currentState = EnemyState.Idle;                                                                            // Sets the enemy's initial state                                                                                                                                     
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.25f);
        Gizmos.DrawSphere(transform.position, aggroRadius);
        Gizmos.color = new Color(0.25f, 0.25f, 0.25f, 0.1f);
        Gizmos.DrawSphere(transform.position, returnToOriginRadius);

    }


    void Update()
    {
        _distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if(_currentState == EnemyState.Idle)
        {
            UpdateIdle();
        }

        else if(_currentState == EnemyState.Aggro)
        {
            UpdateAggro();
        }

        else if (_currentState == EnemyState.ReturnToOrigin)
        {
            UpdateReturnToOrigin();
        }




    }

    // State Updates
    private void UpdateAggro()
    {
        _renderer.material = aggroMat;

        MoveToPlayer();

        if (_distanceToPlayer >= returnToOriginRadius) _currentState = EnemyState.ReturnToOrigin;

    }

    private void UpdateReturnToOrigin()
    {
        _renderer.material = returningMat;
        MoveToOriginalPos();

        float _distanceToOrigin = Vector3.Distance(_originalPos, transform.position);

        if (_distanceToOrigin <= 0.05f)
        {
            _currentState = EnemyState.Idle;
        }
    }


    private void UpdateIdle()
    {
        _renderer.material = idleMat;

        if (_distanceToPlayer <= aggroRadius)
            _currentState = EnemyState.Aggro;
    }

    // Enemy Movement 
    void MoveToOriginalPos()
    {
        transform.position += (_originalPos - transform.position).normalized * Time.deltaTime * moveSpeed;
    }

    void MoveToPlayer()
    {
        transform.position += (player.position - transform.position).normalized * Time.deltaTime * moveSpeed;
    }

  
   
}
