using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{

    public int speed;
    private Rigidbody2D _rb;






    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {


        if (Input.GetKey("left"))
        {
            _rb.AddForce(new Vector2(-speed * Time.deltaTime, 0));
          
        }

        if (Input.GetKey("right"))
        {
            _rb.AddForce(new Vector2(speed * Time.deltaTime, 0));
            
        }




       
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if (collision.transform.tag == "muro")
        //{
            
       // }

    }

   





 
}

 


