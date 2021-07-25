using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MyEnemy
{
   

    private NavMeshAgent _agente;
    private Vector3 _initialPosition;
    private bool _attack = false;
 

    private void Awake()
    {
        _agente = GetComponent<NavMeshAgent>();
        
    }


    void Start()
    {
        _initialPosition = transform.position;
        
        _agente.updateRotation = false;
        _agente.updateUpAxis = false;

        _player = GameObject.FindGameObjectWithTag("Player");
        
        
        //_rb = gameObject.GetComponent<Rigidbody2D>();
        //_animator = gameObject.GetComponent<Animator>();
        //_renderer = gameObject.GetComponent<SpriteRenderer>();
    }


    private void Update()
    {


        if (IsInPersonalSpace() && !_attack)
        {
            _agente.destination = _player.transform.position;
        }
        else
        {
            if (!_attack)
            {

                _agente.destination = _initialPosition;

            }
        }

        CanSeePlayer();
        if (CanSeePlayer())
        {     
            transform.rotation = _player.transform.position.x > transform.position.x ? Quaternion.Euler(0, -180, 0) : Quaternion.Euler(0, 0, 0);
        }
            

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag == "Player" && !_attack)
        {  
            StartCoroutine(Attack(stats.attackDelay));
        }
    }

    protected IEnumerator Attack(float Time)
    {
        _attack = true;
        //ataque al jugador

        _agente.destination = transform.position;
        yield return new WaitForSeconds(Time);

        _attack = false;
      

        yield return null;
    }

}
