using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public PlayerMovement movement;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Ghost")
        {
            MyEnemy enemy = collision.GetComponent<MyEnemy>();
            if (movement.IsAttackingDown())
                movement.BounceUpwards();


            else
            {
                float right = enemy.transform.position.x > gameObject.transform.position.x ? 1 : -1;
                Vector3 attackDir = new Vector3(right, .5f, 0);
                enemy.PushBack(attackDir);
            }
            

            enemy.GetHit(false);
        }
    }
}
