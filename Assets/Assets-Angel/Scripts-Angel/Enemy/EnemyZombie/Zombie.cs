using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MyEnemy
{
   

    private NavMeshAgent _agente;
    private Vector3 _initialPosition;
    private bool _attack = false;

    //private Animator _anim;

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
        gameObject.GetComponent<Animator>().SetBool("Move", false);

        //_anim = gameObject.GetComponent<Animator>();

        //_rb = gameObject.GetComponent<Rigidbody2D>();
        //_renderer = gameObject.GetComponent<SpriteRenderer>();
    }


    private void Update()
    {


        if (IsInPersonalSpace() && !_attack)
        {
            _agente.destination = _player.transform.position;

            gameObject.GetComponent<Animator>().SetBool("Move", true);
            transform.rotation = _player.transform.position.x > transform.position.x ? Quaternion.Euler(0, -180, 0) : Quaternion.Euler(0, 0, 0);
        }
        else
        {
            if (!_attack)
            {

                _agente.destination = _initialPosition;
                gameObject.GetComponent<Animator>().SetBool("Move", true);
                transform.rotation = _initialPosition.x > transform.position.x ? Quaternion.Euler(0, -180, 0) : Quaternion.Euler(0, 0, 0);
            }
        }
        if(_initialPosition.x == transform.position.x)
        {
            gameObject.GetComponent<Animator>().SetBool("Move", false);
        }

        CanSeePlayer();
        if (CanSeePlayer())
        {     
            transform.rotation = _player.transform.position.x > transform.position.x ? Quaternion.Euler(0, -180, 0) : Quaternion.Euler(0, 0, 0);
        }

        if (_pushBack)
        {
            transform.position = Vector3.Lerp(transform.position, _pushBackPosition, pushVelocity * Time.deltaTime);
        }


    }

    public override void PushBack(Vector3 attackDir)
    {
        _agente.isStopped = true;
        _pushBack = true;
        _pushBackPosition = transform.position + (attackDir * pushForce);

        Invoke("StopPushBack", 1f);
    }

    private void StopPushBack()
    {
        _pushBack = false;
        _agente.isStopped = false;
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
        gameObject.GetComponent<Animator>().SetBool("Move", false);

        yield return new WaitForSeconds(Time);

        gameObject.GetComponent<Animator>().SetBool("Move", true);
        _attack = false;
      

        yield return null;
    }

}
