using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    //define la velocidad del fantasma
    public int speedNormal;
    public int speedRapido;

    private int speed;
    private Rigidbody2D _rb;
    private GameObject _player;
    private PlayerMovement _scriptPlayer;

    //sirven para regular la velocidad del fantasma
    private int _salaPlayer;
    private int _salaGhost = 0;

    private GameObject _gameManager;

    //mecanica de la gema
    private bool _stun = false;
    private SpriteRenderer _spriteRenderer;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _gameManager = GameObject.FindGameObjectWithTag("GameController");

        _spriteRenderer = GetComponent<SpriteRenderer>();

        _player = GameObject.FindGameObjectWithTag("Player");
        _scriptPlayer = _player.GetComponent<PlayerMovement>();
    }

    
    void Update()
    {
        if(_salaGhost < _salaPlayer)
        {           
            speed = speedRapido;
        }
        if(_salaGhost >= _salaPlayer)
        {
            
            speed = speedNormal;
        }

        _rb.AddForce(new Vector2(speed * Time.deltaTime, 0));

    }

    public void SalaId(int idSala)
    {
        _salaPlayer = idSala;
    }

 

    public void OnTriggerEnter2D(Collider2D col)
    {
        if(col.transform.tag == "Detector")
        {
            _salaGhost++;
        }

        if(col.transform.tag == "Player")
        {
            if (!_stun)
            {
                _scriptPlayer.Die();
            }

        }

        if(col.transform.tag == "Gema")
        {
            StartCoroutine(Stuneado(5f));
        }
    }

    protected IEnumerator Stuneado(float Time)
    {
        _stun = true;

        _spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(Time);

        _stun = false;
        _spriteRenderer.color = Color.white;

        yield return null;
    }

}
