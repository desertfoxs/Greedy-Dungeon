using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour
{
    [Tooltip("ID de la sala")]
    public int id;

    private GameObject _gameManager;
    private GameManagerAngel _manager;

    void Start()
    {
        _gameManager = GameObject.FindGameObjectWithTag("GameController");
        _manager = _gameManager.GetComponent<GameManagerAngel>();
    }


    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            
            _manager.CambioDeSala(id);

        }

    }
}
