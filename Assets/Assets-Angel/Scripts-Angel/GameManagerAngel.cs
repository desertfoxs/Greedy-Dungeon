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
    public List<GameObject> playerLife;

    private static GameManagerAngel _instance;

    public static GameManagerAngel Instance { get { return _instance; } }

    [Header("Sounds")]
    public AudioClip coinClip;
    public AudioClip deathClip;
    public AudioClip hurtClip;
    private AudioSource audioSource;

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
        TextScore.text = Score.ToString();
        audioSource = GetComponent<AudioSource>();
        //GameObject playerEntity = GameObject.FindGameObjectWithTag("Player");
        //player = playerEntity.GetComponent<Player>();

        //_vcam = GameObject.FindGameObjectWithTag("Vcam");       
        //_followTarget = _vcam.GetComponent<CinemachineVirtualCamera>();

        //_scriptGhost = _ghost.GetComponent<Ghost>();

    }

    private void Update()
    {
        //time += Time.deltaTime;
    
        if (time >= 10f)
        {
            _ghost.SetActive(true);
            _scriptGhost.SalaId(idSala);
        }

        

    }

    public void GrabCoin()
    {
        audioSource.PlayOneShot(coinClip);
        Score++;
        TextScore.text = Score.ToString();
    }

    public void PlayerHurt()
    {
        GameObject life = playerLife[playerLife.Count - 1];
        playerLife.RemoveAt(playerLife.Count - 1);
        audioSource.PlayOneShot(hurtClip);
        Destroy(life);

        if (Score - 5 < 0)
            Score = 0;

        else
            Score -= 5;
        if(playerLife.Count == 0) {
            audioSource.PlayOneShot(deathClip);
        }
        TextScore.text = Score.ToString();
    }

    public void CambioDeSala(int id)
    {
        idSala = id;

        //_followTarget.Follow = postSalas[id];

    }

}
