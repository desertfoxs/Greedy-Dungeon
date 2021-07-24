using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameManagerAngel : MonoBehaviour
{

    public Transform[] postSalas;
    
    private CinemachineVirtualCamera _followTarget;
    private GameObject _vcam;
 

    void Start()
    {
        _vcam = GameObject.FindGameObjectWithTag("Vcam");       
        _followTarget = _vcam.GetComponent<CinemachineVirtualCamera>();


    }


    public void CambioDeSala(int id)
    {
        _followTarget.Follow = postSalas[id];

    }

}
