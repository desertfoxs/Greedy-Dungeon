using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SalaScritp : MonoBehaviour
{
    public GameObject cameraSala;
    [Tooltip("ID de la sala")]
    public int id;

    private GameObject _gameManager;
    private GameManagerAngel _manager;

    private void Start()
    {
        _gameManager = GameObject.FindGameObjectWithTag("GameController");
        _manager = _gameManager.GetComponent<GameManagerAngel>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       

        if (collision.transform.tag == "Player")
        {
            cameraSala.SetActive(true);
            _manager.CambioDeSala(id);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            cameraSala.SetActive(false);
        }
    }
}
