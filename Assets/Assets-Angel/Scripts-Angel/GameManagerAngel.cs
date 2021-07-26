using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TMPro;

public class GameManagerAngel : MonoBehaviour
{

    //public Transform[] postSalas;
    private int idSala;

    //private CinemachineVirtualCamera _followTarget;
    //private GameObject _vcam;

    public GameObject _ghost;
    private Ghost _scriptGhost;
    private float time;

    [Header("Score")]
    public static int Score;
    public TextMeshProUGUI TextScore;

    [Header("Scene Name")]
    public string sceneName;

    [Header("Player")]
    //public GameUi gameUi;

    private static GameManagerAngel _instance;

    public static GameManagerAngel Instance { get { return _instance; } }

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    void Start()
    {
        //TextScore.text = Score.ToString();

        //GameObject playerEntity = GameObject.FindGameObjectWithTag("Player");
        //player = playerEntity.GetComponent<Player>();

        //_vcam = GameObject.FindGameObjectWithTag("Vcam");       
        //_followTarget = _vcam.GetComponent<CinemachineVirtualCamera>();

        _scriptGhost = _ghost.GetComponent<Ghost>();

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

    public void GrabCoin()
    {
        Score++;
    }

    public void PlayerHurt()
    {

    }

    public void CambioDeSala(int id)
    {
        idSala = id;

        //_followTarget.Follow = postSalas[id];

    }

}
