using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameManagerAngel : MonoBehaviour
{

    public Transform[] postSalas;
    private int idSala;

    private CinemachineVirtualCamera _followTarget;
    private GameObject _vcam;

    public GameObject _ghost;
    private Ghost _scriptGhost;
    private float time;

    void Start()
    {
        
        _vcam = GameObject.FindGameObjectWithTag("Vcam");       
        _followTarget = _vcam.GetComponent<CinemachineVirtualCamera>();

        _scriptGhost= _ghost.GetComponent<Ghost>();

    }

    private void Update()
    {
        time += Time.deltaTime;
    
        if (time >= 10f)
        {
            _ghost.SetActive(true);
            _scriptGhost.SalaId(idSala);
        }

        

    }

    public void CambioDeSala(int id)
    {
        idSala = id;

        _followTarget.Follow = postSalas[id];

    }

}
