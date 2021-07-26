using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    #region properties and fields

    public float moveSpeed = 1f;
    public float rotSpeed = 2f;
    public float stopMovingTime = .3f;
    public float stuckDistance = 1f;

    public float fireTime = 2f;
    public float fireSpeed = 4f;

    public float durability = 2f;

    private Vector3 _suckCenter = Vector3.zero;
    private Vector3 _fireDir = Vector3.zero;
    private VacuumLogic vacuum;

    private Rigidbody2D rg;

    private void Start()
    {
        rg = gameObject.GetComponent<Rigidbody2D>();
    }

    #endregion
    // Update is called once per frame
    void Update()
    {
        if (_suckCenter != Vector3.zero)
        {
            transform.position = Vector3.Lerp(transform.position, _suckCenter, moveSpeed * Time.deltaTime);

            Vector3 vectorToTarget = _suckCenter - transform.position;
            float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * rotSpeed);


            if (Vector3.Distance(transform.position, _suckCenter) < stuckDistance)
            {
                vacuum.Stuck(this);
                transform.position = vacuum.stuckPos.transform.position;
                transform.rotation = Quaternion.Euler(0, 0, -90);
                gameObject.transform.parent = vacuum.gameObject.transform;
                
                _suckCenter = Vector3.zero;
            }
        }

        if (_fireDir != Vector3.zero)
        {
            transform.position = Vector3.Lerp(transform.position, transform.position + _fireDir, fireSpeed * Time.deltaTime);
        }

    }

    public void FireGem(Vector3 moveDir)
    {
        _fireDir = moveDir; 
        transform.SetParent(null);
        rg.isKinematic = false;

        StartCoroutine(FinishFiring());
    }

    private void StopMoving()
    {
        _suckCenter = Vector3.zero;
    }

    IEnumerator FinishFiring()
    {
        yield return new WaitForSeconds(fireTime);
        _fireDir = Vector3.zero; 
        rg.isKinematic = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            MyEnemy enemy = collision.gameObject.GetComponent<MyEnemy>();
            Vector3 attackDir = new Vector3(_fireDir.x * 1, .5f, 0);
            enemy.PushBack(attackDir);

            enemy.GetHit(true);

            durability--;

            if (durability == 0)
            {
                gameObject.SetActive(false);
                Destroy(gameObject, 1f);
            }
        }
        else
        {
            _fireDir = Vector3.zero;
            rg.isKinematic = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Vacuum" && _suckCenter == Vector3.zero)
        {
            _suckCenter = other.transform.position;
            vacuum = other.gameObject.GetComponent<VacuumLogic>();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Vacuum")
        {
            moveSpeed *= .5f;
            Invoke("StopMoving", stopMovingTime);
            moveSpeed /= .5f;
        }

    }
}
