using Prime31;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    #region properties and fields

    public Animator playerAnimator;
    public CharacterController2D controller;
    public GameObject vacuum;
    private VacuumLogic _vacuumLogic;

    public float gravity = -25f;
    public float movementSpeed = 5f;
    public float groundDamping = 20f; // how fast do we change direction? higher means faster
    public float inAirDamping = 5f;
    public float jumpHeight = 3f;

    public float fallTime = 0.5f;

    public bool disableMomvement = false;
    private bool downAttack = false;

    [HideInInspector]
    private float normalizedHorizontalSpeed = 0;

    private Queue<string> _inputBuffer = new Queue<string>();

    private RaycastHit2D _lastControllerColliderHit;

    private Vector3 _movement;
    private Vector3 _smothSpeed;

    private bool right = true;


    #endregion

    private void Start()
    {
        _vacuumLogic = vacuum.GetComponent<VacuumLogic>();
    }


    // Update is called once per frame
    void Update()
    {
        if (!disableMomvement)
        {
            if (controller.isGrounded)
                _movement.y = 0;

            HandleHorMovement();

            HandleJump();
        }

        else
        {
            _movement.x = Mathf.Clamp(_movement.x - (_movement.x > 0 ? 0.2f : -0.2f) * Time.deltaTime, 0, 0);
        }

        _movement.y += gravity * Time.deltaTime;

        controller.move(_movement * Time.deltaTime);
        _movement = controller.velocity;
    }

    private void HandleHorMovement()
    {
        float horDir = Input.GetAxisRaw("Horizontal");

        playerAnimator.SetFloat("Speed", Mathf.Abs(horDir));

        if (horDir > 0)
        {
            Vector3 scale = this.transform.localScale;
            this.transform.localScale = new Vector3(Mathf.Abs(scale.x), scale.y, scale.z);
            right = true;
        }

        else if (horDir < 0)
        {
            Vector3 scale = this.transform.localScale;
            this.transform.localScale = new Vector3(Mathf.Abs(scale.x) * -1, scale.y, scale.z);
            right = false;
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

    private void HandleJump()
    {

        if (Input.GetButtonDown("Jump"))
        {
            _inputBuffer.Enqueue("Jump");
            Invoke("RemoveAction", 0.2f);
        }

        // we can only jump whilst grounded
        if (controller.isGrounded)
        {
            downAttack = false;
            vacuum.transform.rotation = Quaternion.Euler(0, 0, 0);

            if (_inputBuffer.Count > 0 && _inputBuffer.Peek() == "Jump")
            {
                _movement.y = Mathf.Sqrt(2f * jumpHeight * -gravity);
                _inputBuffer.Dequeue();

                playerAnimator.SetBool("Falling", false);
                StartCoroutine(SetFalling());
            }
        }
        else
        {
            float down = Input.GetAxisRaw("Vertical");
            if (down < 0 && !_vacuumLogic.sucking)
            {
                vacuum.transform.rotation = Quaternion.Euler(0, 0, right ? -90f : 90f);
                downAttack = true;
            }

            else if (down == 0 || _vacuumLogic.sucking)
            {
                vacuum.transform.rotation = Quaternion.Euler(0, 0, 0);
                downAttack = false;
            }
        }

        playerAnimator.SetBool("Grounded", controller.isGrounded);

    }

    void RemoveAction()
    {
        if (_inputBuffer.Count > 0)
            _inputBuffer.Dequeue();
    }

    public void BounceUpwards()
    {
        _movement.y = Mathf.Sqrt(2f * jumpHeight * -gravity);
        StartCoroutine(SetFalling());
    }

    public bool IsAttackingDown()
    {
        return downAttack;
    }

    public void EnableMovement()
    {
        this.disableMomvement = false;
    }
    public void EnableAttack()
    {
        _vacuumLogic.ResetAttack();
    }

    private IEnumerator SetFalling()
    {
        yield return new WaitForSeconds(fallTime);
        playerAnimator.SetBool("Falling", true);
        StopAllCoroutines();
    }
}
