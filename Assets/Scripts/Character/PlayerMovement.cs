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

    public bool disableMomvement = false;
    private bool downAttack = false;

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
        if (!disableMomvement)
        {
            if (controller.isGrounded)
                _movement.y = 0;

            float horDir = Input.GetAxisRaw("Horizontal");

            if (horDir > 0)
            {
                Vector3 scale = this.transform.localScale;
                this.transform.localScale = new Vector3(Mathf.Abs(scale.x), scale.y, scale.z);
            }

            else if (horDir < 0)
            {
                Vector3 scale = this.transform.localScale;
                this.transform.localScale = new Vector3(Mathf.Abs(scale.x) * -1, scale.y, scale.z);
            }

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

            float smoothedMovementFactor = controller.isGrounded ? groundDamping : inAirDamping; // how fast do we change direction?
            _movement.x = Mathf.Lerp(_movement.x, horDir * movementSpeed, Time.deltaTime * smoothedMovementFactor);

            // if holding down bump up our movement amount and turn off one way platform detection for a frame.
            // this lets us jump down through one way platforms
            if (controller.isGrounded && Input.GetKey(KeyCode.DownArrow))
            {
                _movement.y *= 3f;
                controller.ignoreOneWayPlatformsThisFrame = true;
            }
        }

        else
        {
            _movement.x = Mathf.Clamp(_movement.x - (_movement.x > 0 ? 0.2f : -0.2f) * Time.deltaTime, 0, _movement.x > 0 ? 1 : -1);
        }

        _movement.y += gravity * Time.deltaTime;

        controller.move(_movement * Time.deltaTime);
        _movement = controller.velocity;

    }

    void RemoveAction()
    {
        if (_inputBuffer.Count > 0)
            _inputBuffer.Dequeue();
    }

    public void BounceUpwards()
    {
        _movement.y = Mathf.Sqrt(2f * jumpHeight * -gravity);
    }

    public bool IsAttackingDown()
    {
        return downAttack;
    }

    public void EnableMovement()
    {
        this.disableMomvement = false;
    }
}
