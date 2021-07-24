using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VacuumLogic : MonoBehaviour
{
    #region properties and fields

    public float vacumSpeedReduce = 0.8f;
    public GameObject vacuumPoint;
    public PlayerMovement movement;

    public ParticleSystem vacuumParticles;

    private PolygonCollider2D _vacuumCollider;

    private bool stuck = false;
    #endregion

    private void Start()
    {
        _vacuumCollider = vacuumPoint.GetComponent<PolygonCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!stuck)
        {
            if (Input.GetButtonDown("Suck"))
            {
                movement.movementSpeed *= vacumSpeedReduce;
                _vacuumCollider.enabled = true;
                vacuumParticles.Play();
            }
            else if (Input.GetButtonUp("Suck"))
            {
                movement.movementSpeed /= vacumSpeedReduce;
                _vacuumCollider.enabled = false;
                vacuumParticles.Stop();
            }
        }
    }
}
