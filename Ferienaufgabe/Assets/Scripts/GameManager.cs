using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ePlayerType { player = 1, ki = 2, none};

public class GameManager : GenericSingletonClass<GameManager>
{
    [Header("Game Values")]
    public float MAX_Y_MOVEMENT = 7.8f;
    public bool GameOver { get; set; }
    public int PointsToWin = 3;
    public int PickupsNeededForNewLife = 5;
    public float MAX_CHARGE_POWER = 300;

    [Space]
    [Header("Ball")]
    public GameObject ballPrefab;

    [Space]
    [Header("Colectables")]
    public GameObject item1Prefab;
    public GameObject item2Prefab;

    [Space]
    [Header("UI Elements")]
    public TMPro.TMP_Text PointsPlayer;
    public TMPro.TMP_Text PointsKi;
    //Son auflade meter dingens
    public TMPro.TMP_Text WinnerText;
    public GameObject GameOverPanel;


    private int currPointsPlayer = 0;
    private int currPointsKi = 0;
    private ePlayerType winner = ePlayerType.none;

    private GameObject instatiatedBall = null;
    

    private void Awake()
    {
        Time.timeScale = 1;
        AudioListener.pause = false;
        //Reset
    }

    public void OnBorderCollision(ePlayerType type)
    {
        switch (type)
        {
            case ePlayerType.player:
                AddPointPlayer();
                break;
            case ePlayerType.ki:
                AddPointKi();
                break;
            default:
                break;
        }
        Destroy(instatiatedBall);
        if (!GameOver)
            instatiatedBall = Instantiate(ballPrefab);
    }

    private void AddPointPlayer()
    {
        currPointsPlayer++;
        if(currPointsPlayer >= PointsToWin)
        {
            winner = ePlayerType.player;
            GameOver = true;
        }
        Debug.Log("Point for Player");
    }

    private void AddPointKi()
    {
        currPointsKi++;
        if(currPointsKi >= PointsToWin)
        {
            winner = ePlayerType.ki;
            GameOver = true;
        }
        Debug.Log("Point for KI");
    }

    public void StartRound()
    {
        instatiatedBall = Instantiate(ballPrefab);
    }

    private void Reset()
    {
        winner = ePlayerType.none;
        currPointsKi = 0;
        currPointsPlayer = 0;
        UpdateUI();
    }

    private void UpdateUI()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        StartRound();
    }

    // Update is called once per frame
    void Update()
    {
        if(GameOver)
        {
            //Show Game Over UI
        }
    }
}
