using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VacuumLogic : MonoBehaviour
{
    #region properties and fields

    public float vacumSpeedReduce = 0.8f;
    public GameObject vacuumPoint;
    public GameObject stuckPos;
    public PlayerMovement movement;

    public ParticleSystem vacuumParticles;
    public ParticleSystem forceParticles;

    public Animator playerAnimator;

    public bool sucking = false;

    private PolygonCollider2D _vacuumCollider;

    private bool stuck = false;
    private Gem _gem;

    private AudioSource audioSource;
    public float maxSoundPitch = 2f;
    public float pitchDuration = 2f;
    private float timeElapsed = 0;

    public bool punching = false;

    [Header("Sounds")]
    public AudioClip vacuumClip;
    public AudioClip hitClip;
    public AudioClip diamondShotClip;
    #endregion

    private void Start()
    {
        _vacuumCollider = vacuumPoint.GetComponent<PolygonCollider2D>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!stuck)
        {
            if (Input.GetButtonDown("Suck"))
            {
                StartSucking();
            }
            else if (Input.GetButtonUp("Suck"))
            {
                StopSucking();
            }

            if (Input.GetButton("Fire") && !punching)
            {
                if (movement.IsAttackingDown())
                    playerAnimator.SetTrigger("DownAttack");
                else
                {
                    playerAnimator.SetTrigger("Attacking");
                    movement.PlayDust();
                }
                audioSource.PlayOneShot(hitClip);
                punching = true;
            }
        }
        else
        {
            if (Input.GetButton("Fire"))
            {
                punching = true;
                movement.movementSpeed *= vacumSpeedReduce;
                StopSucking();


                float down = Input.GetAxisRaw("Vertical");

                if (down < 0)
                {
                    playerAnimator.SetTrigger("DownAttack");
                    movement.BounceUpwards();
                    _gem.FireGem(Vector3.down);
                }
                else
                {
                    bool right = movement.gameObject.transform.localScale.x > 0;
                    _gem.FireGem(right ? Vector3.right : Vector3.left);
                }
                audioSource.PlayOneShot(diamondShotClip);
                
                stuck = false;
                _gem = null;
                punching = false;
            }
        }

        if(sucking) {
            audioSource.pitch = GetVacuumSoundPitch();
        }
    }

    private void StartSucking()
    {
        sucking = true;
        movement.movementSpeed *= vacumSpeedReduce;
        _vacuumCollider.enabled = true;
        audioSource.clip = vacuumClip;
        audioSource.loop = true;
        audioSource.pitch = 1;
        audioSource.Play();
        vacuumParticles.Play();
        forceParticles.Play();
        playerAnimator.SetBool("Sucking", true);
    }

    private void StopSucking()
    {
        sucking = false;
        movement.movementSpeed /= vacumSpeedReduce;
        _vacuumCollider.enabled = false;
        timeElapsed = 0;
        audioSource.loop = false;
        audioSource.Stop();
        vacuumParticles.Stop();
        forceParticles.Stop();
        playerAnimator.SetBool("Sucking", false);
    }

   
    public void Stuck(Gem gem)
    {
        stuck = true;
        vacuumParticles.Stop();
        
        movement.movementSpeed /= vacumSpeedReduce;
        _vacuumCollider.enabled = false;

        _gem = gem;
    }

    public void ResetAttack()
    {
        punching = false;
    }

    private float GetVacuumSoundPitch() {
        if (timeElapsed < pitchDuration) {
            float pitch = Mathf.Lerp(1, maxSoundPitch, timeElapsed / pitchDuration);
            timeElapsed += Time.deltaTime;
            return pitch;
        } else {
            return maxSoundPitch;
        }
    }

}
