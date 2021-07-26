using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MyEnemy : MonoBehaviour
{
    [HideInInspector]
    public UnityEvent deathEvent = new UnityEvent();
    [HideInInspector]
    public UnityEvent fireEvent = new UnityEvent();

    protected bool isDead = false;
    protected bool isAsleep = false;

    public EnemyStats stats;

    [Header("Attack Settings")]
    public bool attacking = false;

  
    //protected Animator _animator;
    protected SpriteRenderer _renderer;
    protected Rigidbody2D _rb;
    protected GameObject _player;

    [Header("Audio Source")]
    //public AudioSource audio;

    [Header("Wander Config")]
    public float wanderTimeDir = 1f;
    public float wanderRestTime = 3f;

    //public State currentState;

    [HideInInspector]
    public bool needsWanderDirection = true;

    [HideInInspector]
    public Vector3 wanderTarget;

    protected bool _waitForHurt = false;

    public float pushForce = 1f;
    public float pushVelocity = 1f;
    protected Vector3 _pushBackPosition;
    protected bool _pushBack = false;
 

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        
        //_animator = gameObject.GetComponent<Animator>();
        _renderer = gameObject.GetComponent<SpriteRenderer>();
        _rb = gameObject.GetComponent<Rigidbody2D>();

        //currentState = new IdleState(this, _player, _animator);
    }

    private void Update()
    {
        if (!isAsleep)
            //currentState = currentState.Process();

        if (CanSeePlayer())
            transform.rotation = _player.transform.position.x < transform.position.x ? Quaternion.Euler(0, -180, 0) : Quaternion.Euler(0, 0, 0);

        if (_pushBack)
        {
            transform.position = Vector3.Lerp(transform.position, _pushBackPosition, pushVelocity * Time.deltaTime);
        }
    }


    ////////////////// Status Methods //////////////////////

    public bool CanSeePlayer()
    {
        // Enemy layer distinta a Default para evitar el raycast (Mismo con Attack y Slash)
        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            _player.transform.position - transform.position,
            stats.enemyVision,
            1 << LayerMask.NameToLayer("Default")
        );

        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Player")) //&& !_player.death)
            {
                return true;
            }
        }

        return false;
    }


    public bool IsInPersonalSpace()
    {
        return Vector3.Distance(_player.transform.position, transform.position) < stats.personalSpace;
    }



    ////////////////// Hurt Methods //////////////////////

    public virtual void PushBack(Vector3 attackDir)
    {
        _pushBack = true;
        _pushBackPosition = transform.position + (attackDir * pushForce);
    }

    public virtual void GetHit(bool damage)
    {
        if (!_waitForHurt)
        {
            _waitForHurt = true;

            if (damage)
            {
                stats.enemyHealth -= 1;
                Debug.Log("Enemy damaged: " + stats.enemyHealth);
            }

            if (stats.enemyHealth <= 0)
                Die();

            //else
                //_animator.SetTrigger("hurt");

            StartCoroutine(FinishInvulnerability(1f));
        }
    }



    protected virtual void Die()
    {
        isDead = true;
        //_animator.SetTrigger("isDead");

        transform.rotation = Quaternion.Euler(0, 0, 0);
        _renderer.flipX = false;
        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = 0;
    }


    public void FinishDeath()
    {
        deathEvent.Invoke();
        Destroy(gameObject);
    }

    ////////////////// Other Methods //////////////////////

    public void WaitToWander()
    {
        StartCoroutine(WaitWanderDirection(wanderTimeDir));
    }

    public void WaitToWake(float sleepTime)
    {
        StartCoroutine(WakeUp(sleepTime));
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "muro")
            wanderTarget *= -1;
    }

    protected IEnumerator WakeUp(float timeTo)
    {
        isAsleep = true;
        yield return new WaitForSeconds(timeTo);
        isAsleep = false;
    }

    protected IEnumerator WaitWanderDirection(float time)
    {
        needsWanderDirection = false;
        yield return new WaitForSeconds(time);
        needsWanderDirection = true;
    }

    protected IEnumerator FinishInvulnerability(float time)
    {
        _waitForHurt = false;
        yield return new WaitForSeconds(time);
        _waitForHurt = true;
        _pushBack = false;
    }
}

