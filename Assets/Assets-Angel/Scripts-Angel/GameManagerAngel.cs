using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameManagerAngel : MonoBehaviour
{

    public Transform postSalas;
    public CinemachineVirtualCamera followTarget;

    private Vector3 _positionSalas;

    void Start()
    {
        followTarget = GetComponent<CinemachineVirtualCamera>();
        
    }

    // Update is called once per frame
    void Update()
    {
        _positionSalas = new Vector3(postSalas.transform.position.x, postSalas.transform.position.y, postSalas.transform.position.z);

        followTarget.ForceCameraPosition(_positionSalas, transform.rotation);

    }
}
