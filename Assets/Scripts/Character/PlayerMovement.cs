using Prime31;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    #region properties and fields

    public CharacterController2D controller;

    public float gravity = -25f;
    public float movementSpeed = 5f;
    public float groundDamping = 20f; // how fast do we change direction? higher means faster
    public float inAirDamping = 5f;
    public float jumpHeight = 3f;

    [HideInInspector]
    private float normalizedHorizontalSpeed = 0;

    private Queue<string> _inputBuffer = new Queue<string>();

    private Animator _animator;
    private RaycastHit2D _lastControllerColliderHit;

    private Vector3 _movement;
    private Vector3 _smothSpeed;

    #endregion


    // Update is called once per frame
    void Update()
    {
        if (controller.isGrounded)
            _movement.y = 0;

        float horDir = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump"))
        {
            _inputBuffer.Enqueue("Jump");
            Invoke("RemoveAction", 0.2f);
        }

        // we can only jump whilst grounded
        if (controller.isGrounded)
        {
            if (_inputBuffer.Count > 0 && _inputBuffer.Peek() == "Jump")
            {
                _movement.y = Mathf.Sqrt(2f * jumpHeight * -gravity);

                _inputBuffer.Dequeue();
            }
        }
        else
        {
            //Debug.Log("NOT GROUNDED!");
        }

        float smoothedMovementFactor = controller.isGrounded ? groundDamping : inAirDamping; // how fast do we change direction?
        _movement.x = Mathf.Lerp(_movement.x, horDir * movementSpeed, Time.deltaTime * smoothedMovementFactor);

        _movement.y += gravity * Time.deltaTime;

        // if holding down bump up our movement amount and turn off one way platform detection for a frame.
        // this lets us jump down through one way platforms
        if (controller.isGrounded && Input.GetKey(KeyCode.DownArrow))
        {
            _movement.y *= 3f;
            controller.ignoreOneWayPlatformsThisFrame = true;
        }


        controller.move(_movement * Time.deltaTime);
        _movement = controller.velocity;

    }

    void RemoveAction()
    {
        _inputBuffer.Dequeue();
    }
}
