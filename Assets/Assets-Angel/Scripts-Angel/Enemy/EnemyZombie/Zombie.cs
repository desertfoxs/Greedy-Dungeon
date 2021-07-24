using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MyEnemy
{
   
    private NavMeshAgent _agente;
    private Transform _initialPosition;

    private void Awake()
    {
        _agente = GetComponent<NavMeshAgent>();            
    }


    void Start()
    {
        _agente.updateRotation = false;
        _agente.updateUpAxis = false;

        _initialPosition = gameObject.GetComponent<Transform>();

        _player = GameObject.FindGameObjectWithTag("Player");
        //_animator = gameObject.GetComponent<Animator>();
        //_renderer = gameObject.GetComponent<SpriteRenderer>();
        _rb = gameObject.GetComponent<Rigidbody2D>();
    }
    

    private void Update()
    {
        
        if (IsInPersonalSpace())
        {
            
            CanSeePlayer();
            _agente.SetDestination(_player.transform.position);
            
        }
        else
        {
            _agente.SetDestination(_initialPosition.position);
            CanSeePlayer();
        }

       
        if (CanSeePlayer())
        {
            
            transform.rotation = _player.transform.position.x > transform.position.x ? Quaternion.Euler(0, -180, 0) : Quaternion.Euler(0, 0, 0);
        }
            

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag == "Player")
        {  
            StartCoroutine(Attack(stats.attackDelay));
        }
    }

    protected IEnumerator Attack(float Time)
    {
        
        //ataque al jugador
      
        _agente.SetDestination(transform.position);
        yield return new WaitForSeconds(Time);        
        _agente.SetDestination(_player.transform.position);
        yield return null;
    }

}
