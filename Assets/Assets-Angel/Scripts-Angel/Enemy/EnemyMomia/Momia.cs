using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Momia : MyEnemy
{
    private NavMeshAgent _agente;
    private bool _attack = false;
    
    //variables para la patrulla
    public Transform targetA;
    public Transform targetB;
    private bool _postA = true;
    private bool _postB = false;
   

    private void Awake()
    {
        _agente = GetComponent<NavMeshAgent>();

    }
    void Start()
    {   
        _agente.updateRotation = false;
        _agente.updateUpAxis = false;
        
        _player = GameObject.FindGameObjectWithTag("Player");


        //_rb = gameObject.GetComponent<Rigidbody2D>();
        //_animator = gameObject.GetComponent<Animator>();
        //_renderer = gameObject.GetComponent<SpriteRenderer>();
    }


    void Update()
    {
        //pregunta si esta en el espacio personal y si no esta atacando
        if (IsInPersonalSpace() && !_attack)
        {
            //pregunta si el player esta en su espalda si no lo esta lo sigue
            if (DeEspalda())
            {
                //pregunta si el player entro en el rango de PersonalSpaceBack
                if(Vector3.Distance(_player.transform.position, transform.position) < stats.personalSpaceBack)
                {
                    _agente.destination = _player.transform.position;
                    transform.rotation = _player.transform.position.x > transform.position.x ? Quaternion.Euler(0, -180, 0) : Quaternion.Euler(0, 0, 0);
                }
                
            }
            else
            {
                _agente.destination = _player.transform.position;
                transform.rotation = _player.transform.position.x > transform.position.x ? Quaternion.Euler(0, -180, 0) : Quaternion.Euler(0, 0, 0);
            }
        }
        else
        {
            if (!_attack)
            {   
                //logica de la patrulla del punto A hacia el B
                //los target tienen que estar: TargetA - izq : TargetB - der
                if (_postB)
                {
                  
                   _agente.destination = targetA.position;
                    //mira hacia la izquierda
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                }

                if(_postA)
                {
                   
                    _agente.destination = targetB.position;
                    //mira hacia la derecha
                    transform.rotation = Quaternion.Euler(0, -180, 0);
                }

                if(transform.position.x == targetA.position.x)
                {
                    _postA = true;
                    _postB = false;
                }

                if (transform.position.x == targetB.position.x)
                {
                    _postB = true;
                    _postA = false;
                }
            }
        }

    }

    //Logica de la deteccion de si el player esta a su espalda
    public bool DeEspalda()
    {
        if (transform.rotation == Quaternion.Euler(0, 0, 0) && transform.position.x < _player.transform.position.x )
        {
            return true;
        }
        
        if (transform.rotation == Quaternion.Euler(0, -180, 0) && transform.position.x > _player.transform.position.x)
        {
            return true;
        }

        return false;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player" && !_attack)
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
