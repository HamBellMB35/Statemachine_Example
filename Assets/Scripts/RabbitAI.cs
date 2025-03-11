using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitAI : MonoBehaviour
{
    Rigidbody2D rigidBody;
    Animator animator;
    NinjaController ninja; // this is my enemy
    Vector2 toEnemy;
    float enemyDistance;

    public float minDistance = 3.0f;
    public float maxDistance = 8.0f;
    public float runningSpeed = 2.0f;
    private enum RabbitState
    {
        Idle, 
        RunTowards,
        RunAaway
    }

    private RabbitState _currentState;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        ninja = FindObjectOfType<NinjaController>();
        _currentState = RabbitState.Idle;
    }
    
    void Start ()
    {
	}

    void SetVelocityX(float x)
    {
        Vector2 velocity = rigidBody.velocity;
        velocity.x = x;
        rigidBody.velocity = velocity;
    }

	void Update ()
    {
        toEnemy = ninja.transform.position - transform.position;
        enemyDistance = Mathf.Abs(toEnemy.x);
        UIManager.Instance.SetDistance(enemyDistance);

        switch (_currentState)
        {
            case RabbitState.Idle:
                UpdateRabbitIdleState();
                break;

            case RabbitState.RunTowards:
                UpdateRabbitRunTowards();
                break;

            case RabbitState.RunAaway:
                UpdateRabbitRunAaway();
                break;

        }

        //TODO: Implement Movement via SetVelocityX


	}

    private void UpdateRabbitIdleState()
    {
        SetVelocityX(0);

        if(enemyDistance >= maxDistance)
        {
            _currentState = RabbitState.RunTowards;
        }

        if(enemyDistance < minDistance)
        {
            _currentState = RabbitState.RunAaway;
        }

        animator.SetInteger("anim", 0);
        
    }

    private void UpdateRabbitRunTowards()
    {
        SetVelocityX(-runningSpeed);

        if(enemyDistance < maxDistance)
        {
            _currentState = RabbitState.Idle;
        }

        animator.SetInteger("anim", 1);

    }

    private void UpdateRabbitRunAaway()
    {
        SetVelocityX(runningSpeed);

        if (enemyDistance > minDistance)
        {
            _currentState = RabbitState.Idle;
        }

        animator.SetInteger("anim", 2);

    }
}
