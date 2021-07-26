using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Score")]
    public static int Score;
    public TextMeshProUGUI TextScore;

    [Header("Scene Name")]
    public string sceneName;

    [Header("Player")]
    //public GameUi gameUi;

    private static GameManager _instance;

    public static GameManager Instance { get { return _instance; } }

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
    }

    public void GrabCoin()
    {
        Score++;
    }

    public void PlayerHurt()
    {

    }
}
