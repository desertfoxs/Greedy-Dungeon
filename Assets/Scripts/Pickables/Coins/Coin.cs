using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    #region properties and fields

    public float moveSpeed = 1f;
    public float rotSpeed = 2f;
    public float stopMovingTime = .3f;
    public float destroyDistance = 1f;

    private Vector3 _suckCenter;

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

            if (Vector3.Distance(transform.position, _suckCenter) < destroyDistance)
            {
                GameManagerAngel.Instance.GrabCoin();
                Destroy(gameObject);
            }
        }
            
    }

    private void StopMoving()
    {
        _suckCenter = Vector3.zero;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Vacuum")
        {
            _suckCenter = other.transform.position;
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
